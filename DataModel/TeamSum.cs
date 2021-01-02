using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///TeamSum
	 ///</summary>
	 public class TeamSum
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// 设置号源日期 
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public DateTime? teamDate { get; set; }

		/// <summary>
        /// 设置号源周几 
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? teamWeek { get; set; }

		/// <summary>
        /// 总设置号源数量 
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? teamSum { get; set; }

		/// <summary>
        /// 已用号源 
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? teamAlready { get; set; }

		/// <summary>
        /// 剩余号源 
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? teamSurplus { get; set; }

		/// <summary>
        /// 关联sumtime_Code
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string teamSumtimeCode { get; set; }

		/// <summary>
        /// 关联hospital_Code
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string teamHospitalCode { get; set; }

		/// <summary>
        /// 关联lnc_Code 
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string teamLncCode { get; set; }

		/// <summary>
        /// 是否启用号源 
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string teamFlag { get; set; }

	 
	 }
}	 
