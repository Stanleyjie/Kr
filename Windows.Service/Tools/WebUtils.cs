using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Windows.Service
{
    /// <summary>
    /// 网络工具类。
    /// </summary>
    public sealed class WebUtils
    {
        private int _timeout = 100000;

        /// <summary>
        /// 请求与响应的超时时间
        /// </summary>
        public int Timeout
        {
            get { return this._timeout; }
            set { this._timeout = value; }
        }

        #region  HttpWebRequest 请求方法
        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>HTTP响应</returns>
        public string DoPost(string url, IDictionary<string, string> parameters, string charset)
        {
            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;
            req.Timeout = 120000;//毫秒
            byte[] postData = Encoding.GetEncoding(charset).GetBytes(BuildQuery(parameters, charset));
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsStringNew(rsp, encoding);
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>HTTP响应</returns>
        public HttpWebResponse DoPostRsp(string url, IDictionary<string, string> parameters, string charset)
        {
            HttpWebRequest req = GetWebRequest(url, "POST");
            req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;
            req.Timeout = 120000;//毫秒
            byte[] postData = Encoding.GetEncoding(charset).GetBytes(BuildQuery(parameters, charset));
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            return rsp;
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>HTTP响应</returns>
        public string DoGet(string url, IDictionary<string, string> parameters, string charset)
        {
            try
            {
                if (parameters != null && parameters.Count > 0)
                {
                    if (url.Contains("?"))
                    {
                        url = url + "&" + BuildQuery(parameters, charset);
                    }
                    else
                    {
                        url = url + "?" + BuildQuery(parameters, charset);
                    }
                }

                HttpWebRequest req = GetWebRequest(url, "GET");
                req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;

                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                return GetResponseAsString(rsp, encoding);
            }
            catch (Exception e)
            {
                LogHelper.Error("处理作业任务失败:" + e.Message, "" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return null;
            }
        }

        public HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "Aop4Net";
            req.Timeout = this._timeout;
            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);

                // 按字符读取并写入字符串缓冲
                int ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    // 过滤结束符
                    char c = (char)ch;
                    if (c != '\0')
                    {
                        result.Append(c);
                    }
                }
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }

            return result.ToString();
        }

        /// <summary>
        /// 把流转文本（没有双引号）
        /// </summary>
        /// <param name="response"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public string GetResponseAsStringNew(HttpWebResponse response, Encoding encoding)
        {
            string strMsg = string.Empty;
            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), encoding))
                {
                    strMsg = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch
            { }
            return strMsg;
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters, string charset)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");

                    string encodedValue = HttpUtility.UrlEncode(value, Encoding.GetEncoding(charset));

                    postData.Append(encodedValue);
                    hasParam = true;
                }
            }

            return postData.ToString();
        }
        #endregion

        #region httpClient请求
        public string ClientGet(string url,Dictionary<string,string> pdata,string charse,string Keys="")
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.Timeout = TimeSpan.FromSeconds(120);
            //httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            url += BuildQuery(pdata, charse);
            if (!string.IsNullOrEmpty(Keys))
            {
               httpClient.DefaultRequestHeaders.Add("ClientInfo", Keys);
            }
            HttpResponseMessage response = httpClient.GetAsync(new Uri(url)).Result;
            //确保HTTP成功状态值
            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        public  string ClientPost(string url, Dictionary<string, string> pdata,string charse,string Keys="")
        {
            HttpClient httpClient = new HttpClient();
            httpClient.MaxResponseContentBufferSize = 256000;
            httpClient.Timeout = TimeSpan.FromSeconds(120);
            //httpClient.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/36.0.1985.143 Safari/537.36");
            //httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           
            var content = new FormUrlEncodedContent(pdata);
            //content.Headers.ContentType= new MediaTypeHeaderValue("application/json");
            content.Headers.ContentType.CharSet = charse;
            if (!string.IsNullOrEmpty(Keys))
            {
                content.Headers.Add("ClientInfo", Keys);
            }
            HttpResponseMessage response = httpClient.PostAsync(url, content).Result;

            //确保HTTP成功状态值
            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;
            return result;


            //HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(new { Name = "董哲", IdCard = "340402199505110015",Tel="188888888218888" }));
            //httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //httpContent.Headers.ContentType.CharSet = "utf-8";
            //HttpClient httpClient = new HttpClient();
            //HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            //////确保HTTP成功状态值
            //response.EnsureSuccessStatusCode();
            //string result = response.Content.ReadAsStringAsync().Result;
            //return result;



        }
        #endregion
    }
}
