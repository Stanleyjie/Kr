using System;
using SqlSugar;
using System.Collections.Generic;
using System.Text;

namespace DataModel
{
	 ///<summary>
	 ///HospitalProjectContrast
	 ///</summary>
	 public class HospitalProjectContrast
	 {
	 	/// <summary>
        /// id
        /// </summary>
		[SugarColumn(IsNullable =false, IsPrimaryKey =true, IsIdentity = true)]	
		public int id { get; set; }

		/// <summary>
        /// hospitalId
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public int hospitalId { get; set; }

		/// <summary>
        /// projectId
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public int projectId { get; set; }

		/// <summary>
        /// tPProjectId
        /// </summary>
		[SugarColumn(IsNullable =false)]
		public int tPProjectId { get; set; }

	 
	 }
}	 
