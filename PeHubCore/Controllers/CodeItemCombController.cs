using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataModel;
using DataModel.Other;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Service.IService;

namespace PeHubCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeItemCombController : BaseApiController
    {
        private readonly ICodeItemCombService _codeItemCombService;
        private readonly IcodeItemCombRepository _codeItemCombRepository;
        private readonly IcodeItemCombEntryRepository _codeItemCombEntryRepository;
        private readonly ICodeItemCombEntryHospRepository _codeItemCombEntryHospRepository;

        public CodeItemCombController(ICodeItemCombService codeItemCombService, IcodeItemCombRepository codeItemCombRepository,
            IcodeItemCombEntryRepository codeItemCombEntryRepository, ICodeItemCombEntryHospRepository codeItemCombEntryHospRepository)
        {
            _codeItemCombService = codeItemCombService;
            _codeItemCombRepository = codeItemCombRepository;
            _codeItemCombEntryRepository = codeItemCombEntryRepository;
            _codeItemCombEntryHospRepository = codeItemCombEntryHospRepository;
        }

        #region 获取组合列表
        /// <summary>
        /// 获取组合列表
        /// </summary>
        /// <param name="combCode"></param>
        /// <returns></returns>
        [HttpPost("GetCodeItemCombList")]
        public async Task<IActionResult> GetCodeItemCombList()
        {
            var combs = await _codeItemCombService.GetCodeItemCombList();
            result.returnData = combs;
            return Ok(result);
        }
        #endregion

        #region 组合添加
        /// <summary>
        /// 组合添加
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("CodeItemCombAdd")]
        public async Task<IActionResult> CodeItemCombAdd([FromBody] encryData data)
        {
            var item = await _codeItemCombService.CodeItemCombAdd(data.CodeItemCombData);
            if (item != null)
            {
                result.success = true;
                result.returnData = item;
                result.returnMsg = "组合添加成功!";
            }
            else
            {
                result.success = false;
                result.returnMsg = "组合添加失败!";
            }
            return Ok(result);
        }
        #endregion

        #region 组合修改
        /// <summary>
        /// 组合修改
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("CodeItemCombUpdate")]
        public async Task<IActionResult> CodeItemCombUpdate([FromBody] encryData data)
        {
            var success = await _codeItemCombService.CodeItemCombUpdate(data.CodeItemCombData);
            result.success = success;
            result.returnMsg = success ? "组合修改成功!" : "组合修改失败!";
            return Ok(result);
        }
        #endregion

        #region 删除组合
        /// <summary>
        /// 删除组合
        /// </summary>
        /// <returns></returns>
        [HttpPost("CodeItemCombDel")]
        public async Task<IActionResult> CodeItemCombDel([FromBody] encryData data)
        {
            var success = await _codeItemCombService.CodeItemCombDel(data.CodeItemCombData.combCode);
            result.success = success;
            result.returnMsg = success ? "组合删除成功!" : "组合删除失败!";
            return Ok(result);
        }
        #endregion

        #region 获取项目-组合关联表列表
        /// <summary>
        /// 获取项目-组合关联表列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCodeItemCombEntryList")]
        public async Task<IActionResult> GetCodeItemCombEntryList()
        {
            try
            {
                result.returnData = await Task.Factory.StartNew(() => _codeItemCombService.GetCodeItemCombEntryList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "获取项目组合关系表列表失败";
            }
            return Ok(result);
        }
        #endregion

        #region 获取项目-组合关联表详情
        /// <summary>
        /// 获取项目-组合关联表详情
        /// </summary>
        /// <param name="combCode"></param>
        /// <returns></returns>
        [HttpPost("GetCodeItemCombEntryDetail")]
        public async Task<IActionResult> GetCodeItemCombEntryDetail(string combCode)
        {
            try
            {
                result.returnData = await Task.Factory.StartNew(() => _codeItemCombService.GetCodeItemCombEntryDetail(combCode));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "获取项目组合关系列表失败";
            }
            return Ok(result);
        }
        #endregion

        #region 添加项目-组合关系
        /// <summary>
        /// 添加项目-组合关系
        /// </summary>
        /// <param name="cice"></param>
        /// <returns></returns>
        [HttpPost("CodeItemCombEntryAdd")]
        public async Task<IActionResult> CodeItemCombEntryAdd(codeItemCombEntry cice)
        {
            try
            {
                //判断关系是否存在
                var codeItemCombEntryn = _codeItemCombEntryRepository.FindByClause(o => o.combCode == cice.combCode);
                //如果不存在关系则添加，存在则修改
                if (codeItemCombEntryn == null)
                {
                    codeItemCombEntry cicen = new codeItemCombEntry();
                    cicen.combCode = cice.combCode;
                    cicen.clsCode = cice.clsCode;
                    cicen.itemCode = cice.itemCode;
                    await Task.Factory.StartNew(() => _codeItemCombEntryRepository.Insert(cicen));
                    result.returnMsg = "成功添加关系";
                }
                else
                {
                    codeItemCombEntryn.combCode = cice.combCode;
                    codeItemCombEntryn.clsCode = cice.clsCode;
                    codeItemCombEntryn.itemCode = cice.itemCode;
                    await Task.Factory.StartNew(() => _codeItemCombEntryRepository.Update(codeItemCombEntryn));
                    result.returnMsg = "成功修改关系";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "添加关系失败";
            }
            return Ok(result);
        }
        #endregion

        #region 删除项目-组合关系
        /// <summary>
        /// 删除项目-组合关系
        /// </summary>
        /// <param name="combCode"></param>
        /// <returns></returns>
        [HttpPost("CodeItemCombEntryDel")]
        public async Task<IActionResult> CodeItemCombEntryDel(string combCode)
        {
            try
            {
                await Task.Factory.StartNew(() => _codeItemCombService.CodeItemCombEntryDel(combCode));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后在试";
            }
            return Ok(result);
        }
        #endregion

        #region 获取医院-组合列表
        /// <summary>
        /// 获取医院-组合列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetCodeItemCombEntryHospList")]
        public async Task<IActionResult> GetCodeItemCombEntryHospList()
        {
            try
            {
                result.returnData = await Task.Factory.StartNew(() => _codeItemCombService.GetCodeItemCombEntryHospList());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "获取列表失败";
            }
            return Ok(result);
        }
        #endregion

        #region 获取医院-组合详情
        /// <summary>
        /// 获取医院-组合详情
        /// </summary>
        /// <param name="hospCombCode"></param>
        /// <returns></returns>
        [HttpPost("GetCodeItemCombEntryHospDetail")]
        public async Task<IActionResult> GetCodeItemCombEntryHospDetail(string hospCombCode)
        {
            try
            {
                result.returnData = await Task.Factory.StartNew(() => _codeItemCombService.GetCodeItemCombEntryHospDetail(hospCombCode));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "获取详情失败";
            }
            return Ok(result);
        }
        #endregion

        #region 添加医院-组合关系
        /// <summary>
        /// 添加医院-组合关系
        /// </summary>
        /// <param name="cceh"></param>
        /// <returns></returns>
        [HttpPost("CodeItemCombEntryHospAdd")]
        public async Task<IActionResult> CodeItemCombEntryHospAdd(CodeItemCombEntryHosp cceh)
        {
            try
            {
                //判断是否存在关系
                var CodeItemCombEntry = _codeItemCombEntryHospRepository.FindByClause(o => o.hospCombCode == cceh.hospCombCode);
                //如果不存在则新增，存在则修改
                if (CodeItemCombEntry == null)
                {
                    CodeItemCombEntryHosp ccehn = new CodeItemCombEntryHosp();
                    ccehn.hospCombCode = cceh.hospCombCode;
                    ccehn.combCode = cceh.combCode;
                    ccehn.price = cceh.price;
                    ccehn.hospitalCode = cceh.hospitalCode;
                    await Task.Factory.StartNew(() => _codeItemCombEntryHospRepository.Insert(ccehn));
                    result.returnMsg = "成功添加关系";
                }
                else
                {
                    CodeItemCombEntry.hospCombCode = cceh.hospCombCode;
                    CodeItemCombEntry.combCode = cceh.combCode;
                    CodeItemCombEntry.price = cceh.price;
                    CodeItemCombEntry.hospitalCode = cceh.hospitalCode;
                    await Task.Factory.StartNew(() => _codeItemCombEntryHospRepository.Update(cceh));
                    result.returnMsg = "成功修改关系";
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后在试";
            }
            return Ok(result);
        }
        #endregion

        #region 删除医院-组合关系
        /// <summary>
        /// 删除医院-组合关系
        /// </summary>
        /// <param name="hospCombCode"></param>
        /// <returns></returns>
        [HttpPost("CodeItemCombEntryHospDel")]
        public async Task<IActionResult> CodeItemCombEntryHospDel(string hospCombCode)
        {
            try
            {
                await Task.Factory.StartNew(() => _codeItemCombService.CodeItemCombEntryHospDel(hospCombCode));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后在试";
            }
            return Ok(result);
        }
        #endregion
    }
}
