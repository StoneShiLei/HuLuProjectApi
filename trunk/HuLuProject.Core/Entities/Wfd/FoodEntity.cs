using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Entities.Wfd
{
    /// <summary>
    /// 食材
    /// </summary>
    [Table(Name = "wfd_food")]
    public class FoodEntity
    {
        /// <summary>
        /// id
        /// </summary>
        [Column(IsPrimary = true)]
        public string Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 食材名称
        /// </summary>
        public string FoodName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column( CanUpdate = false)]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 导航 菜谱
        /// </summary>
        [Navigate(ManyToMany = typeof(MenuFoodEntity))]
        public virtual List<MenuEntity> Menus { get; set; }
    }
}
