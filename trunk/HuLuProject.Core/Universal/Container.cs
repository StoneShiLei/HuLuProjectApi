using Mapster;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.Universal
{
    /// <summary>
    /// 分页容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Container<T>
    {
        /// <summary>
        /// 分页容器总数
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// 分页列表
        /// </summary>
        [JsonProperty("items")]
        public IEnumerable<T> Items { get; set; }
    }
}
