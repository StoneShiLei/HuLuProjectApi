using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Web.Core.Filter
{
    /// <summary>
    /// 全局过滤器
    /// </summary>
    public class GlobalFilter : Attribute, IAuthorizationFilter, IResultFilter
    {

        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {

        }

        void IResultFilter.OnResultExecuted(ResultExecutedContext context)
        {

        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext context)
        {

        }
    }
}
