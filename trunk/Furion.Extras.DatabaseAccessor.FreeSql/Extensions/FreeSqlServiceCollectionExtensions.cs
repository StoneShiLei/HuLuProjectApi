using FreeSql;
using Furion.Extras.DatabaseAccessor.FreeSql.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Furion.Extras.DatabaseAccessor.FreeSql.Extensions
{
    /// <summary>
    /// freesql 拓展类
    /// </summary>
    public static class FreeSqlServiceCollectionExtensions
    {
        public static IServiceCollection AddFreeSql(this IServiceCollection services, Dictionary<string, KeyValuePair<DataType, string>> dbTypes, Action<string, string, string> printaction)
        {
            var fsql = new MultiFreeSql();
            //注册多库freesql客户端
            foreach (var db in dbTypes)
            {
                fsql.Register(db.Key, () => new FreeSqlBuilder().UseConnectionString(db.Value.Key, db.Value.Value).Build());
            }

            if (printaction != null)
                fsql.Aop.CurdAfter += (s, e) =>
                {
                    printaction?.Invoke("FreeSql", "Info", e.Sql);
                };


            services.AddSingleton<IFreeSql>(fsql);

            //注入仓储
            services.AddFreeRepository();


            return services;
        }
    }
}
