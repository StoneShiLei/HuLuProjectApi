using HuLuProject.Core.Entities.Wfd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Managers.Wfd
{
    public class FoodManager : BaseManager
    {
        public FoodManager() : base() { }

        /// <summary>
        /// 根据关键字查询食材
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchText"></param>
        /// <returns></returns>
        public Task<List<FoodEntity>> GetFoodListAsync(string userId,string searchText = "")
        {
            Expression<Func<FoodEntity, bool>> where = f => f.UserId == userId;

            if (!string.IsNullOrWhiteSpace(searchText)) where = where.And(f => f.FoodName.StartsWith(searchText) || f.FoodName.EndsWith(searchText));

            var result = FreeSql.Select<FoodEntity>()
                .Where(where)
                .OrderByDescending(f => f.CreatedTime)
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="foodId"></param>
        /// <returns></returns>
        public Task<FoodEntity> GetOneAsync(string foodId)
        {
            var result = FreeSql.Select<FoodEntity>().Where(u => u.Id == foodId).ToOneAsync();
            return result;
        }

        /// <summary>
        /// 新增或修改食材
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateFoodAsync(FoodEntity entity)
        {
            var result = await FreeSql
                .InsertOrUpdate<FoodEntity>()
                .SetSource(entity)
                .UpdateColumns(f => new { f.FoodName })
                .ExecuteAffrowsAsync();

            return result > 0;
        }

        /// <summary>
        /// 删除食材
        /// </summary>
        /// <param name="userId">食材所属用户id</param>
        /// <param name="foodId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveFoodAsync(string userId,string foodId)
        {
            using var uow = FreeSql.CreateUnitOfWork();

            var result1 = await uow.Orm.Delete<FoodEntity>().Where(f => f.UserId == userId && f.Id == foodId).ExecuteAffrowsAsync();
            var result2 = await uow.Orm.Delete<MenuFoodEntity>().Where(m => m.FoodId == foodId).ExecuteAffrowsAsync();
            uow.Commit();
            return result1 > 0 || result2 > 0;
        }

        /// <summary>
        /// 判断用户是否已设置同名食材
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="foodName"></param>
        /// <returns></returns>
        public Task<bool> IsExistNameAsync(string userId,string foodName)
        {
            var result = FreeSql.Select<FoodEntity>().AnyAsync(f => f.UserId == userId && f.FoodName == foodName);
            return result;
        }

        /// <summary>
        /// 判断id是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> IsExistAsync(string id)
        {
            var result = FreeSql.Select<FoodEntity>().AnyAsync(o => o.Id == id);
            return result;
        }
    }
}
