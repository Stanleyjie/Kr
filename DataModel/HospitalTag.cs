using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///HospitalTag
	 ///</summary>
	 public class HospitalTag
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// 医院代码
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string hospitalCode { get; set; }

		/// <summary>
        /// 标签id
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public int tagId { get; set; }

	 
	 }
}	 
