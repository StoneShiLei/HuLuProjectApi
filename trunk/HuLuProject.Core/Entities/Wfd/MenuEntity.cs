using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Entities.Wfd
{
    /// <summary>
    /// 食谱
    /// </summary>
    [Table(Name = "wfd_menu")]
    public class MenuEntity
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
        /// 菜谱名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Column(CanUpdate = false)]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 分类id
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 导航 分类
        /// </summary>
        [Navigate(nameof(TypeId))]
        public virtual TypeEntity Type { get; set; }

        /// <summary>
        /// 导航 食材
        /// </summary>
        [Navigate(ManyToMany = typeof(MenuFoodEntity))]
        public virtual List<FoodEntity> Foods { get; set; }
    }
}
