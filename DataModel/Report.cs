using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///Report
	 ///</summary>
	 public class Report
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// userId
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string userId { get; set; }

		/// <summary>
        /// hospitalId
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string hospitalId { get; set; }

		/// <summary>
        /// content
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string content { get; set; }

	 
	 }
}	 
