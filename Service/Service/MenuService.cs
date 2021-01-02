using DataModel;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class MenuService : BaseService<Menu>, IMenuService
    {
        private readonly IPeAdminRepository _peAdminRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPeRoleRepository _peRoleRepository;
        private readonly IMenuRoleRepository _menuRoleRepository;
        public MenuService(IMenuRepository menuRepository, IPeAdminRepository peAdminRepository,
         IPeRoleRepository peRoleRepository, IMenuRoleRepository menuRoleRepository)
        {
            _menuRepository = menuRepository;
            _peAdminRepository = peAdminRepository;
            _peRoleRepository = peRoleRepository;
            _menuRoleRepository = menuRoleRepository;
        }


        #region 根据管理员Id获取菜单列表
        public List<Menu> GetMenuListByRole(int id)
        {
            PeAdmin admin = _peAdminRepository.FindById(id);
            List<Menu> menuList = new List<Menu>();
            if ((admin?.adminCode ?? "").Trim().ToUpper() == "SuperAdmin".ToUpper())
            {
                menuList = _menuRepository.FindAll().OrderBy(x => x.sortIndex).ToList();
            }
            else if (id == 0 || (admin?.adminCode ?? "").Trim().ToUpper() == "Admin".ToUpper())
            {
                menuList = _menuRepository.FindAll().Where(x => x.state == 1).OrderBy(x => x.sortIndex).ToList();
            }
            else
            {
                menuList = (from r in _peRoleRepository.FindListByClause(x => x.id == admin.adminType)
                            join s in _menuRoleRepository.FindAll() on r.id equals s.roleId
                            join m in _menuRepository.FindAll().Where(x => x.state == 1) on s.menuId equals m.id
                            select m).OrderBy(x => x.sortIndex).ToList();
            }
            return AddChildN(0, menuList);
        }

        private List<Menu> AddChildN(int Pid, List<Menu> menuList)
        {
            var data = menuList.Where(x => x.parentId == Pid);//这里是获取数据
            List<Menu> list = new List<Menu>();
            foreach (var item in data)
            {
                //这一块主要是转换成TreeChidViewModel的值.
                Menu childViewModel = new Menu();
                childViewModel.id = item.id;
                childViewModel.name = item.name;
                childViewModel.path = item.path;
                childViewModel.metaTitle = item.metaTitle;
                childViewModel.metaTicon = item.metaTicon;
                childViewModel.sortIndex = item.sortIndex;
                childViewModel.parentId = item.parentId;
                childViewModel.viewPowerID = item.viewPowerID;
                childViewModel.state = item.state;
                childViewModel.children = GetChildList(childViewModel, menuList);
                //childViewModel.meta = new { keepAlive = true };
                list.Add(childViewModel);
            }
            return list;
        }

        private List<Menu> GetChildList(Menu treeChildView, List<Menu> menuList)
        {
            if (_menuRepository.FindByClause(x => x.parentId == treeChildView.id) == null)
            {
                return null;
            }
            else
            {
                return AddChildN(treeChildView.id, menuList);
            }
        }
        #endregion
    }
}
