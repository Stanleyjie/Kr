using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///PeAdmin
	 ///</summary>
	 public class PeAdmin
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// adminCode
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string adminCode { get; set; }

		/// <summary>
        /// adminName
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string adminName { get; set; }

		/// <summary>
        /// adminPwd
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string adminPwd { get; set; }

		/// <summary>
        /// adminType
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? adminType { get; set; }

		/// <summary>
        /// createDate
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public DateTime? createDate { get; set; }

		/// <summary>
        /// createID
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string createID { get; set; }

		/// <summary>
        /// updateDate
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public DateTime? updateDate { get; set; }

		/// <summary>
        /// updateID
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string updateID { get; set; }

		/// <summary>
        /// loginDate
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public DateTime? loginDate { get; set; }

		/// <summary>
        /// openId
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public string openId { get; set; }

		/// <summary>
        /// state
        /// </summary>
		[SugarColumn(IsNullable =true)]
		public int? state { get; set; }


		//²¹³ä×Ö¶Î
		[SugarColumn(IsIgnore = true)]
		public string token { get; set; }

		//²¹³ä×Ö¶Î ²Ëµ¥
		[SugarColumn(IsIgnore = true)]
		public List<Menu> menudata { get; set; }

	}
}	 
