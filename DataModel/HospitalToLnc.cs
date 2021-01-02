using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///HospitalToLnc
	 ///</summary>
	 public class HospitalToLnc
	 {
	 	/// <summary>
        /// 医院Code
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// 医院Code
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string hospitalCode { get; set; }

		/// <summary>
        /// 单位Code 
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string lncCode { get; set; }

		/// <summary>
        /// 是否启用关联 
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string relationFlag { get; set; }

	 
	 }
}	 
