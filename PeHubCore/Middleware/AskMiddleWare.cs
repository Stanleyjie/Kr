using log4net;
using Microsoft.AspNetCore.Http;
using ServiceExt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PeHubCore.Middleware
{
    /// <summary>
    /// 中间件 记录请求和响应数据
    /// </summary>
    public class AskMiddleWare
    {
        protected ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(AskMiddleWare));

        /// <summary>
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// </summary>
        /// <param name="next"></param>
        public AskMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if ((Appsettings.GetSectionValue("Middleware:AskMiddleWare") ?? "").ToUpper() == "TRUE")
            {
                // 过滤，只有接口
                if (context.Request.Path.Value.Contains("api"))
                {
                    context.Request.EnableBuffering();
                    var olnycode = context.TraceIdentifier;
                    Stream originalBody = context.Response.Body;

                    try
                    {
                        // 存储请求数据
                        await RequestDataLog(context, olnycode);

                        using (var ms = new MemoryStream())
                        {
                            context.Response.Body = ms;

                            await _next(context);

                            // 存储响应数据
                            ResponseDataLog(context.Response, ms, olnycode);

                            ms.Position = 0;
                            await ms.CopyToAsync(originalBody);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("***");
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("***");

                        // 记录异常
                        //ErrorLogData(context.Response, ex);
                    }
                    finally
                    {
                        context.Response.Body = originalBody;
                    }
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task RequestDataLog(HttpContext context, string code)
        {
            var request = context.Request;
            var sr = new StreamReader(request.Body);

            var content = $" QueryData:{request.Path + request.QueryString}\r\n BodyData:{await sr.ReadToEndAsync()}";

            if (!string.IsNullOrEmpty(content))
            {
                Parallel.For(0, 1, e =>
                {
                    log.Info("ASK-" + code + "输入数据\r\n" + content);
                });

                request.Body.Position = 0;
            }
        }

        private void ResponseDataLog(HttpResponse response, MemoryStream ms, string code)
        {
            ms.Position = 0;
            var ResponseBody = new StreamReader(ms).ReadToEnd();

            // 去除 Html
            var reg = "<[^>]+>";
            var isHtml = Regex.IsMatch(ResponseBody, reg);

            if (!string.IsNullOrEmpty(ResponseBody))
            {
                Parallel.For(0, 1, e =>
                {
                    log.Info("ASK-" + code + "输出数据\r\n" + ResponseBody);
                });
            }
        }
    }
}