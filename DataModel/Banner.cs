using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///Banner
	 ///</summary>
	 public class Banner
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// imageUrl
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string imageUrl { get; set; }

		/// <summary>
        /// href
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string href { get; set; }

		/// <summary>
        /// title
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string title { get; set; }

	 
	 }
}	 
