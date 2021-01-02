using DataModel;
using Repository.IRepository;
using SqlSugar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repository
{
    /// <summary>
    /// IMemberRepository
    /// </summary>
    public class MemberRepository : BaseRepository<Member>, IMemberRepository
    {
        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="openIdOrName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<Member>, int)> GetMemberListAsync(string openIdOrName, int page, int rows)
        {
            var exp = Expressionable.Create<Member>();
            if (!string.IsNullOrEmpty(openIdOrName))
            {
                exp.And(m => m.openId.Contains(openIdOrName) || m.name.Contains(openIdOrName));
            }
            var lambda = exp.ToExpression();
            var count = new RefAsync<int>(0);
            var memberList = await _db.Queryable<Member>()
                .Where(lambda)
                .OrderBy(m => m.name)
                .ToPageListAsync(page, rows, count);
            return (memberList, count.Value);
        }
    }
}