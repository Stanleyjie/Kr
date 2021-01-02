using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DataModel.Other;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Service.IService;

namespace PeHubCore.Controllers
{
    //[ApiController]
    [Route("base")]
    public class BaseApiController : ControllerBase
    {
        //其他全局的事情
        private static IContainer Container { get; set; }
        protected ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(BaseApiController));
        protected ResponsResult result = new ResponsResult();

        //protected   ICommonService _commonService;
        protected   IMemoryCache _cacheService;
        public BaseApiController()
        {
            using (var scope = Startup.AutofacContainer.BeginLifetimeScope())
            {
                //_commonService = scope.Resolve<ICommonService>();
            }
        }

        /// <summary>
        /// 设置session(使用前先注入sessionService)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected bool setSession(string key, object obj)
        {
            try
            {
                string jsonstr = JsonConvert.SerializeObject(obj);
                byte[] byteArray = Encoding.Default.GetBytes(jsonstr);
                HttpContext.Session.Set(key, byteArray);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取session (使用前先注入sessionService)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        protected T getSession<T>(string key)
        {
            try
            {
                byte[] byteArray;
                HttpContext.Session.TryGetValue(key, out byteArray);
                if (byteArray == null)
                {
                    return default(T);
                }
                string jsonstr = Encoding.Default.GetString(byteArray, 0, byteArray.Length);
                return JsonConvert.DeserializeObject<T>(jsonstr);
            }
            catch (Exception e)
            {
                return default(T);
            }
        }

        /// <summary>
        /// 访问后台管理
        /// </summary>
        /// <returns></returns>
        [HttpGet("admin")]
        public RedirectResult PeAdminLogin()
        {
            return Redirect("/admin/index.html");
        }

    }
}