using DataModel.Other;
using SqlSugar;
using System.Collections.Generic;

namespace DataModel
{
    ///<summary>
    ///Member
    ///</summary>
    public class Member
    {
        /// <summary>
        /// id
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = false)]
        public int id { get; set; }

        /// <summary>
        /// 微信openid
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string openId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public MemberSex sex { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string phone { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string identityCard { get; set; }

        /// <summary>
        /// 工作单位
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string workUnit { get; set; }

        /// <summary>
        /// 工龄
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? seniority { get; set; }

        /// <summary>
        /// 危害因素
        /// </summary>
        [SugarColumn(IsNullable = true, IsJson = true)]
        public List<string> hagard { get; set; }
    }
}