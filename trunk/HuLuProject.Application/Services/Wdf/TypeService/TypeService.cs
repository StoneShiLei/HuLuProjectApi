using Furion;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Furion.UnifyResult;
using HuLuProject.Application.Services.Wdf.MenuService.Dtos;
using HuLuProject.Application.Services.Wdf.TypeService.Dtos;
using HuLuProject.Core.Entities.Wfd;
using HuLuProject.Core.Managers.Wfd;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.TypeService
{
    /// <summary>
    /// 分类接口
    /// </summary>
    [ApiDescriptionSettings("WhatsForDinner")]
    public class TypeService : BaseService
    {
        private readonly TypeManager typeManager = App.GetRequiredService<TypeManager>();

        /// <summary>
        /// 根据关键字获取分类列表 为空返回全部
        /// </summary>
        /// <param name="text">关键字</param>
        /// <param name="include">是否获取分类下的食谱</param>
        /// <returns></returns>
        [HttpGet, Route("type/list")]
        public async Task<List<TypeOutput>> GetTypeList(string text = "")
        {
            var entitys = await typeManager.GetTypeListAsync(UserId, text, true);
            var result = entitys.Select(e => 
            {
               return new TypeOutput
                {
                    Id = e.Id,
                    TypeName = e.TypeName,
                    UserId = e.UserId,
                    Menus = Mapper.Map<List<MenuOutput>>(e.Menus),
                    MenuCount = e.MenuCount,
                    CreatedTime = e.CreatedTime,
                };
            }).ToList();
            return result;
        }

        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpGet, Route("type/get")]
        public async Task<TypeOutput> GetOne([Required] string typeId)
        {
            var entity = await typeManager.GetOneAsync(typeId);
            var result = Mapper.Map<TypeOutput>(entity);
            return result;
        }

        /// <summary>
        /// 新增或修改分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("type/addOrUpdate")]
        public async Task<bool> AddOrUpdateType([Required, FromBody] TypeInput input)
        {
            //如果修改的名称与原名称一致则直接返回
            var entity = await typeManager.GetOneAsync(input.Id);
            if (string.Equals(input.TypeName, entity?.TypeName)) return true;

            if (await typeManager.IsExistNameAsync(UserId, input.TypeName))
            {
                UnifyContext.Fill(new { Message = $"已存在同名分类：{input.TypeName}" });
                return false;
            }

            //如果id为空 或 id不存在 则新建id  防止id格式非法
            if (string.IsNullOrWhiteSpace(input.Id) || !await typeManager.IsExistAsync(input.Id))
            {
                input.Id = $"TYP-{IDGen.NextID(new { LittleEndianBinary16Format = true, TimeNow = DateTimeOffset.UtcNow })}";
            }

            var entiy = new TypeEntity
            {
                Id = input.Id,
                UserId = UserId,
                TypeName = input.TypeName,
                CreatedTime = DateTime.UtcNow
            };
            var result = await typeManager.AddOrUpdateTypeAsync(entiy);
            return result;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("type/remove")]
        public async Task<bool> RemoveType([Required, FromBody] TypeDeleteInput input)
        {
            var result = await typeManager.RemoveTypeAsync(UserId, input.TypeId);
            if (result) return true;

            UnifyContext.Fill(new { Message = "删除失败，该分类下仍存在菜谱！" });
            return false;
        }
    }
}
