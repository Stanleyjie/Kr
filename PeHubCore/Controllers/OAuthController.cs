using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DataModel.Other;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;

namespace PeHubCore.Controllers
{
    public class OAuthController : Controller
    {
        protected ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(OAuthController));
        public OAuthController()
        {

        }

        # region vue的获取openid
        /// <summary>
        /// 获取openId
        /// </summary>
        /// <param name="targetUrl">跳转的类型，放回到前端自己处理（是类型key，不是url）</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OAuth(string targetUrl = "")
        {
            ResponsResult model = new ResponsResult();
            // model.returnData = "12345678";
            // model.success = true;
            // return Json(model);
            if (!string.IsNullOrEmpty(getSession<string>("OpenId")))
            {
                model.returnData = getSession<string>("OpenId");
                model.success = true;
                return Json(model);
            }

            #region 通过授权页面获取openid

            //获取openId
            string state = targetUrl;
            string authorizeUrl = OAuthApi.GetAuthorizeUrl(WxConfig.appId, WxConfig.redirectUrl, state, OAuthScope.snsapi_base);
            ;
            model.returnMsg = authorizeUrl;
            model.returnData = authorizeUrl;
            model.success = false;
            return Json(model);

            #endregion
        }

        /// <summary>
        /// 处理授权回调
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult OAuthCallback(string code, string state)
        {

            if (string.IsNullOrEmpty(code))
            {
                return Redirect(WxConfig.NgUrl + "?type=OAuthCallback" + "&success=0" + "&returnMsg=" + "您拒绝了授权!");
            }

            OAuthAccessTokenResult result = new OAuthAccessTokenResult();

            //通过，用code换取access_token
            try
            {
                result = OAuthApi.GetAccessToken(WxConfig.appId, WxConfig.appSecret, code);
            }
            catch (Exception e)
            {
                log.Error("微信认证"+e.Message);
                return Redirect(WxConfig.NgUrl + "?type=OAuthCallback" + "&success=0" + "&returnMsg=" + "服务器繁忙!");
            }
            if (result.errcode != ReturnCode.请求成功)
            {
                return Redirect(WxConfig.NgUrl + "?type=OAuthCallback" + "&success=0" + "&returnMsg=" + result.errmsg);
            }

            if (state.Split('_').Length > 1 && state.Split('_')[0] == "DZ")
            {
                var keyUrl = state.Split('_')[0] + "Url";
                var regno = state.Split('_')[1];

                log.Debug("微信授权地址"+WxConfig.DZUrl + ",regno=" + regno + ",openid=" + result.openid);
                return Redirect(WxConfig.DZUrl + "?no=" + regno + "&openid=" + result.openid);
            }

            //Session["OAuthAccessToken"] = result;
            setSession("OpenId", result.openid);
            log.Debug("微信授权回调地址"+WxConfig.NgUrl + "?type=" + state + "&success=1" + "&returnData=" + result.openid);

            return Redirect(WxConfig.NgUrl + "?type=" + state + "&success=1" + "&returnData=" + HttpUtility.UrlEncode(result.openid));
        }
        #endregion

        #region 第三方默认授权模式
        /// <summary>
        /// 导诊授权
        /// </summary>
        /// <param name="type">type和_和key组合，比如'DZ_19022221221'</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DZOauth(string type)
        {
            string wxurl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + WxConfig.appId +
                "&redirect_uri=" + HttpUtility.UrlEncode(WxConfig.redirectUrl) +
                "&response_type=code&scope=snsapi_userinfo&state=" + type + "&connect_redirect=1#wechat_redirect";
            return Redirect(wxurl);
        }

        #endregion

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
    }
}