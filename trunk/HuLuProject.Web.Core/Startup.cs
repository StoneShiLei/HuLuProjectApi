using FreeSql;
using Furion;
using Furion.Cache.Extensions;
using Furion.Extras.DatabaseAccessor.FreeSql.Extensions;
using HuLuProject.Web.Core.Filter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;

namespace HuLuProject.Web.Core
{
    [AppStartup(1000)]
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //Jwt授权实现
            services.AddJwt<JwtHandler>(enableGlobalAuthorize:true);

            //跨域
            services.AddCorsAccessor();

            //swagger newtonJson支持
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddControllersWithViews(m =>
            {
                m.Filters.Add(typeof(GlobalFilter));
            })
            .AddNewtonsoftJson()
            // 配置多语言
            .AddAppLocalization()
            //配置规范化结果
            .AddInjectWithUnifyResult(options =>
            {
                options.SpecificationDocumentConfigure = spt =>
                {
                    spt.SwaggerGenConfigure = gen =>
                    gen.CustomSchemaIds(s => s.FullName);
                };
            });

            var x = App.Configuration["Connection:0:DbKey"];
            //配置freesql多库
            if (!string.IsNullOrWhiteSpace(App.Configuration["Connection:0:DbKey"]))
            {
                var connStrs = new Dictionary<string, KeyValuePair<DataType, string>>();
                foreach (var section in App.Configuration.GetSection("Connection").GetChildren())
                {
                    connStrs.Add(section["DbKey"], KeyValuePair.Create(Enum.Parse<DataType>(section["DbType"]), section["ConnStr"]));
                }

                services.AddFreeSql(connStrs,
                    App.Settings.EnablePrintingLog != true ? null :
                    (string category, string state, string message) =>
                    {
                        App.PrintToMiniProfiler(category, state, message); //打印sql日志委托
                    }
                );
            }

            //配置redis分布式缓存 
            if (!string.IsNullOrWhiteSpace(App.Configuration["Redis:ConnectionString"]))
            {
                services.AddRedisCache(App.Configuration["Redis:ConnectionString"], App.Configuration["Redis:InstanceName"]);
            }


            //注册远程请求客户端
            services.AddRemoteRequest();

            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 添加规范化结果状态码，需要在这里注册
            app.UseUnifyResultStatusCodes();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseInject(string.Empty);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}