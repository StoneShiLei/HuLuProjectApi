using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.MenuService.Dtos
{
    public class MenuRandomInput
    {
        /// <summary>
        /// typeId
        /// </summary>
        [JsonProperty("typeId")]
        [Required]
        public string TypeId { get; set; }

        /// <summary>
        /// 出菜数量
        /// </summary>
        [JsonProperty("volume")]
        [Required]
        public int Volume { get; set; }

        /// <summary>
        /// 包含的食材id
        /// </summary>
        [JsonProperty("foodIds")]
        [Required]
        public List<string> FoodIds { get; set; }
    }
}
