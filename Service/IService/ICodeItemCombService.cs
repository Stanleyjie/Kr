using DataModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ICodeItemCombService : IBaseService<codeItemComb>
    {
        #region 组合表操作
        /// <summary>
        /// 获取组合列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<codeItemComb>> GetCodeItemCombList();

        /// <summary>
        /// 组合添加
        /// </summary>
        /// <param name="codeItemComb"></param>
        /// <returns></returns>
        Task<codeItemComb> CodeItemCombAdd(codeItemComb codeItemComb);

        /// <summary>
        /// 组合修改
        /// </summary>
        /// <param name="codeItemComb"></param>
        /// <returns></returns>
        Task<bool> CodeItemCombUpdate(codeItemComb codeItemComb);
        /// <summary>
        /// 组合删除
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        Task<bool> CodeItemCombDel(string itemCode);
        #endregion

        #region 项目-组合关系表操作
        /// <summary>
        /// 获取项目-组合关系列表
        /// </summary>
        /// <returns></returns>
        object GetCodeItemCombEntryList();
        /// <summary>
        /// 获取项目-组合关系详情
        /// </summary>
        /// <param name="combCode"></param>
        /// <returns></returns>
        object GetCodeItemCombEntryDetail(string combCode);
        /// <summary>
        /// 添加项目-组合关系
        /// </summary>
        /// <param name="cice"></param>
        /// <returns></returns>
        bool CodeItemCombEntryAdd(codeItemCombEntry cice);
        /// <summary>
        /// 删除项目-组合关系
        /// </summary>
        /// <param name="combCode"></param>
        /// <returns></returns>
        bool CodeItemCombEntryDel(string combCode);
        #endregion

        #region 组合-医院码关系表操作
        /// <summary>
        /// 获取组合-医院关系列表
        /// </summary>
        /// <returns></returns>
        object GetCodeItemCombEntryHospList();
        /// <summary>
        /// 获取组合-医院关系详情
        /// </summary>
        /// <param name="hospCombCode"></param>
        /// <returns></returns>
        object GetCodeItemCombEntryHospDetail(string hospCombCode);
        /// <summary>
        /// 添加组合-医院关系
        /// </summary>
        /// <param name="cceh"></param>
        /// <returns></returns>
        bool CodeItemCombEntryHospAdd(CodeItemCombEntryHosp cceh);
        /// <summary>
        /// 删除组合-医院关系
        /// </summary>
        /// <param name="hospCombCode"></param>
        /// <returns></returns>
        bool CodeItemCombEntryHospDel(string hospCombCode);
        #endregion
    }
}
