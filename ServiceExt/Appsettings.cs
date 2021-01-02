using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceExt
{
    public static class Appsettings
    {
        /// <summary>
        /// 配置文件的跨类库获取
        /// </summary>
        static IConfiguration Configuration { get; set; }

        /// <summary>
        /// 读取appsetting.json里面的配置文件
        /// </summary>
        static Appsettings()
        {
            Configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();
        }

        /// <summary>
        /// 获取配置节信息
        /// </summary>
        /// <param name="section">配置节，层级关系以英文:分隔表示(例：Logging:LogLevel:Default)
        /// </param>
        /// <returns></returns>
        public static string GetSectionValue(string section)
        {
            return Configuration[section];
        }
    }
}
