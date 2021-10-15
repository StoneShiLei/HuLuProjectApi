using HuLuProject.Application.Services.Wdf.FoodService.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.MenuService.Dtos
{
    public class MenuRandomOutput
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
        /// 分类id
        /// </summary>
        [JsonProperty("typeId")]
        public string TypeId { get; set; }

        [JsonProperty("foods")]
        public List<FoodOutput> Foods { get; set; }
    }
}
