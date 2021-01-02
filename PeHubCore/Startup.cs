using AspectCore.Extensions.Autofac;
using Autofac;
using IdentityServer4.AccessTokenValidation;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using PeHubCore.Config;
using Senparc.CO2NET;
using Senparc.CO2NET.AspNet;
using Senparc.CO2NET.RegisterServices;
using Senparc.Weixin;
using Senparc.Weixin.Entities;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Containers;
using Senparc.Weixin.RegisterServices;
using Senparc.Weixin.TenPay;
using ServiceExt;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PeHubCore
{
    public class Startup
    {
        public static ILoggerRepository repository { get; set; }

        public static IContainer AutofacContainer;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            repository = LogManager.CreateRepository("rollingAppender");
            XmlConfigurator.Configure(repository, new System.IO.FileInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config")));
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson();// 支持 NewtonsoftJson
            //services.AddMvcCore(options => {
            //    options.Filters.Add(typeof(ApiDataFilter)); // by type
            //    //options.Filters.Add(new ApiDataFilter()); // an instance
            //});
            //services.AddMvcCore()
            //.AddAuthorization();

            //缓存
            services.AddMemoryCache();
            //使用session
            services.AddSession();

            #region Ids 授权管理配置 这个方法放在 在ids授权之前

            services.AddIdentityServer(options =>
                {
                    options.PublicOrigin = Appsettings.GetSectionValue("AppSettings:IdSHttpsUrl");
                })
               .AddDeveloperSigningCredential()
               .AddInMemoryIdentityResources(IdsConfig.GetIdentityResources())
               .AddInMemoryApiResources(IdsConfig.GetApis())
               .AddInMemoryClients(IdsConfig.GetClients())
               .AddTestUsers(IdsConfig.GetUsers());

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            #endregion Ids 授权管理配置 这个方法放在 在ids授权之前

            //注意这里把授权和API合并在一起了

            #region 配置api调用 Ids授权 注意这个需要放到前面

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    //这里面需要填写域名
                    //var url = Configuration.GetValue<string>("URLS");
                    var url = Appsettings.GetSectionValue("AppSettings:IdSHttpsUrl");
                    options.Authority = url;
                    options.RequireHttpsMetadata = false;
                    options.Audience = "api1"; //指定只使用哪个作用域，不填又前端控制
                });

            #endregion 配置api调用 Ids授权 注意这个需要放到前面

            #region 配置跨域

            services.AddCors(options => options.AddPolicy("Cors",
                builder =>
                builder.WithOrigins(Appsettings.GetSectionValue("AppSettings:CorsIPs").Split(','))
                .AllowAnyMethod().AllowAnyHeader().AllowCredentials()));
            //builder.WithOrigins("http://localhost:8080").AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

            #endregion 配置跨域

            #region 配置swagger操作文档

            #region 操作版本控制

            //获取api手册版本（可以不用）
            //services.AddApiVersioning(options =>
            //{
            //    options.AssumeDefaultVersionWhenUnspecified = true;
            //    options.DefaultApiVersion = ApiVersion.Default;
            //    options.ReportApiVersions = true;
            //});
            // swagger
            //添加API操作说明
            //services.AddSwaggerGen(option =>
            //{
            //    //option.SwaggerDoc("v1", new OpenApiInfo
            //    //{
            //    //    Version = "v1",
            //    //    Title = "sparktodo API",
            //    //    Description = "API for sparktodo",
            //    //    Contact = new OpenApiContact() { Name = "MIC", Email = "MIC@outlook.com" }
            //    //});

            // //option.SwaggerDoc("v2", new OpenApiInfo { Version = "v2", Title = "API V2" });

            // option.DocInclusionPredicate((docName, apiDesc) => { var versions =
            // apiDesc.CustomAttributes() .OfType<ApiVersionAttribute>() .SelectMany(attr => attr.Versions);

            // return versions.Any(v => $"v{v.ToString()}" == docName); });

            //    // 包含文档文件
            //    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);
            //});
            //    //[ApiVersion("2")] 通过这个绑定

            #endregion 操作版本控制

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{typeof(Startup).Assembly.GetName().Name}.xml"), true);

                // 开启加权小锁
                c.OperationFilter<AddResponseHeadersFilter>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                // 在header中添加token，传递到后台
                c.OperationFilter<SecurityRequirementsOperationFilter>();

                // 必须是 oauth2
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });
            });

            #endregion 配置swagger操作文档

            services.AddHttpClient();//将HttpClient注入进来

            //Senparc.CO2NET 全局注册（必须）
            services.AddSenparcGlobalServices(Configuration);
            //Senparc.Weixin 注册
            services.AddSenparcWeixinServices(Configuration);

            //return AutofacConfigure.Register(services);

            //此处需要在在进行调用接口时在后面加上下划线  _  (一定要加上，后面如果有修改可以查看源码示例代码)
            services.AddCertHttpClient(Appsettings.GetSectionValue("SenparcWeixinSetting:TenPayV3_MchId") + "_",
                Appsettings.GetSectionValue("SenparcWeixinSetting:TenPayV3_MchId"),
                Appsettings.GetSectionValue("SenparcWeixinSetting:TenPayV3_CertPath"));
            //此处可以添加更多 Cert 证书
            services.AddControllersWithViews();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<SenparcSetting> senparcSetting, IOptions<SenparcWeixinSetting> senparcWeixinSetting)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region 读取静态文件

            app.UseStaticFiles();

            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(
            //        Path.Combine(Directory.GetCurrentDirectory(), "MyStaticFiles")),
            //    RequestPath = "/StaticFiles"
            //});

            #endregion 读取静态文件

            app.UseCookiePolicy();

            //使用缓存
            app.UseResponseCaching();

            //注册Session服务
            app.UseSession();

            #region 使用跨域

            app.UseCors("Cors");

            #endregion 使用跨域

            #region 使用swagger

            //使用swagger
            app.UseSwagger();
            //Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                //option.SwaggerEndpoint("/swagger/v2/swagger.json", "API V1");
                option.RoutePrefix = "swagger";
                option.DocumentTitle = "sparktodo API";
            });

            #endregion 使用swagger

            #region 使用Ids服务

            app.UseIdentityServer();

            #endregion 使用Ids服务

            //使用https
            app.UseHttpsRedirection();

            //默认网站打开跳转静态地址
            if (!string.IsNullOrEmpty(Appsettings.GetSectionValue("AppSettings:DefaultHtml")))
            {
                DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
                defaultFilesOptions.DefaultFileNames.Clear();
                defaultFilesOptions.DefaultFileNames.Add(Appsettings.GetSectionValue("AppSettings:DefaultHtml"));
                app.UseDefaultFiles(defaultFilesOptions);
            }

            app.UseStaticFiles();
            app.UseRouting();

            //app.UseMvc();

            #region Senparc.Weixin SDK

            // 启动 CO2NET 全局注册，必须！ 关于 UseSenparcGlobal() 的更多用法见 CO2NET Demo：https://github.com/Senparc/Senparc.CO2NET/blob/master/Sample/Senparc.CO2NET.Sample.netcore3/Startup.cs
            var registerService = app.UseSenparcGlobal(env, senparcSetting.Value, globalRegister =>
            {
                #region CO2NET 全局配置

                #region APM 系统运行状态统计记录配置

                //测试APM缓存过期时间（默认情况下可以不用设置）
                Senparc.CO2NET.APM.Config.EnableAPM = true;//默认已经为开启，如果需要关闭，则设置为 false
                Senparc.CO2NET.APM.Config.DataExpire = TimeSpan.FromMinutes(60);

                #endregion APM 系统运行状态统计记录配置

                #endregion CO2NET 全局配置
            }, true)
                //使用 Senparc.Weixin SDK
                .UseSenparcWeixin(senparcWeixinSetting.Value, weixinRegister =>
                {
                    #region 微信相关配置

                    /* 微信配置开始
                    *
                    * 建议按照以下顺序进行注册，尤其须将缓存放在第一位！
                    */

                    #region 注册公众号或小程序（按需）

                    //注册公众号（可注册多个）                                                    -- DPBMARK MP
                    weixinRegister
                            .RegisterMpAccount(senparcWeixinSetting.Value, "公众号")// DPBMARK_END
                                                                                 //除此以外，仍然可以在程序任意地方注册公众号或小程序：
                                                                                 //AccessTokenContainer.Register(appId, appSecret, name);//命名空间：Senparc.Weixin.MP.Containers

                    #endregion 注册公众号或小程序（按需）

                    #region 注册微信支付（按需）        -- DPBMARK TenPay

                            //注册最新微信支付版本（V3）（可注册多个）
                            .RegisterTenpayV3(senparcWeixinSetting.Value, "公众号")//记录到同一个 SenparcWeixinSettingItem 对象中

                    #endregion 注册微信支付（按需）        -- DPBMARK TenPay

                     ;
                    /* 微信配置结束 */

                    #endregion 微信相关配置
                });

            AccessTokenContainer.RegisterAsync(senparcWeixinSetting.Value.WeixinAppId, senparcWeixinSetting.Value.WeixinAppSecret);

            #endregion Senparc.Weixin SDK

            #region 授权认证ids

            app.UseAuthentication();//认证
            app.UseAuthorization();//授权

            #endregion 授权认证ids

            app.UseMiddleware<Middleware.AskMiddleWare>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //注册socket
            app.Map("/socket", WebSocketHelper.Map);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterTypes(Assembly.Load("Repository").GetExportedTypes()).AsImplementedInterfaces();
            builder.RegisterTypes(Assembly.Load("Service").GetExportedTypes()).AsImplementedInterfaces();
            //builder.RegisterType<ISession>();

            builder.RegisterDynamicProxy();
            // 手动高亮
            builder.RegisterBuildCallback(container => AutofacContainer = container);
        }
    }
}