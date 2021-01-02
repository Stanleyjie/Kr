using DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.IService
{
    public interface IMenuService : IBaseService<Menu>
    {
        List<Menu> GetMenuListByRole(int id);
    }
}
