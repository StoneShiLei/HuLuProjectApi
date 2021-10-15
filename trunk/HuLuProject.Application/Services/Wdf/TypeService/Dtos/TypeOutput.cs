using HuLuProject.Application.Services.Wdf.MenuService.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.TypeService.Dtos
{
    public class TypeOutput
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
        /// 分类名称
        /// </summary>
        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        /// <summary>
        /// 该分类下的菜谱数量
        /// </summary>
        [JsonProperty("menuCount")]
        public long MenuCount { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 分类下的菜单
        /// </summary>
        [JsonProperty("menus")]
        public List<MenuOutput> Menus { get; set; }
    }
}
