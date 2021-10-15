using Furion;
using Furion.DistributedIDGenerator;
using Furion.FriendlyException;
using Furion.UnifyResult;
using HuLuProject.Application.Services.Wdf.FoodService.Dtos;
using HuLuProject.Core.Entities.Wfd;
using HuLuProject.Core.Managers.Wfd;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services.Wdf.FoodService
{
    /// <summary>
    /// 食材接口
    /// </summary>
    [ApiDescriptionSettings("WhatsForDinner")]
    public class FoodService : BaseService
    {
        private readonly FoodManager foodManager = App.GetRequiredService<FoodManager>();

        /// <summary>
        /// 根据关键字获取食材列表 为空返回全部
        /// </summary>
        /// <param name="text">关键字</param>
        /// <returns></returns>
        [HttpGet, Route("food/list")]
        public async Task<List<FoodOutput>> GetFoodList(string text = "")
        {
            var entitys = await foodManager.GetFoodListAsync(UserId, text);
            var result = Mapper.Map<List<FoodOutput>>(entitys);
            return result;
        }

        /// <summary>
        /// 查询一条数据
        /// </summary>
        /// <param name="foodId"></param>
        /// <returns></returns>
        [HttpGet, Route("food/get")]
        public async Task<FoodOutput> GetOne([Required] string foodId)
        {
            var entity = await foodManager.GetOneAsync(foodId);
            var result = Mapper.Map<FoodOutput>(entity);
            return result;
        }

        /// <summary>
        /// 新增或修改食材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("food/addOrUpdate")]
        public async Task<bool> AddOrUpdateFood([Required,FromBody]FoodInput input)
        {
            //如果修改的名称与原名称一致则直接返回
            var entity = await foodManager.GetOneAsync(input.Id);
            if (string.Equals(input.FoodName, entity?.FoodName)) return true;

            if(await foodManager.IsExistNameAsync(UserId,input.FoodName))
            {
                UnifyContext.Fill(new { Message = $"已存在同名食材：{input.FoodName}" });
                return false;
            }

            //如果id为空 或 id不存在 则新建id  防止id格式非法
            if(string.IsNullOrWhiteSpace(input.Id) || !await foodManager.IsExistAsync(input.Id))
            {
                input.Id = $"FOD-{IDGen.NextID(new { LittleEndianBinary16Format = true, TimeNow = DateTimeOffset.UtcNow })}";
            }

            var entiy = new FoodEntity
            {
                Id = input.Id,
                UserId = UserId,
                FoodName = input.FoodName,
                CreatedTime = DateTime.UtcNow
            };
            var result = await foodManager.AddOrUpdateFoodAsync(entiy);
            return result;
        }

        /// <summary>
        /// 删除食材
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost, Route("food/remove")]
        public Task<bool> RemoveFood([Required,FromBody] FoodDeleteInput input)
        {
            var result = foodManager.RemoveFoodAsync(UserId, input.FoodId);
            return result;
        }
    }
}
