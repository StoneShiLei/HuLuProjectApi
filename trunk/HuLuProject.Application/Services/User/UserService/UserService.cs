using Furion;
using Furion.DataEncryption;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Furion.UnifyResult;
using HuLuProject.Application.Services.User.UserService.Dtos;
using HuLuProject.Core.Entities.User;
using HuLuProject.Core.Managers.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.User.UserService
{
    /// <summary>
    /// 用户鉴权
    /// </summary>
    [ApiDescriptionSettings("User")]
    public class UserService : BaseService
    {
        private readonly UserManager userManager = App.GetRequiredService<UserManager>();
        private readonly string AesKey = App.Configuration["AppSettings:AesKey"];

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet, Route("user/login"), AllowAnonymous]
        public async ValueTask<bool> Login([Required]string userName, [Required]string password)
        {
            password = DESCEncryption.Encrypt(password,AesKey);
            var user = await userManager.CheckUserAsync(userName, password);
            if (user == null) 
            {
                UnifyContext.Fill(new { Message = "用户名或密码错误" });
                return false;
            }

            var payload = new Dictionary<string, object>()
            {
                { "userId",user.Id},{ "userName",user.UserName }
            };
            var accessToken = JWTEncryption.Encrypt(payload);
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, 43200);

            App.HttpContext.Response.Headers["access-token"] = accessToken;
            App.HttpContext.Response.Headers["x-access-token"] = refreshToken;
            return true;
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("user/register"), AllowAnonymous]
        public async Task<bool> Register([Required,FromBody] UserRegisterInput input)
        {
            if(await userManager.IsExistAsync(input.UserName))
            {
                UnifyContext.Fill(new { Message = "用户名已注册" });
                return false;
            }

            input.PassWord = DESCEncryption.Encrypt(input.PassWord, AesKey);
            var user = new UserEntity
            {
                Id = $"USR-{IDGen.NextID(new { LittleEndianBinary16Format = true, TimeNow = DateTimeOffset.UtcNow })}",
                UserName = input.UserName,
                PassWord = input.PassWord,
                CreatedTime = DateTime.UtcNow,
            };
            return await userManager.AddOrUpdateUserAsync(user);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("user/modify")]
        public async Task<bool> Modify([Required,FromBody] UserInput input)
        {
            if (!string.Equals(UserId, input.Id)) throw Oops.Oh("非法操作：与登陆用户UserID不一致");
            if (await userManager.IsExistAsync(input.UserName))
            {
                UnifyContext.Fill(new { Message = "用户名已注册或与现用户名相同" });
                return false;
            }

            input.PassWord = DESCEncryption.Encrypt(input.PassWord, AesKey);
            var user = new UserEntity
            {
                Id = input.Id,
                UserName = input.UserName,
                PassWord = input.PassWord
            };
            return await userManager.AddOrUpdateUserAsync(user);
        }

        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet, Route("user/get")]
        public async Task<UserOutput> GetOne([Required] string userId)
        {
            if (!string.Equals(UserId, userId)) throw Oops.Oh("非法操作：与登陆用户UserID不一致");
            var entity = await userManager.GetOneAsync(userId);
            var result = Mapper.Map<UserOutput>(entity);
            return result;
        }

        /// <summary>
        /// 用于前端检查用户登陆状态
        /// </summary>
        /// <returns></returns>
        [HttpGet,Route("user/check")]
        public ValueTask<bool> CheckUserAuth()
        {
            return ValueTask.FromResult(true);
        }
    }
}
