using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///MenuRole
	 ///</summary>
	 public class MenuRole
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// menuId
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public int menuId { get; set; }

		/// <summary>
        /// roleId
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public int roleId { get; set; }

	 
	 }
}	 
