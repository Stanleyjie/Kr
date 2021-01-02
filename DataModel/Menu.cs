using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///Menu
	 ///</summary>
	 public class Menu
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// name
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string name { get; set; }

		/// <summary>
        /// path
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string path { get; set; }


		/// <summary>
        /// metaTitle
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string metaTitle { get; set; }

		/// <summary>
        /// metaTicon
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string metaTicon { get; set; }

		/// <summary>
        /// sortIndex
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? sortIndex { get; set; }

		/// <summary>
        /// parentId
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? parentId { get; set; }

		/// <summary>
        /// viewPowerID
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? viewPowerID { get; set; }

		/// <summary>
        /// state
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? state { get; set; }


		/// <summary>
		/// 子菜单列表
		/// </summary>
		[SugarColumn(IsNullable = true, IsIgnore = true)]
		public List<Menu> children { get; set; }

		/// <summary>
		/// 元数据对象
		/// </summary>
		[SugarColumn(IsNullable = true, IsIgnore = true)]
		public object meta { get; set; }


	}
}	 
