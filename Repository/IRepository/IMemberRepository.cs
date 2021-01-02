using DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    /// <summary>
    /// IMemberRepository
    /// </summary>
    public interface IMemberRepository : IBaseRepository<Member>
    {
        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="openIdOrName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <returns></returns>
        Task<(IEnumerable<Member>, int)> GetMemberListAsync(string openIdOrName, int page, int rows);
    }
}