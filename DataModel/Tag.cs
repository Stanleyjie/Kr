using SqlSugar;

namespace DataModel
{
    ///<summary>
    ///Tag
    ///</summary>
    public class Tag
    {
        /// <summary>
        /// id
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true, IsIdentity = true)]
        public int id { get; set; }

        /// <summary>
        /// name
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string name { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string hospitalCode { get; set; }
    }
}