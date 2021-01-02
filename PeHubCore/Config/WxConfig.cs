using ServiceExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeHubCore
{
    /// <summary>
    /// 配置文件配置管理
    /// </summary>
    public class WxConfig
    {
        /// <summary>
        /// 应用ID
        /// </summary>
        public static string appId = Appsettings.GetSectionValue("SenparcWeixinSetting:WeixinAppId")?? "";

        /// <summary>
        /// 应用密钥
        /// </summary>
        public static string appSecret = Appsettings.GetSectionValue("SenparcWeixinSetting:WeixinAppSecret") ?? "";

        /// <summary>
        /// 商户号
        /// </summary>
        public static string mchId = Appsettings.GetSectionValue("SenparcWeixinSetting:TenPayV3_MchId") ?? "";

        /// <summary>
        /// 签名方式
        /// </summary>
        public static string signType = Appsettings.GetSectionValue("SenparcWeixinSetting:signType") ?? "";

        /// <summary>
        /// 域名
        /// </summary>
        public static string domain = Appsettings.GetSectionValue("SenparcWeixinSetting:domain") ?? "";

        /// <summary>
        /// 微信支付结果通知的回调地址
        /// </summary>
        public static string notifyUrl = string.Format(Appsettings.GetSectionValue("SenparcWeixinSetting:notifyUrl") ?? "", domain);

        /// <summary>
        /// 支付回调
        /// </summary>
        public static string regPayNotifyUrl = string.Format(Appsettings.GetSectionValue("SenparcWeixinSetting:regPayNotifyUrl") ?? "", domain);

        /// <summary>
        /// 授权回调地址
        /// </summary>
        public static string redirectUrl = string.Format(Appsettings.GetSectionValue("SenparcWeixinSetting:redirectUrl") ?? "", domain);

        //跳转授权
        public static string DZUrl = Appsettings.GetSectionValue("SenparcWeixinSetting:DZUrl") ?? "";

         

        /// <summary>
        /// 授权回调到前端授权地址
        /// </summary>
        public static string NgUrl = Appsettings.GetSectionValue("SenparcWeixinSetting:NgUrl") ?? "";

        /// <summary>
        /// API密钥
        /// </summary>
        //健康E互
        //public static string key = "56abbe56e056f20f883ee10adc3849ba";
        //康软科技测试号1
        // public static string key = "u6emvg0howigok86xqmd257akvqp8kbg";
        //康软科技测试号2

        public static string key = Appsettings.GetSectionValue("SenparcWeixinSetting:TenPayV3_Key") ?? "";


        /// <summary>
        /// 是否测试支付(商品描述:test,金额:0.01)
        /// </summary>
        public static bool payTest = Appsettings.GetSectionValue("SenparcWeixinSetting:payTest") == "true" ? true : false;

        /// <summary>
        /// 证书路径
        /// </summary>
        public static string certPath = Appsettings.GetSectionValue("SenparcWeixinSetting:TenPayV3_CertPath") ?? "";

        
        /// <summary>
        /// 订单超时时间间隔 单位：分钟
        /// </summary>
        public static int TimeOutMin = Convert.ToInt32(Appsettings.GetSectionValue("SenparcWeixinSetting:TimeOutMin") ?? "0");

        public static string IP = Appsettings.GetSectionValue("SenparcWeixinSetting:wxpayIP")?? "";
    }
}
