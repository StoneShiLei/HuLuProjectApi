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
    public class TypeManager : BaseManager
    {
        public TypeManager() : base() { }

        /// <summary>
        /// 获取用户的食谱分类列表
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="searchText">搜索关键字</param>
        /// <param name="include">是否贪婪加载type下的菜谱</param>
        /// <param name=""></param>
        /// <returns></returns>
        public Task<List<V_Type>> GetTypeListAsync(string userId,string searchText = "", bool include = false)
        {
            Expression<Func<TypeEntity, bool>> where = t => t.UserId == userId;

            if (!string.IsNullOrWhiteSpace(searchText)) where = where.And(t => t.TypeName.StartsWith(searchText) || t.TypeName.EndsWith(searchText));

            var select = FreeSql.Select<TypeEntity>().Where(where).OrderByDescending(t => t.CreatedTime);
            if (include) select.IncludeMany(t => t.Menus);

            var result = select.ToListAsync(t => new V_Type 
            {
                Id = t.Id,
                Menus = t.Menus,
                TypeName = t.TypeName,
                UserId = t.UserId,
                CreatedTime = t.CreatedTime,
                MenuCount = FreeSql.Select<MenuEntity>().Where(m => m.TypeId == t.Id).Count()
            });

            return result;
        }

        /// <summary>
        /// 新增或修改分类
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateTypeAsync(TypeEntity entity)
        {
            var result = await FreeSql
                .InsertOrUpdate<TypeEntity>()
                .SetSource(entity)
                .UpdateColumns(f => new { f.TypeName })
                .ExecuteAffrowsAsync();

            return result > 0;
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public Task<TypeEntity> GetOneAsync(string typeId)
        {
            var result = FreeSql.Select<TypeEntity>().Where(u => u.Id == typeId).ToOneAsync();
            return result;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="userId">分类所属用户id</param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveTypeAsync(string userId, string typeId)
        {
            using var uow = FreeSql.CreateUnitOfWork();

            //如果该分类下含有子项目  则不允许删除
            var result1 = await uow.Orm.Select<MenuEntity>().AnyAsync(m => m.TypeId == typeId);
            if (result1) return false;

            var result2 = await uow.Orm.Delete<TypeEntity>().Where(f => f.UserId == userId && f.Id == typeId).ExecuteAffrowsAsync();
            uow.Commit();
            return result2 > 0;
        }

        /// <summary>
        /// 判断用户是否已设置同名分类
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public Task<bool> IsExistNameAsync(string userId, string typeName)
        {
            var result = FreeSql.Select<TypeEntity>().AnyAsync(f => f.UserId == userId && f.TypeName == typeName);
            return result;
        }

        /// <summary>
        /// 判断id是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> IsExistAsync(string id)
        {
            var result = FreeSql.Select<TypeEntity>().AnyAsync(o => o.Id == id);
            return result;
        }
    }
}
