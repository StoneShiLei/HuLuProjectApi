using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.TypeService.Dtos
{
    public class TypeInput
    {
        /// <summary>
        /// id
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        [JsonProperty("typeName")]
        public string TypeName { get; set; }
    }
}
