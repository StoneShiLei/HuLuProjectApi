using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.MenuService.Dtos
{
    public class MenuDeleteInput
    {
        /// <summary>
        /// id
        /// </summary>
        [Required]
        [JsonProperty("menuId")]
        public string MenuId { get; set; }
    }
}
