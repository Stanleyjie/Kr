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
    public class HospitalController : BaseApiController
    {
        private readonly IHospitalService _hospitalService;

        public HospitalController(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService ?? throw new ArgumentNullException(nameof(hospitalService));
        }

        /// <summary>
        /// 获取医院列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHospitalList")]
        public async Task<IActionResult> GetHospitalListAsync([FromBody] encryData data)
        {
            var paramers = data.data;
            (var hospitalList, var count) = await _hospitalService.GetHospitalListAsync(paramers.page, paramers.rows, paramers.hospitalAndCodeOrName, paramers.hospitalLevel);
            result.returnData = hospitalList;
            result.countSum = count;
            return Ok(result);
        }

        /// <summary>
        /// 获取医院等级列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetHospitalLevelList")]
        public IActionResult GetHospitalLevelList()
        {
            var levelList = new List<object>();
            foreach (var v in Enum.GetValues(typeof(HospitalLevel)))
            {
                levelList.Add(new { key = v, label = v.ToString() });
            };
            result.returnData = levelList;
            return Ok(result);
        }

        /// <summary>
        /// 根据医院代码获取医院信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost("GetHospitalByCode")]
        public async Task<IActionResult> GetHospitalByCodeAsync([FromBody] encryData data)
        {
            var hospital = await _hospitalService.GetHospitalByCodeAsync(data.HospitalData.code);
            result.returnData = hospital;
            return Ok(result);
        }

        /// <summary>
        /// 创建医院
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("CreateHospital")]
        public async Task<IActionResult> CreateHospitalAsync([FromBody] encryData data)
        {
            var hospital = await _hospitalService.CreateHospitalAsync(data.HospitalData);
            if (hospital != null)
            {
                result.success = true;
                result.returnData = hospital;
                result.returnMsg = "创建医院成功!";
            }
            else
            {
                result.success = false;
                result.returnMsg = "创建医院失败!";
            }
            return Ok(result);
        }

        /// <summary>
        /// 根据医院代码修改医院信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("UpdateHospitalByCode")]
        public async Task<IActionResult> UpdateHospitalByCodeAsync([FromBody] encryData data)
        {
            var success = await _hospitalService.UpdateHospitalByCodeAsync(data.HospitalData);
            result.success = success;
            result.returnMsg = success ? "修改医院成功!" : "修改医院失败!";
            return Ok(result);
        }

        /// <summary>
        /// 根据医院代码删除医院
        /// </summary>
        /// <param name="hospitalCode"></param>
        /// <returns></returns>
        [HttpPost("DeleteHospitalByCode")]
        public async Task<IActionResult> DeleteHospitalByCodeAsync([FromBody] encryData data)
        {
            var success = await _hospitalService.DeleteHospitalByCodeAsync(data.HospitalData.code);
            result.success = success;
            result.returnMsg = success ? "删除医院成功!" : "删除医院失败!";
            return Ok(result);
        }
    }
}