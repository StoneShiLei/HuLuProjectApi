using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.FoodService.Dtos
{
    public class FoodDeleteInput
    {
        /// <summary>
        /// id
        /// </summary>
        [Required]
        [JsonProperty("foodId")]
        public string FoodId { get; set; }
    }
}
