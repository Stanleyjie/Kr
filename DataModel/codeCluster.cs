using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///codeCluster
	 ///</summary>
	 public class codeCluster
	 {
	 	/// <summary>
        /// clusCdoe
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = false)]	
		public string clusCode { get; set; }

		/// <summary>
        /// clusName
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string clusName { get; set; }

		/// <summary>
        /// price
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public decimal price { get; set; }

		/// <summary>
        /// sex
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string sex { get; set; }

		/// <summary>
        /// clusNote
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string clusNote { get; set; }

	 
	 }
}	 
