using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Entities.User
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [Table(Name = "user_account")]
    public class UserEntity
    {
        /// <summary>
        /// id
        /// </summary>
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 加密密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Column(CanUpdate = false)]
        public DateTime CreatedTime { get; set; }
    }
}
