using DataModel;
using DataModel.Other;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeHubCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : BaseApiController
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService ?? throw new ArgumentNullException(nameof(memberService));
        }

        /// <summary>
        /// 获取会员列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        [HttpPost("GetMemberList")]
        public async Task<IActionResult> GetMemberListAsync([FromBody] encryData data)
        {
            var paramers = data.data;
            (var memberList, var count) = await _memberService.GetMemberListAsync(
                paramers.memberOpenIdOrName,
                paramers.page,
                paramers.rows);
            result.returnData = memberList;
            result.countSum = count;
            return Ok(result);
        }

        /// <summary>
        /// 获取会员性别列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetMemberSexList")]
        public IActionResult GetMemberSexList()
        {
            var sexList = new List<object>();
            foreach (var v in Enum.GetValues(typeof(MemberSex)))
            {
                sexList.Add(new { key = v, label = v.ToString() });
            };
            result.returnData = sexList;
            return Ok(result);
        }

        /// <summary>
        /// 修改会员信息
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost("UpdateMemberByOpenId")]
        public async Task<IActionResult> UpdateMemberByOpenIdAsync([FromBody] encryData data)
        {
            result.success = await _memberService.UpdateMemberByOpenIdAsync(data.MemberData);
            result.returnMsg = result.success ? "修改会员信息成功!" : "修改会员信息失败!";
            return Ok(result);
        }
    }
}