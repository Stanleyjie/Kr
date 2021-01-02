using DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.IService
{
    public interface IPeAdminService : IBaseService<PeAdmin>
    {
        bool Vaildate(string userName, string password);

        #region 添加数据
        /// <summary>
        /// 新增管理员信息
        /// </summary>
        /// <param name="admin">管理员实体</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        bool AddAdmin(PeAdmin admin, ref string errMsg);
        #endregion

        #region 修改数据
        /// <summary>
        /// 修改管理员信息
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        bool UpdateAdmin(PeAdmin admin);
        #endregion

        #region 查询数据
        /// <summary>
        /// 获取所有或根据Admin_Code、Admin_Name和状态返回管理员列表
        /// </summary>
        /// <param name="kw">关键字</param>
        /// <param name="status">管理员是否禁用</param>
        /// <returns></returns>
        object GetAdminList(string kw, int? status);
        #endregion


        #region Role添加数据
        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool AddRole(PeRole role);
        #endregion

        #region Role修改数据
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        bool UpdateRole(PeRole role);
        #endregion

        #region Role查询数据
        /// <summary>
        /// 获取所有或根据角色名、描述和状态返回角色列表
        /// </summary>
        /// <param name="kw">关键字</param>
        /// <param name="status">角色是否禁用</param>
        /// <returns></returns>
        object GetRoleList(string kw, int? status);
        #endregion


        List<int> GetMenuListById(int id);

        bool SetPower(int roleId, object[] menuIds);
    }
}
