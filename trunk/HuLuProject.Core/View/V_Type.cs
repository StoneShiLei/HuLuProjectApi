using HuLuProject.Core.Entities.Wfd;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Core.View
{
    public class V_Type:TypeEntity
    {
        /// <summary>
        /// 分类下的菜单统计数量
        /// </summary>
        public long MenuCount { get; set; }
    }
}
