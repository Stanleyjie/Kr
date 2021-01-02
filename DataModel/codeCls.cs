using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///codeCls
	 ///</summary>
	 public class codeCls
	 {
	 	/// <summary>
        /// clsCode
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = false)]	
		public string clsCode { get; set; }

		/// <summary>
        /// clsName
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string clsName { get; set; }

		/// <summary>
        /// dispOrder
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string dispOrder { get; set; }

	 
	 }
}	 
