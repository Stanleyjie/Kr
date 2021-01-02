using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using DataModel;
using DataModel.Other;

namespace PeHubCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeClusterController : BaseApiController
    {
        private readonly ICodeClusterService _codeClusterService;
        private readonly IcodeClusterRepository _codeClusterRepository;
        private readonly IcodeClusterEntryRepository _codeClusterEntryRepository;
        public CodeClusterController(ICodeClusterService codeClusterService, IcodeClusterRepository codeClusterRepository,
            IcodeClusterEntryRepository codeClusterEntryRepository)
        {
            _codeClusterService = codeClusterService;
            _codeClusterRepository = codeClusterRepository;
            _codeClusterEntryRepository = codeClusterEntryRepository;
        }
        #region 获取套餐列表
        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCodeClusterList")]
        public async Task<IActionResult> GetCodeClusterList()
        {
            var clusters = await _codeClusterService.GetCodeClusterList();
            result.returnData = clusters;
            return Ok(result);
        }
        #endregion

        #region 套餐添加
        /// <summary>
        /// 套餐添加
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("CodeClusterAdd")]
        public async Task<IActionResult> CodeClusterAdd([FromBody] encryData data)
        {
            var item = await _codeClusterService.CodeClusterAdd(data.codeClusterData);
            if (item != null)
            {
                result.success = true;
                result.returnData = item;
                result.returnMsg = "套餐添加成功!";
            }
            else
            {
                result.success = false;
                result.returnMsg = "套餐添加失败!";
            }
            return Ok(result);
        }
        #endregion

        #region 套餐修改
        /// <summary>
        /// 套餐修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("CodeClusterUpdate")]
        public async Task<IActionResult> CodeClusterUpdate([FromBody] encryData data)
        {
            var success = await _codeClusterService.CodeClusterUpdate(data.codeClusterData);
            result.success = success;
            result.returnMsg = success ? "套餐修改成功!" : "套餐修改失败!";
            return Ok(result);
        }
        #endregion

        #region 套餐删除
        /// <summary>
        /// 套餐删除
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("CodeClusterDel")]
        public async Task<IActionResult> CodeClusterDel([FromBody] encryData data)
        {
            var success = await _codeClusterService.CodeClusterDel(data.codeClusterData.clusCode);
            result.success = success;
            result.returnMsg = success ? "套餐删除成功!" : "套餐删除失败!";
            return Ok(result);
        }
        #endregion

        #region 获取组合-套餐关系列表
        /// <summary>
        /// 获取组合-套餐关系列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCodeClusterEntryList")]
        public async Task<IActionResult> GetCodeClusterEntryList()
        {
            try
            {
                await Task.Factory.StartNew(() => _codeClusterService.GetCodeClusterEntryList());
                result.returnMsg = "成功获取组合-套餐关系列表";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "获取组合-套餐关系列表失败";
            }
            return Ok(result);
        }
        #endregion

        #region 获取组合-套餐关系详情
        /// <summary>
        /// 获取组合-套餐关系详情
        /// </summary>
        /// <param name="clusCode"></param>
        /// <returns></returns>
        [HttpPost("GetCodeClusterEntryDetail")]
        public async Task<IActionResult> GetCodeClusterEntryDetail(string clusCode)
        {
            try
            {
                await Task.Factory.StartNew(() => _codeClusterService.GetCodeClusterEntryDetail(clusCode));
                result.returnMsg = "成功获取组合-套餐关系详情";
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "获取组合-套餐关系详情失败";
            }
            return Ok(result);
        }
        #endregion

        #region 添加组合-套餐关系
        /// <summary>
        /// 添加组合-套餐关系
        /// </summary>
        /// <param name="cce"></param>
        /// <returns></returns>
        [HttpPost("CodeClusterEntryAdd")]
        public async Task<IActionResult> CodeClusterEntryAdd(codeClusterEntry cce)
        {
            try
            {
                //判断是否已存在关系
                var CodeClusterEntry = await Task.Factory.StartNew(() => _codeClusterEntryRepository.FindByClause(o => o.clusCode == cce.clusCode));
                //如果不存在则添加，存在则修改
                if (CodeClusterEntry == null)
                {
                    codeClusterEntry ccen = new codeClusterEntry();
                    ccen.clusCode = cce.clusCode;
                    ccen.combCode = cce.combCode;
                    ccen.price = cce.price;
                    await Task.Factory.StartNew(() => _codeClusterEntryRepository.Insert(ccen));
                    result.returnMsg = "成功添加组合-套餐关系";
                }
                else
                {
                    CodeClusterEntry.clusCode = cce.clusCode;
                    CodeClusterEntry.combCode = cce.combCode;
                    CodeClusterEntry.price = cce.price;
                    await Task.Factory.StartNew(() => _codeClusterEntryRepository.Update(cce));
                    result.returnMsg = "成功修改组合-套餐关系";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "添加/修改组合-套餐关系失败";
            }
            return Ok(result);
        }
        #endregion

        #region 删除组合-套餐关系
        /// <summary>
        /// 删除组合-套餐关系
        /// </summary>
        /// <param name="clusCode"></param>
        /// <returns></returns>
        [HttpPost("CodeClusterEntryDel")]
        public async Task<IActionResult> CodeClusterEntryDel(string clusCode)
        {
            try
            {
                await Task.Factory.StartNew(() => _codeClusterService.CodeClusterEntryDel(clusCode));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "删除组合-套餐关系失败";
            }
            return Ok(result);
        }
        #endregion
    }
}
