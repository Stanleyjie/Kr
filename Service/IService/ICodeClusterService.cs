using DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ICodeClusterService: IBaseService<codeCluster>
    {
        #region 套餐表操作
        /// <summary>
        /// 获取套餐列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<codeCluster>> GetCodeClusterList();

        /// <summary>
        /// 套餐添加
        /// </summary>
        /// <param name="codeCluster"></param>
        /// <returns></returns>
        Task<codeCluster> CodeClusterAdd(codeCluster codeCluster);

        /// <summary>
        /// 套餐修改
        /// </summary>
        /// <param name="codeCluster"></param>
        /// <returns></returns>
        Task<bool> CodeClusterUpdate(codeCluster codeCluster);

        /// <summary>
        /// 套餐删除
        /// </summary>
        /// <param name="clusCode"></param>
        /// <returns></returns>
        Task<bool> CodeClusterDel(string clusCode);
        #endregion

        #region 组合-套餐表操作
        /// <summary>
        /// 获取组合-套餐关系列表
        /// </summary>
        /// <returns></returns>
        object GetCodeClusterEntryList();
        /// <summary>
        /// 获取组合-套餐关系详情
        /// </summary>
        /// <param name="clusCode"></param>
        /// <returns></returns>
        object GetCodeClusterEntryDetail(string clusCode);
        /// <summary>
        /// 添加组合-套餐关系
        /// </summary>
        /// <param name="cce"></param>
        /// <returns></returns>
        bool CodeClusterEntryAdd(codeClusterEntry cce);
        /// <summary>
        /// 删除组合-套餐关系
        /// </summary>
        /// <param name="clusCode"></param>
        /// <returns></returns>
        bool CodeClusterEntryDel(string clusCode);
        #endregion
    }
}
