using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.TypeService.Dtos
{
    public class TypeDeleteInput
    {
        /// <summary>
        /// id
        /// </summary>
        [Required]
        [JsonProperty("typeId")]
        public string TypeId { get; set; }
    }
}
