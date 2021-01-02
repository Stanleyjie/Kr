using DataModel;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CodeItemService : BaseService<CodeItem>, ICodeItemService
    {
        public readonly ICodeItemRepository _codeItemRepository;
        public readonly IcodeItemCombEntryRepository _codeItemCombEntryRepository;
        public CodeItemService(ICodeItemRepository codeItemRepository, IcodeItemCombEntryRepository codeItemCombEntryRepository)
        {
            _codeItemRepository = codeItemRepository;
            _codeItemCombEntryRepository = codeItemCombEntryRepository;
            _baseRepository = _codeItemRepository;
        }

        /// <summary>
        /// 获取项目列表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<CodeItem>> GetCodeItemList()
        {
            var items = _codeItemRepository.FindAll();
            return Task.FromResult(items);
        }

        /// <summary>
        /// 项目添加
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public Task<CodeItem> CodeItemAdd(CodeItem codeItem)
        {
            _codeItemRepository.Insert(codeItem);
            return Task.FromResult(codeItem);
        }

        /// <summary>
        /// 项目修改
        /// </summary>
        /// <param name="codeItem"></param>
        /// <returns></returns>
        public Task<bool> CodeItemUpdate(CodeItem codeItem)
        {
            var result = _codeItemRepository.Update(codeItem);
            return Task.FromResult(result);
        }

        /// <summary>
        /// 项目删除
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public Task<bool> CodeItemDel(string itemCode)
        {
            var result = Submit(() =>
            {
                _codeItemRepository.Delete(i => i.itemCode == itemCode);
                _codeItemCombEntryRepository.Delete(h => h.itemCode == itemCode);
            });
            return Task.FromResult(result);
        }
    }
}
