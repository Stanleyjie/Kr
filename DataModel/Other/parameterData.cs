//using Newtonsoft.Json;
using Newtonsoft.Json;
using ServiceExt;
using System;

namespace DataModel.Other
{
    public class parameterData
    {
        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string idCard { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string tel { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }

        /// <summary>
        /// </summary>
        public string smscode { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 微信用户id
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// id 前端请求路由表向后端传参
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string kw { get; set; }

        public string status { get; set; }
        public int state { get; set; }

        public object[] ids { get; set; }

        public string data { get; set; }
        public string token { get; set; }

        public string startDate { get; set; }
        public string endDate { get; set; }

        public string password { get; set; }

        public int page { get; set; }

        public int rows { get; set; }

        public HospitalLevel? hospitalLevel { get; set; }

        public string hospitalAndCodeOrName { get; set; }
        public string memberOpenIdOrName { get; set; }
        public int tagId { get; set; }
    }

    public class commonSum
    {
        public string type { get; set; }

        public int start { get; set; }

        public int end { get; set; }

        public string date_Time { get; set; }
    }

    public class TokenParData
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public class encryData
    {
        private string _encryStr;
        private string serializeStr;

        //public dynamic data;
        public parameterData data;

        public commonSum SumData;
        public TokenParData TokenData;
        public PeAdmin PeAdminData;
        public PeRole PeRoleData;
        public Hospital HospitalData;
        public Tag TagData;
        public Member MemberData;
        public CodeItem ItemData;
        public codeItemCombEntry ItemCombEntryData;
        public codeItemComb CodeItemCombData;
        public codeCluster codeClusterData;

        public string encryStr
        {
            get { return _encryStr; }
            set
            {
                this.serializeStr = SecurityHelper.AESDecrypt(value, "0123456789abcdef");
                if (string.IsNullOrEmpty(serializeStr))
                {
                    throw new Exception("加密失败");
                }
                //this.data = JsonConvert.DeserializeObject<dynamic>(serializeStr);
                this.data = JsonConvert.DeserializeObject<parameterData>(serializeStr);
                this.SumData = JsonConvert.DeserializeObject<commonSum>(serializeStr);
                this.TokenData = JsonConvert.DeserializeObject<TokenParData>(serializeStr);
                this.PeAdminData = JsonConvert.DeserializeObject<PeAdmin>(serializeStr);
                this.PeRoleData = JsonConvert.DeserializeObject<PeRole>(serializeStr);
                this.HospitalData = JsonConvert.DeserializeObject<Hospital>(serializeStr);
                this.TagData = JsonConvert.DeserializeObject<Tag>(serializeStr);
                this.MemberData = JsonConvert.DeserializeObject<Member>(serializeStr);
                this.ItemData = JsonConvert.DeserializeObject<CodeItem>(serializeStr);
                this.ItemCombEntryData = JsonConvert.DeserializeObject<codeItemCombEntry>(serializeStr);
                this.CodeItemCombData = JsonConvert.DeserializeObject<codeItemComb>(serializeStr);
                this.codeClusterData = JsonConvert.DeserializeObject<codeCluster>(serializeStr);
                this._encryStr = value;
            }
        }
    }
}