using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DataModel;
using DataModel.Other;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.IService;
using ServiceExt;

namespace PeHubCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SystemSettingController : BaseApiController
    {
        private readonly IPeAdminService _peAdminService;
        private readonly IMenuService _menuService;


        public SystemSettingController(IMenuService menuService, IPeAdminService peAdminService)
        {
            _menuService = menuService;
            _peAdminService = peAdminService;
        }

        /// <summary>
        /// 前端登录
        /// </summary>
        /// <param name="pdata"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]encryData pData)
        {
            try
            {

                PeAdmin admin = await Task.Factory.StartNew(() => _peAdminService.FindByClause(x => (x.adminCode ?? "").ToUpper() == pData.data.code.ToUpper()));
                if (admin == null)
                {
                    result.success = false;
                    result.returnMsg = "不存在该管理员";
                    return Ok(result);
                }
                if (admin.adminPwd.ToUpper() != SecurityHelper.ToMD5((pData.data.password ?? "").Trim()).Substring(8, 16).ToUpper())
                {
                    result.success = false;
                    result.returnMsg = "密码不正确";
                    return Ok(result);
                }
                if (admin.state == 0)
                {
                    result.success = false;
                    result.returnMsg = "当前账号已被停用，请联系管理员";
                    return Ok(result);
                }


                var client = new HttpClient();

                var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = Appsettings.GetSectionValue("AppSettings:IdSHttpsUrl") + "/connect/token",
                    ClientId = "clientanduser",
                    ClientSecret = "secret",
                    UserName = "m1",
                    Password = "password",
                    Scope = "api1"
                });

                if (tokenResponse.IsError)
                {
                    throw new Exception(tokenResponse.Error);
                }
                admin.token = tokenResponse.AccessToken;

                //记录登录时间
                admin.loginDate = DateTime.Now;
                _peAdminService.Update(admin);

                //清空密码返回
                admin.adminPwd = null;
                admin.menudata = _menuService.GetMenuListByRole(admin.id);
                result.returnData = admin;
                return Ok(result);

            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "登录失败，请稍后重试";
                return Ok(result);
            }

        }

        /// <summary>
        /// 根据前端传的用户获取动态路由
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("GetMenuListByRole")]
        public async Task<IActionResult> GetMenuListById([FromBody]encryData pData)
        {
            try
            {
                List<Menu> menu = await Task.Factory.StartNew(() => _menuService.GetMenuListByRole(pData.data.id));
                result.returnData = menu;
                return Ok(result);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
                return Ok(result);
            }
        }

        #region 管理员列表
        /// <summary>
        /// 获取管理员列表
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("GetAdminList")]
        public async Task<IActionResult> GetAdminList([FromBody]encryData pData)
        {
            try
            {
                var adminList = await Task.Factory.StartNew(() => _peAdminService.GetAdminList(pData.data.kw, pData.data.state));
                result.returnData = adminList;
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
            }
            return Ok(result);
        }

        /// <summary>
        /// 添加管理员信息
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin([FromBody]encryData pData)
        {
            try
            {
                string errMsg = string.Empty;
                bool isInsert = await Task.Factory.StartNew(() => _peAdminService.AddAdmin(pData.PeAdminData, ref errMsg));
                if (!isInsert)
                {
                    log.Error(errMsg);
                    result.success = false;
                    result.returnMsg = errMsg;
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
            }
            return Ok(result);
        }

        /// <summary>
        /// 修改管理员信息
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("UpdateAdmin")]
        public async Task<IActionResult> UpdateAdmin([FromBody]encryData pData)
        {
            try
            {
                bool isUpdate = await Task.Factory.StartNew(() => _peAdminService.UpdateAdmin(pData.PeAdminData));
                if (!isUpdate)
                {
                    result.success = false;
                    result.returnMsg = "修改信息失败";
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
            }
            return Ok(result);
        }
        /// <summary>
        /// 根据id集合批量删除管理员信息
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("DeleteAdminByIds")]
        public async Task<IActionResult> DeleteAdminByIds([FromBody]encryData pData)
        {
            try
            {
                bool isDelete = await Task.Factory.StartNew(() => _peAdminService.DeleteByIds(pData.data.ids));
                if (!isDelete)
                {
                    result.success = false;
                    result.returnMsg = "删除信息失败";
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
            }
            return Ok(result);
        }
        #endregion

        #region 角色列表
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("GetRoleList")]
        public async Task<IActionResult> GetRoleList([FromBody]encryData pData)
        {
            try
            {
                var roleList = await Task.Factory.StartNew(() => _peAdminService.GetRoleList(pData.data.kw, pData.PeRoleData.state));
                result.returnData = roleList;
                return Ok(result);
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
                return Ok(result);
            }
        }

        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody]encryData pData)
        {
            try
            {
                bool isAdd = await Task.Factory.StartNew(() => _peAdminService.AddRole(pData.PeRoleData));
                if (!isAdd)
                {
                    result.success = false;
                    result.returnMsg = "添加角色失败";
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
            }
            return Ok(result);
        }

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody]encryData pData)
        {
            try
            {
                bool isUpdate = await Task.Factory.StartNew(() => _peAdminService.UpdateRole(pData.PeRoleData));
                if (!isUpdate)
                {
                    result.success = false;
                    result.returnMsg = "修改角色失败";
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
            }
            return Ok(result);
        }

        /// <summary>
        /// 根据id集合批量删除角色信息
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("DeleteRoleByIds")]
        public async Task<IActionResult> DeleteRoleByIds([FromBody]encryData pData)
        {
            try
            {
                bool isDelete = await Task.Factory.StartNew(() => _peAdminService.DeleteByIds(pData.data.ids));
                if (!isDelete)
                {
                    result.success = false;
                    result.returnMsg = "删除角色失败";
                }
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
            }
            return Ok(result);
        }

        /// <summary>
        /// 根据角色id获取菜单id列表
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("GetMenuIdsByRoleId")]
        public async Task<IActionResult> GetMenuIdsByRoleId([FromBody]encryData pData)
        {
            try
            {
                result.returnData = await Task.Factory.StartNew(() => _peAdminService.GetMenuListById(pData.PeRoleData.id));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
            }
            return Ok(result);
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="pData"></param>
        /// <returns></returns>
        [HttpPost("SetPower")]
        public async Task<IActionResult> SetPower([FromBody]encryData pData)
        {
            try
            {
                await Task.Factory.StartNew(() => _peAdminService.SetPower(pData.PeRoleData.id, pData.data.ids));
            }
            catch (Exception e)
            {
                log.Error(e.Message);
                result.success = false;
                result.returnMsg = "系统繁忙！请稍后再试";
            }
            return Ok(result);
        }
        #endregion

    }
}