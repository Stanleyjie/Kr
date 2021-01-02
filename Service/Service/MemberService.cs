using DataModel;
using Repository.IRepository;
using Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Service
{
    public class MemberService : BaseService<Member>, IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository ?? throw new System.ArgumentNullException(nameof(memberRepository));
            _baseRepository = _memberRepository;
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<Member>, int)> GetMemberListAsync(string openIdOrName, int page, int rows)
        {
            var result = await _memberRepository.GetMemberListAsync(openIdOrName, page, rows);
            return result;
        }

        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public Task<bool> UpdateMemberByOpenIdAsync(Member member)
        {
            var memberToUpdate = _memberRepository.FindByClause(m => m.openId == member.openId);
            if (memberToUpdate is null)
            {
                return Task.FromResult(false);
            }
            memberToUpdate.name = member.name;
            memberToUpdate.sex = member.sex;
            memberToUpdate.workUnit = member.workUnit;
            memberToUpdate.seniority = member.seniority;
            memberToUpdate.identityCard = member.identityCard;
            memberToUpdate.hagard = member.hagard ?? Enumerable.Empty<string>().ToList();
            var result = _memberRepository.Update(memberToUpdate);
            return Task.FromResult(result);
        }
    }
}