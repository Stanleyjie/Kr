using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///codeClusterEntry
	 ///</summary>
	 public class codeClusterEntry
	 {
	 	/// <summary>
        /// clusCode
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = false)]	
		public string clusCode { get; set; }

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

	 
	 }
}	 
