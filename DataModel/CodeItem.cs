using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///CodeItem
	 ///</summary>
	 public class CodeItem
	 {
	 	/// <summary>
        /// itemCode
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = false)]	
		public string itemCode { get; set; }

		/// <summary>
        /// itemName
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string itemName { get; set; }

		/// <summary>
        /// clsCode
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string clsCode { get; set; }

		/// <summary>
        /// unit
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string unit { get; set; }

		/// <summary>
        /// refLower
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string refLower { get; set; }

		/// <summary>
        /// refUpper
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string refUpper { get; set; }

		/// <summary>
        /// sex
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public string sex { get; set; }

		/// <summary>
        /// price
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public decimal price { get; set; }

	 
	 }
}	 
