using FreeSql;
using Furion.FriendlyException;
using HuLuProject.Core.Entities.Wfd;
using HuLuProject.Core.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Managers.Wfd
{
    /// <summary>
    /// 菜谱
    /// </summary>
    public class MenuManager : BaseManager
    {
        public MenuManager() : base() { }

        /// <summary>
        /// 随机查询菜谱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public async Task<List<MenuEntity>> GetRandomMenuListAsync(string userId, List<I_Menu> inputList)
        {
            List<MenuEntity> result = new();
            foreach(var input in inputList)
            {
                string typeId = input.TypeId;
                var volume = input.Volume;
                var foodIds = input.FoodIds;

                if (volume < 1) continue;

                //该用户 某分类下包含某些食材的菜谱
                Expression<Func<MenuEntity, bool>> where = m => m.UserId == userId && m.TypeId == typeId && m.IsEnabled == true;
                if (foodIds != null && foodIds.Any()) where = where.And(m => m.Foods.AsSelect().Any(f => foodIds.Contains(f.Id))); //如果foodIds不为空则查询包含food的菜谱

                //如果请求的数量大于等于符合条件的菜谱数量,则直接返回全部符合条件的结果
                var menuCount = FreeSql.Select<MenuEntity>().Where(where).Count();
                if(volume >= menuCount)
                {
                    var menus = await FreeSql.Select<MenuEntity>()
                        .Where(where)
                        .Include(m => m.Type)
                        .IncludeMany(m => m.Foods)
                        .ToListAsync();
                    result.AddRange(menus);
                    continue;
                }

                //取出全部符合条件的菜谱的id 然后随机取出count的数量  查询出完整结果返回
                var menuIds = await FreeSql.Select<MenuEntity>().Where(where).ToListAsync(m => m.Id);
                List<string> randomIds = new();
                Random rm = new();
                for(int i=0;i<volume;i++)
                {
                    //生成一个不大于menuIds长度的随机数
                    int index = rm.Next(menuIds.Count);
                    randomIds.Add(menuIds[index]);
                    menuIds.RemoveAt(index);
                }
                where = where.And(m => randomIds.Contains(m.Id));
                var randomList = await FreeSql.Select<MenuEntity>().Where(where).Include(m => m.Type).IncludeMany(m => m.Foods).ToListAsync();

                //加入到结果集
                result.AddRange(randomList);
            }

            return result;
        }

        /// <summary>
        /// 查询菜谱列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchText">搜索关键字</param>
        /// <param name="include">是否贪婪加载菜谱的type和food</param>
        /// <returns></returns>
        public Task<List<MenuEntity>> GetMenuListAsync(string userId,string searchText = "",bool include = false)
        {
            Expression<Func<MenuEntity, bool>> where = m => m.UserId == userId;

            if (!string.IsNullOrWhiteSpace(searchText)) where = where.And(m => m.MenuName.StartsWith(searchText) || m.MenuName.EndsWith(searchText));

            var select = FreeSql.Select<MenuEntity>().Where(where).OrderByDescending(m => m.CreatedTime);
            if (include) select.IncludeMany(m => m.Foods).Include(m => m.Type);

            var result = select.ToListAsync();
            return result;
        }

        /// <summary>
        /// 添加菜谱 级联保存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="foodIds"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateMenuAsync(MenuEntity entity,List<string> foodIds)
        {
            using var uow = FreeSql.CreateUnitOfWork();
            var menuRepo = FreeSql.GetRepository<MenuEntity>();
            var foodRepo = FreeSql.GetRepository<FoodEntity>();
            menuRepo.UnitOfWork = uow;
            foodRepo.UnitOfWork = uow;

            //根据foodid查询出该用户的food列表  赋值给菜谱 
            var foods = await foodRepo.Where(f => f.UserId == entity.UserId && foodIds.Contains(f.Id)).ToListAsync();
            entity.Foods = foods;

            var result = await menuRepo.InsertOrUpdateAsync(entity);
            await menuRepo.SaveManyAsync(entity, "Foods");

            uow.Commit();
            return result != null;
        }

        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="state"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<bool> EnableMenuAsync(bool state,string menuId)
        {
            var result = await FreeSql.Update<MenuEntity>().Set(m => m.IsEnabled, state).Where(m => m.Id == menuId).ExecuteAffrowsAsync();
            return result > 0;
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public Task<MenuEntity> GetOneAsync(string menuId)
        {
            var result = FreeSql.Select<MenuEntity>().Where(u => u.Id == menuId).IncludeMany(m => m.Foods).ToOneAsync();
            return result;
        }

        /// <summary>
        /// 删除菜谱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveMenuAsync(string userId,string menuId)
        {
            using var uow = FreeSql.CreateUnitOfWork();

            var result1 = await uow.Orm.Delete<MenuEntity>().Where(m => m.UserId == userId && m.Id == menuId).ExecuteAffrowsAsync();
            var result2 = await uow.Orm.Delete<MenuFoodEntity>().Where(m => m.MenuId == menuId).ExecuteAffrowsAsync();
            uow.Commit();
            return result1 > 0 || result2 > 0;
        }

        /// <summary>
        /// 判断用户是否已设置同名菜谱
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuName"></param>
        /// <returns></returns>
        public Task<bool> IsExistNameAsync(string userId, string menuName)
        {
            var result = FreeSql.Select<MenuEntity>().AnyAsync(m => m.UserId == userId && m.MenuName == menuName);
            return result;
        }

        /// <summary>
        /// 判断id是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> IsExistAsync(string id)
        {
            var result = FreeSql.Select<MenuEntity>().AnyAsync(o => o.Id == id);
            return result;
        }
    }
}
