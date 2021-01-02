using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel;
using DataModel.Other;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.IRepository;
using Service.IService;

namespace PeHubCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class CodeItemController : BaseApiController
    {
        private readonly ICodeItemService _codeItemService;
        public CodeItemController(ICodeItemService codeItemService)
        {
            _codeItemService = codeItemService ?? throw new ArgumentNullException(nameof(codeItemService));
        }

        #region 获取项目列表
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCodeItemList")]
        public async Task<IActionResult> GetCodeItemList()
        {
            var items = await _codeItemService.GetCodeItemList();
            result.returnData = items;
            return Ok(result);
        }
        #endregion

        #region 项目添加
        /// <summary>
        /// 项目添加
        /// </summary>
        /// <returns></returns>
        [HttpPost("CodeItemAdd")]
        public async Task<IActionResult> CodeItemAdd([FromBody] encryData data)
        {
            var item = await _codeItemService.CodeItemAdd(data.ItemData);
            if (item != null)
            {
                result.success = true;
                result.returnData = item;
                result.returnMsg = "项目添加成功!";
            }
            else
            {
                result.success = false;
                result.returnMsg = "项目添加失败!";
            }
            return Ok(result);
        }
        #endregion

        #region 项目修改
        /// <summary>
        /// 项目修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("CodeItemUpdate")]
        public async Task<IActionResult> CodeItemUpdate([FromBody] encryData data)
        {
            var success = await _codeItemService.CodeItemUpdate(data.ItemData);
            result.success = success;
            result.returnMsg = success ? "项目修改成功!" : "项目修改失败!";
            return Ok(result);
        }
        #endregion

        #region 删除项目
        /// <summary>
        /// 删除项目
        /// </summary>
        /// <returns></returns>
        [HttpPost("CodeItemDel")]
        public async Task<IActionResult> CodeItemDel([FromBody] encryData data)
        {
            var success = await _codeItemService.CodeItemDel(data.ItemData.itemCode);
            result.success = success;
            result.returnMsg = success ? "项目删除成功!" : "项目删除失败!";
            return Ok(result);
        }
        #endregion

    }
}
