using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Other
{
    public class ResponsResult
    {
        public ResponsResult()
        {
            success = true;
            returnData = null;
            returnMsg = "";
            pageSum = 0;
            countSum = 0;
        }
        /// <summary>
        /// 返回状态代码0 成功,其他失败
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 调用错误返回信息
        /// </summary>
        public string returnMsg { get; set; }

        /// <summary>
        /// 调用成功后返回信息
        /// </summary>
        public object returnData { get; set; }

        /// <summary>
        /// 多少页
        /// </summary>
        public int pageSum { get; set; }

        /// <summary>
        /// 多少条
        /// </summary>
        public int countSum { get; set; }
    }
}
