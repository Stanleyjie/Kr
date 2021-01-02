using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///codeItemComb
	 ///</summary>
	 public class codeItemComb
	 {
	 	/// <summary>
        /// combCode
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = false)]	
		public string combCode { get; set; }

		/// <summary>
        /// combName
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string combName { get; set; }

		/// <summary>
        /// clsCode
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string clsCode { get; set; }

		/// <summary>
        /// price
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public decimal price { get; set; }

		/// <summary>
        /// deptCode
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string deptCode { get; set; }

		/// <summary>
        /// hospitalCode
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string hospitalCode { get; set; }

	 
	 }
}	 
