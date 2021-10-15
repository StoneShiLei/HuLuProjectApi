using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.MenuService.Dtos
{
    /// <summary>
    /// 列表页模型
    /// </summary>
    public class MenuListOutput
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonProperty("userId")]
        public string UserId { get; set; }

        /// <summary>
        /// 菜谱名称
        /// </summary>
        [JsonProperty("menuName")]
        public string MenuName { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        /// <summary>
        /// 食材名称 顿号隔开
        /// </summary>
        [JsonProperty("foodNames")]
        public string FoodNames { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [JsonProperty("isEnabled")]
        public bool IsEnabled { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }
    }
}
