using DataModel;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CodeItemCombService: BaseService<codeItemComb>, ICodeItemCombService
    {
        public readonly IcodeItemCombRepository _codeItemCombRepository;
        private readonly IcodeItemCombEntryRepository _codeItemCombEntryRepository;
        private readonly ICodeItemCombEntryHospRepository _codeItemCombEntryHospRepository;
        public CodeItemCombService(IcodeItemCombRepository codeItemCombRepository, IcodeItemCombEntryRepository codeItemCombEntryRepository
            , ICodeItemCombEntryHospRepository codeItemCombEntryHospRepository)
        {
            _codeItemCombRepository = codeItemCombRepository;
            _codeItemCombEntryRepository = codeItemCombEntryRepository;
            _codeItemCombEntryHospRepository = codeItemCombEntryHospRepository;
            _baseRepository = _codeItemCombRepository;
        }

        #region 组合表操作
        
        /// <summary>
        /// 获取组合列表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<codeItemComb>> GetCodeItemCombList()
        {
            var combs = _codeItemCombRepository.FindAll();
            return Task.FromResult(combs);
        }

        /// <summary>
        ///组合添加
        /// </summary>
        /// <param name="codeItemComb"></param>
        /// <returns></returns>
        public Task<codeItemComb> CodeItemCombAdd(codeItemComb codeItemComb)
        {
            _codeItemCombRepository.Insert(codeItemComb);
            return Task.FromResult(codeItemComb);
        }

        /// <summary>
        /// 组合修改
        /// </summary>
        /// <param name="codeItemComb"></param>
        /// <returns></returns>
        public Task<bool> CodeItemCombUpdate(codeItemComb codeItemComb)
        {
            var result = _codeItemCombRepository.Update(codeItemComb);
            return Task.FromResult(result);
        }

        /// <summary>
        /// 组合删除
        /// </summary>
        /// <param name="combCode"></param>
        /// <returns></returns>
        public Task<bool> CodeItemCombDel(string combCode)
        {
            var result = Submit(() =>
            {
                _codeItemCombRepository.DeleteById(combCode);
                _codeItemCombEntryHospRepository.DeleteById(combCode);
            });
            return Task.FromResult(result);
        }
        #endregion

        #region 项目-组合关系表操作
        /// <summary>
        /// 获取项目-组合关系列表
        /// </summary>
        /// <returns></returns>
        public object GetCodeItemCombEntryList()
        {
            return _codeItemCombEntryRepository.FindByClause(x => 1 == 1);
        }
        /// <summary>
        /// 获取项目-组合关系详情
        /// </summary>
        /// <param name="combCode"></param>
        /// <returns></returns>
        public object GetCodeItemCombEntryDetail(string combCode)
        {
            return _codeItemCombEntryRepository.FindByClause(x => x.combCode == combCode);
        }
        /// <summary>
        /// 添加项目-组合关系
        /// </summary>
        /// <param name="cice"></param>
        /// <returns></returns>
        public bool CodeItemCombEntryAdd(codeItemCombEntry cice)
        {
            try
            {
                _codeItemCombEntryRepository.Insert(cice);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除项目-组合关系
        /// </summary>
        /// <param name="combCode"></param>
        /// <returns></returns>
        public bool CodeItemCombEntryDel(string combCode)
        {
            try
            {
                _codeItemCombEntryRepository.DeleteById(combCode);
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region 组合-医院码关系表操作
        /// <summary>
        /// 获取项目-组合关系列表
        /// </summary>
        /// <returns></returns>
        public object GetCodeItemCombEntryHospList()
        {
            return _codeItemCombEntryHospRepository.FindByClause(x => 1 == 1);
        }
        /// <summary>
        /// 获取组合-医院码关系详情
        /// </summary>
        /// <param name="combCode"></param>
        /// <returns></returns>
        public object GetCodeItemCombEntryHospDetail(string hospCombCode)
        {
            return _codeItemCombEntryHospRepository.FindByClause(x => x.hospCombCode == hospCombCode);
        }
        /// <summary>
        /// 添加组合-医院码关系
        /// </summary>
        /// <param name="cceh"></param>
        /// <returns></returns>
        public bool CodeItemCombEntryHospAdd(CodeItemCombEntryHosp cceh)
        {
            try
            {
                _codeItemCombEntryHospRepository.Insert(cceh);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除组合-医院码关系
        /// </summary>
        /// <param name="hospCombCode"></param>
        /// <returns></returns>
        public bool CodeItemCombEntryHospDel(string hospCombCode)
        {
            try
            {
                _codeItemCombEntryHospRepository.DeleteById(hospCombCode);
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}
