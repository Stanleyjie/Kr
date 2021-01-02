using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///HospitalToSumTime
	 ///</summary>
	 public class HospitalToSumTime
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// 医院Code
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string hospitalCode { get; set; }

		/// <summary>
        /// 时间段Code
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string sumtimeCode { get; set; }

		/// <summary>
        /// 是否启用关联
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string relationFlag { get; set; }

	 
	 }
}	 
