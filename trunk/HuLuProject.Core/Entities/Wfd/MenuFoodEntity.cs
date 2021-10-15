using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Entities.Wfd
{
    /// <summary>
    /// 菜谱食材关系
    /// </summary>
    [Table(Name = "wfd_menu_food")]
    public class MenuFoodEntity
    {
        /// <summary>
        /// menuId
        /// </summary>
        [Column(IsPrimary = true)]
        public string MenuId { get; set; }

        /// <summary>
        /// foodId
        /// </summary>
        [Column(IsPrimary = true)]
        public string FoodId { get; set; }

        /// <summary>
        /// 菜谱
        /// </summary>
        public virtual MenuEntity Menu { get; set; }

        /// <summary>
        /// 食材
        /// </summary>
        public virtual FoodEntity Food { get; set; }
    }
}
