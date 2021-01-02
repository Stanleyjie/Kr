using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///codeItemCombEntry
	 ///</summary>
	 public class codeItemCombEntry
	 {
	 	/// <summary>
        /// combCode
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = false)]	
		public string combCode { get; set; }

		/// <summary>
        /// clsCode
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string clsCode { get; set; }

		/// <summary>
        /// itemCode
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = false)]	
		public string itemCode { get; set; }

	 
	 }
}	 
