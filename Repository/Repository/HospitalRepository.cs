using DataModel;
using DataModel.Other;
using Repository.IRepository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repository
{
    /// <summary>
    /// IHospitalRepository
    /// </summary>
    public class HospitalRepository : BaseRepository<Hospital>, IHospitalRepository
    {
        /// <summary>
        /// 根据医院代码获取医院信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<Hospital> GetHospitalByCodeAsync(string code)
        {
            var hospital = FindByClause(h => h.code == code);
            if (hospital != null)
            {
                var tags = ExecuteSelectQuery<Tag>(@"
                                select * from HospitalTag ht
                                left join Tag t on ht.tagId=t.id
                                where ht.hospitalCode =@code", new { code });
                hospital.Tags = tags;
            }
            return Task.FromResult(hospital);
        }

        /// <summary>
        /// 获取医院列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<Hospital>, int)> GetHospitalListAsync(int page, int rows, string codeOrName, HospitalLevel? level)
        {
            var exp = Expressionable.Create<Hospital>();
            if (!string.IsNullOrEmpty(codeOrName))
            {
                exp.And(h => h.name.Contains(codeOrName) || h.code == codeOrName);
            }
            if (level != null && Enum.IsDefined(typeof(HospitalLevel), level))
            {
                exp.And(h => h.level == level);
            }
            var lambda = exp.ToExpression();
            var count = new RefAsync<int>(0);
            var hospitalList = await _db.Queryable<Hospital>().Where(lambda).OrderBy(h => h.id).ToPageListAsync(page, rows, count);
            if (hospitalList.Any())
            {
                var codes = hospitalList.Select(h => h.code).ToArray();
                var tags = ExecuteSelectQuery<Tag>(@"
                                select
                                ht.hospitalCode,
                                t.id,t.name
                                from HospitalTag ht
                                left join Tag t on ht.tagId=t.id
                                where ht.hospitalCode in (@codes)", new { codes });
                foreach (var hospital in hospitalList)
                {
                    hospital.Tags = tags.Where(t => t.hospitalCode == hospital.code);
                }
            }
            return (hospitalList, count.Value);
        }
    }
}