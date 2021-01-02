using DataModel;
using Repository.IRepository;
using Service.IService;
using ServiceExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class PeAdminService : BaseService<PeAdmin>, IPeAdminService
    {
        private readonly IPeAdminRepository _peAdminRepository;
        private readonly IMenuRoleRepository _menuRoleRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPeRoleRepository _peRoleRepository;
        public PeAdminService(IPeAdminRepository peAdminRepository, IPeRoleRepository peRoleRepository,
           IMenuRoleRepository menuRoleRepository,IMenuRepository menuRepository)
        {
            _peAdminRepository = peAdminRepository;
            _peRoleRepository = peRoleRepository;
            _menuRoleRepository = menuRoleRepository;
            _menuRepository = menuRepository;
            this._baseRepository = _peAdminRepository;
        }

        public bool Vaildate(string userName, string password)
        {
            try
            {
                //此处写固定 因为是web项目。如果需要修改则需要在数据库添加表
                if (userName == "krinfo" && password == "krExternal")
                {
                    return true;
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        #region 添加数据
        /// <summary>
        /// 新增管理员信息
        /// </summary>
        /// <param name="admin">管理员实体</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public bool AddAdmin(PeAdmin admin, ref string errMsg)
        {
            if (_peAdminRepository.FindByClause(x => (x.adminCode ?? "").ToUpper() == (admin.adminCode ?? "").ToUpper()) != null)
            {
                errMsg = $"账号（{admin.adminCode}）已存在！请重新输入";
                return false;
            }
            admin.adminPwd = ServiceExt.SecurityHelper.ToMD5(admin.adminPwd.Trim()).Substring(8, 16);
            admin.createDate = DateTime.Now;
            _peAdminRepository.Insert(admin);
            return true;
        }
        #endregion

        #region 删除数据

        #endregion

        #region 修改数据
        /// <summary>
        /// 修改管理员信息
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public bool UpdateAdmin(PeAdmin admin)
        {
            PeAdmin oldAdmin = _peAdminRepository.FindByClause(x => x.id == admin.id);
            admin.adminCode = oldAdmin.adminCode;
            admin.adminPwd = string.IsNullOrEmpty(admin.adminPwd) ? oldAdmin.adminPwd : SecurityHelper.ToMD5(admin.adminPwd.Trim()).Substring(8, 16);
            admin.createDate = oldAdmin.createDate;
            admin.createID = oldAdmin.createID;
            admin.updateDate = DateTime.Now;
            admin.loginDate = oldAdmin.loginDate;
            admin.openId = oldAdmin.openId;
            return _peAdminRepository.Update(admin);
        }
        #endregion

        #region 查询数据
        /// <summary>
        /// 获取所有或根据Admin_Code、Admin_Name和状态返回管理员列表
        /// </summary>
        /// <param name="kw">关键字</param>
        /// <param name="status">管理员是否禁用</param>
        /// <returns></returns>
        public object GetAdminList(string kw, int? status)
        {
            kw = (kw ?? "").Trim();
            var adminList = from a in _peAdminRepository.FindAll()
                            join r in _peRoleRepository.FindAll()
                            on a.adminType equals r.id
                            where (string.IsNullOrEmpty(kw) || a.adminCode.Contains(kw) || a.adminName.Contains(kw)) && (status == null || a.state == status)
                            select new
                            {
                                a.id,
                                a.adminCode,
                                a.adminName,
                                a.state,
                                r.roleName,
                                a.adminType
                            };
            return adminList;
        }
        #endregion


        #region role

        #region 添加数据
        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool AddRole(PeRole role)
        {
            role.createDate = DateTime.Now;
            _peRoleRepository.Insert(role);
            return true;
        }
        #endregion

        #region 删除数据

        #endregion

        #region 修改数据
        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool UpdateRole(PeRole role)
        {
            PeRole oldRole = _peRoleRepository.FindByClause(x => x.id == role.id);
            role.createDate = oldRole.createDate;
            role.createId = oldRole.createId;
            role.updateDate = DateTime.Now;
            return _peRoleRepository.Update(role);
        }
        #endregion

        #region 查询数据
        /// <summary>
        /// 获取所有或根据角色名、描述和状态返回角色列表
        /// </summary>
        /// <param name="kw">关键字</param>
        /// <param name="status">角色是否禁用</param>
        /// <returns></returns>
        public object GetRoleList(string kw, int? status)
        {
            kw = (kw ?? "").Trim();
            var roleList = _peRoleRepository.FindListByClause(x => (string.IsNullOrEmpty(kw) || x.roleName.Contains(kw) || x.description.Contains(kw))
              && (status == null || x.state == status)).Select(x => new
              {
                  x.id,
                  x.roleName,
                  x.description,
                  x.state
              });
            return roleList;
        }
        #endregion

        #endregion


        /// <summary>
        /// 获取角色的菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetMenuListById(int id)
        {
            return _menuRoleRepository.FindListByClause(x => x.roleId == id).Select(x => x.menuId).ToList();
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        public bool SetPower(int roleId, object[] menuIds)
        {
            #region 
            //删除角色所有菜单
            _baseRepository.ExecuteSqlExecuteSqlCommand("delete from MenuSet where RoleID=@RoleId", new { RoleId = roleId });
            //遍历添加菜单
            foreach (object menuId in menuIds)
            {
                MenuRole menuSet = new MenuRole();
                menuSet.menuId = int.Parse(menuId.ToString());
                menuSet.roleId = roleId;
                Menu menu = _menuRepository.FindByClause(x => x.id == menuSet.menuId); //查询Menu表ID为MenuId的数据
                Menu parentMenu = _menuRepository.FindByClause(x => x.id == menu.parentId);//查询Menu表中所对应的父级
                if (parentMenu != null)
                {
                    _menuRoleRepository.Insert(menuSet);
                    if (_menuRoleRepository.FindByClause(x => x.roleId == roleId && x.menuId == parentMenu.id) == null)
                    {
                        MenuRole parentMenuSet = new MenuRole();
                        parentMenuSet.menuId = parentMenu.id;
                        parentMenuSet.roleId = roleId;
                        _menuRoleRepository.Insert(parentMenuSet);
                    }
                }
                continue;
            }
            return true;
            #endregion

        }
    }
}
