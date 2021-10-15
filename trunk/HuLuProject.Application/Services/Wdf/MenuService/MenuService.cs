using Furion;
using Furion.DistributedIDGenerator;
using Furion.UnifyResult;
using HuLuProject.Application.Services.Wdf.FoodService.Dtos;
using HuLuProject.Application.Services.Wdf.MenuService.Dtos;
using HuLuProject.Core.Entities.Wfd;
using HuLuProject.Core.Managers.Wfd;
using HuLuProject.Core.Universal;
using HuLuProject.Core.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.MenuService
{
    /// <summary>
    /// 菜谱接口
    /// </summary>
    [ApiDescriptionSettings("WhatsForDinner")]
    public class MenuService : BaseService
    {
        private readonly MenuManager menuManager = App.GetRequiredService<MenuManager>();


        /// <summary>
        /// 随机出菜 post查询
        /// </summary>
        /// <param name="inputList">查询条件</param>
        /// <returns></returns>
        [HttpPost, Route("menu/random")]
        public async Task<List<MenuRandomOutput>> GetRandomMenuList([FromBody] List<MenuRandomInput> inputList)
        {
            if (!inputList.Any()) return new List<MenuRandomOutput>();

            var entitys = await menuManager.GetRandomMenuListAsync(UserId, Mapper.Map<List<I_Menu>>(inputList));

            var result = entitys.Select(e => new MenuRandomOutput
            {
                Id = e.Id,
                UserId = e.UserId,
                MenuName = e.MenuName,
                TypeName = e.Type.TypeName,
                TypeId = e.TypeId,
                Foods = Mapper.Map<List<FoodOutput>>(e.Foods)
            }).ToList();

            return result;
        }

        /// <summary>
        /// 根据关键字获取菜谱列表（包含食材） 为空返回全部
        /// </summary>
        /// <param name="text">关键字</param>
        /// <returns></returns>
        [HttpGet, Route("menu/list")]
        public async Task<List<MenuListOutput>> GetMenuList(string text = "")
        {
            var entitys = await menuManager.GetMenuListAsync(UserId, text,true);

            var result = entitys.Select(e => new MenuListOutput
            { 
                Id = e.Id,
                UserId = e.UserId,
                MenuName = e.MenuName,
                TypeName = e.Type.TypeName,
                FoodNames = string.Join('、',e.Foods.Select(i => i.FoodName)),
                CreatedTime = e.CreatedTime,
                IsEnabled = e.IsEnabled
            }).ToList();

            return result;
        }

        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet, Route("menu/get")]
        public async Task<MenuOutput> GetOne([Required] string menuId)
        {
            var entity = await menuManager.GetOneAsync(menuId);
            var result = new MenuOutput
            {
                Id = entity.Id,
                UserId = entity.UserId,
                TypeId = entity.TypeId,
                MenuName = entity.MenuName,
                FoodIds = entity.Foods.Select(f => f.Id).ToList()
            };
            return result;
        }

        /// <summary>
        /// 新增或修改菜谱 允许同名菜谱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("menu/addOrUpdate")]
        public async Task<bool> AddOrUpdateMenu([Required, FromBody] MenuInput input)
        {

            //如果id为空 或 id不存在 则新建id  防止id格式非法
            if (string.IsNullOrWhiteSpace(input.Id) || !await menuManager.IsExistAsync(input.Id))
            {
                input.Id = $"MEU-{IDGen.NextID(new { LittleEndianBinary16Format = true, TimeNow = DateTimeOffset.UtcNow })}";
            }

            var entity = new MenuEntity
            {
                Id = input.Id,
                UserId = UserId,
                MenuName = input.MenuName,
                TypeId = input.TypeId,
                CreatedTime = DateTime.UtcNow,
                IsEnabled = true
            };
            var result = await menuManager.AddOrUpdateMenuAsync(entity,input.FoodIds);
            return result;
        }

        /// <summary>
        /// 修改菜谱启用状态
        /// </summary>
        /// <param name="state">开启状态</param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpGet, Route("menu/enable")]
        public async Task<bool> EnableMenu([Required] bool state,[Required]string menuId)
        {
            var result = await menuManager.EnableMenuAsync(state, menuId);
            return result;
        }

        /// <summary>
        /// 删除菜谱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("menu/remove")]
        public Task<bool> RemoveMenu([Required, FromBody] MenuDeleteInput input)
        {
            var result = menuManager.RemoveMenuAsync(UserId, input.MenuId);
            return result;
        }
    }
}
