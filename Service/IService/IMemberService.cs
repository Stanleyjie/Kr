using DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IMemberService : IBaseService<Member>
    {
        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        Task<(IEnumerable<Member>, int)> GetMemberListAsync(string openIdOrName, int page, int rows);

        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        Task<bool> UpdateMemberByOpenIdAsync(Member member);
    }
}