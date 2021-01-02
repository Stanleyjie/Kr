using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///CodeItemCombEntryHosp
	 ///</summary>
	 public class CodeItemCombEntryHosp
	 {
	 	/// <summary>
        /// hospCombCode
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = false)]	
		public string hospCombCode { get; set; }

		/// <summary>
        /// combCode
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = false)]	
		public string combCode { get; set; }

		/// <summary>
        /// price
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public decimal price { get; set; }

		/// <summary>
        /// hospitalCode
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string hospitalCode { get; set; }

	 
	 }
}	 
