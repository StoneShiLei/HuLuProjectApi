using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.FoodService.Dtos
{
    public class FoodOutput
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
        /// 食材名称
        /// </summary>
        [JsonProperty("foodName")]
        public string FoodName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }
    }
}
