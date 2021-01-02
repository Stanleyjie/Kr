using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///PersonSum
	 ///</summary>
	 public class PersonSum
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
		public DateTime? personDate { get; set; }

		/// <summary>
        /// 设置号源周几
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? personWeek { get; set; }

		/// <summary>
        /// 总设置号源数量
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? personSum { get; set; }

		/// <summary>
        /// 已用号源
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? personAlready { get; set; }

		/// <summary>
        /// 剩余号源
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? personSurplus { get; set; }

		/// <summary>
        /// 设置号源类型
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string personType { get; set; }

		/// <summary>
        /// 关联sumtime_Code
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string personSumtimeCode { get; set; }

		/// <summary>
        /// 关联hospital_Code
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string personHospitalCode { get; set; }

		/// <summary>
        /// 是否启用号源
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string personFlag { get; set; }

	 
	 }
}	 
