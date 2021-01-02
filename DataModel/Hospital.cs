using DataModel.Other;
using SqlSugar;
using System.Collections.Generic;

namespace DataModel
{
    ///<summary>
    ///Hospital
    ///</summary>
    public class Hospital
    {
        /// <summary>
        /// id
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string name { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        [SugarColumn(IsNullable = true, IsJson = true)]
        public List<string> imageUrls { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public HospitalLevel level { get; set; }

        /// <summary>
        /// 预约时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string appointmentTime { get; set; }

        /// <summary>
        /// 预约通知
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string notification { get; set; }

        /// <summary>
        /// 详情
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string details { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string city { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string district { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string detailAddress { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public float? longitude { get; set; }

        /// <summary>
        /// 维度
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public float? dimensionality { get; set; }

        /// <summary>
        /// 医院代码
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string code { get; set; }

        [SugarColumn(IsIgnore = true)]
        public IEnumerable<Tag> Tags { get; set; }
    }
}