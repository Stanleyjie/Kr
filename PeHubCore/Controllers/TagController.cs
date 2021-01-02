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
    public class TagController : BaseApiController
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService ?? throw new ArgumentNullException(nameof(tagService));
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetTagList")]
        public async Task<IActionResult> GetTagListAsync()
        {
            var tags = await _tagService.GetTagListAsync();
            result.returnData = tags;
            return Ok(result);
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost("CreateTag")]
        public async Task<IActionResult> CreateTagAsync([FromBody] encryData data)
        {
            var tag = await _tagService.CreateTagAsync(data.TagData);
            if (tag != null)
            {
                result.success = true;
                result.returnData = tag;
                result.returnMsg = "创建标签成功!";
            }
            else
            {
                result.success = false;
                result.returnMsg = "创建标签失败!";
            }
            return Ok(result);
        }

        /// <summary>
        /// 修改标签信息
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost("UpdateTag")]
        public async Task<IActionResult> UpdateTagAsync([FromBody] encryData data)
        {
            var success = await _tagService.UpdateTagAsync(data.TagData);
            result.success = success;
            result.returnMsg = success ? "修改标签成功!" : "修改标签失败!";
            return Ok(result);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("DeleteTag")]
        public async Task<IActionResult> DeleteTagAsync([FromBody] encryData data)
        {
            var success = await _tagService.DeleteTagAsync(data.data.tagId);
            result.success = success;
            result.returnMsg = success ? "删除标签成功!" : "删除标签失败!";
            return Ok(result);
        }
    }
}