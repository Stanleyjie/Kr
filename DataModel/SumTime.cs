using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///SumTime
	 ///</summary>
	 public class SumTime
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// 时间段编号
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string sumtimeCode { get; set; }

		/// <summary>
        /// 时间段
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string sumtimeName { get; set; }

		/// <summary>
        /// 时间段开始时间
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string sumtimeBegTime { get; set; }

		/// <summary>
        /// 时间段结束时间
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string sumtimeEndTime { get; set; }

		/// <summary>
        /// 时间段类型
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string sumtimeType { get; set; }

		/// <summary>
        /// 是否启用时间段
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string sumtimeFlag { get; set; }

	 
	 }
}	 
