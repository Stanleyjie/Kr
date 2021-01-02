using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///PeRole
	 ///</summary>
	 public class PeRole
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// roleName
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string roleName { get; set; }

		/// <summary>
        /// description
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string description { get; set; }

		/// <summary>
        /// createDate
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public DateTime? createDate { get; set; }

		/// <summary>
        /// createId
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string createId { get; set; }

		/// <summary>
        /// updateDate
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public DateTime? updateDate { get; set; }

		/// <summary>
        /// updateId
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string updateId { get; set; }

		/// <summary>
        /// state
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? state { get; set; }

	 
	 }
}	 
