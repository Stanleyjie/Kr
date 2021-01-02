using DataModel;
using Repository.IRepository;
using Service.IService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Service
{
    public class CodeClusterService : BaseService<codeCluster>, ICodeClusterService
    {
        private readonly IcodeClusterRepository _codeClusterRepository;
        private readonly IcodeClusterEntryRepository _codeClusterEntryRepository;
        public CodeClusterService(IcodeClusterRepository codeClusterRepository, IcodeClusterEntryRepository codeClusterEntryRepository)
        {
            _codeClusterRepository = codeClusterRepository;
            _codeClusterEntryRepository = codeClusterEntryRepository;
            _baseRepository = _codeClusterRepository;
        }

        #region 套餐表操作
        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<codeCluster>> GetCodeClusterList()
        {
            var clusters = _codeClusterRepository.FindAll();
            return Task.FromResult(clusters);
        }

        /// <summary>
        /// 套餐添加
        /// </summary>
        /// <param name="codeCluster"></param>
        /// <returns></returns>
        public Task<codeCluster> CodeClusterAdd(codeCluster codeCluster)
        {
            _codeClusterRepository.Insert(codeCluster);
            return Task.FromResult(codeCluster);
        }

        /// <summary>
        /// 套餐修改
        /// </summary>
        /// <param name="codeCluster"></param>
        /// <returns></returns>
        public Task<bool> CodeClusterUpdate(codeCluster codeCluster)
        {
            var result = _codeClusterRepository.Update(codeCluster);
            return Task.FromResult(result);
        }

        /// <summary>
        /// 套餐删除
        /// </summary>
        /// <param name="clusCode"></param>
        /// <returns></returns>
        public Task<bool> CodeClusterDel(string clusCode)
        {
            var result = Submit(() =>
            {
                _codeClusterRepository.DeleteById(clusCode);
            });
            return Task.FromResult(result);
        }
        #endregion

        #region 组合-套餐表操作
        /// <summary>
        /// 获取组合-套餐关系列表
        /// </summary>
        /// <returns></returns>
        public object GetCodeClusterEntryList()
        {
            return _codeClusterEntryRepository.FindByClause(x => 1 == 1);
        }
        /// <summary>
        /// 获取组合-套餐关系详情
        /// </summary>
        /// <param name="clusCode"></param>
        /// <returns></returns>
        public object GetCodeClusterEntryDetail(string clusCode)
        {
            return _codeClusterEntryRepository.FindByClause(x => x.clusCode == clusCode);
        }
        /// <summary>
        /// 添加组合-套餐关系
        /// </summary>
        /// <param name="cce"></param>
        /// <returns></returns>
        public bool CodeClusterEntryAdd(codeClusterEntry cce)
        {
            try
            {
                _codeClusterEntryRepository.Insert(cce);
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 删除组合-套餐关系
        /// </summary>
        /// <param name="clusCode"></param>
        /// <returns></returns>
        public bool CodeClusterEntryDel(string clusCode)
        {
            try
            {
                _codeClusterEntryRepository.DeleteById(clusCode);
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
