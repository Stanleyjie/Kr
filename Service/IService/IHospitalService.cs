using DataModel;
using DataModel.Other;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface IHospitalService : IBaseService<Hospital>
    {
        /// <summary>
        /// 获取医院列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        Task<(IEnumerable<Hospital>, int)> GetHospitalListAsync(int page, int rows, string codeOrName, HospitalLevel? level);

        /// <summary>
        /// 创建医院
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns></returns>
        Task<Hospital> CreateHospitalAsync(Hospital hospital);

        /// <summary>
        /// 根据医院代码更新医院信息
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns></returns>
        Task<bool> UpdateHospitalByCodeAsync(Hospital hospital);

        /// <summary>
        /// 根据医院代码删除医院信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteHospitalByCodeAsync(string code);

        /// <summary>
        /// 根据医院Code获取医院信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<Hospital> GetHospitalByCodeAsync(string code);
    }
}