using Furion;
using Furion.Cache;
using Furion.DynamicApiController;
using MapsterMapper;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HuLuProject.Application.Services
{
    public abstract class BaseService : IDynamicApiController
    {
        protected readonly IMapper Mapper = App.GetRequiredService<IMapper>();
        protected readonly IMemoryCache MemoryCache = App.GetRequiredService<IMemoryCache>();
        protected readonly RedisCacheHelper RedisCache = App.GetService<RedisCacheHelper>();
        protected readonly string UserId = App.User?.FindFirstValue("userid");
    }
}
