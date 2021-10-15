using Furion.Authorization;
using Furion.DataEncryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace HuLuProject.Web.Core
{
    /// <summary>
    /// jwt授权实现
    /// </summary>
    public class JwtHandler : AppAuthorizeHandler
    {
        /// <summary>
        ///  添加自动刷新token逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            // 自动刷新 token
            if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext()))
            {
                await AuthorizeHandleAsync(context);
            }
            else context.Fail();    // 授权失败
        }

        public override Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            // 这里写您的授权判断逻辑，授权通过返回 true，否则返回 false

            return Task.FromResult(true);
        }
    }
}