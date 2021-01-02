using DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ICodeItemService : IBaseService<CodeItem>
    {
        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<CodeItem>> GetCodeItemList();

        /// <summary>
        /// 项目添加
        /// </summary>
        /// <param name="codeItem"></param>
        /// <returns></returns>
        Task<CodeItem> CodeItemAdd(CodeItem codeItem);

        /// <summary>
        /// 项目修改
        /// </summary>
        /// <returns></returns>
        Task<bool> CodeItemUpdate(CodeItem codeItem);
        /// <summary>
        /// 项目删除
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        Task<bool> CodeItemDel(string itemCode);
    }
}
