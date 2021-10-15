using HuLuProject.Core.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Managers.User
{
    /// <summary>
    /// 用户接口
    /// </summary>
    public class UserManager : BaseManager
    {
        public UserManager() : base() { }

        /// <summary>
        /// 检查用户凭据是否有效
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="enryptionPassWord">aes加密的密码</param>
        /// <returns></returns>
        public async Task<UserEntity> CheckUserAsync(string userName,string enryptionPassWord)
        {
            var result = await FreeSql.Select<UserEntity>().Where(u => u.UserName == userName && u.PassWord == enryptionPassWord).ToOneAsync();
            return result;
        }

        /// <summary>
        /// 新增或修改用户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> AddOrUpdateUserAsync(UserEntity entity)
        {
            var result = await FreeSql
                .InsertOrUpdate<UserEntity>()
                .SetSource(entity)
                .UpdateColumns(u => new { u.UserName,u.PassWord })
                .ExecuteAffrowsAsync();
            return result > 0;
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<UserEntity> GetOneAsync(string userId)
        {
            var result = FreeSql.Select<UserEntity>().Where(u => u.Id == userId).ToOneAsync();
            return result;
        }

        /// <summary>
        /// 判断用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> IsExistAsync(string userName)
        {
            var result = await FreeSql.Select<UserEntity>().AnyAsync(u => u.UserName == userName);
            return result;
        }
    }
}

