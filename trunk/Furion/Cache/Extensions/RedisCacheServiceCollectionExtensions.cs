using Furion;
using Furion.Cache;
using Furion.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furion.Cache.Extensions
{
    /// <summary>
    /// Redis 服务拓展类
    /// </summary>
    [SuppressSniffer]
    public static class RedisCacheServiceCollectionExtensions
    {
        /// <summary>
        /// 注入Redis缓存
        /// </summary>
        /// <param name="services"></param>
        /// <param name="redisConnectionString"></param>
        /// <param name="redisInstanceName"></param>
        /// <returns></returns>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, string redisConnectionString, string redisInstanceName)
        {
            services.AddSingleton(new RedisCacheHelper(redisConnectionString, redisInstanceName));
            return services;
        }
    }
}
