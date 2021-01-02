using DataModel;
using DataModel.Other;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    /// <summary>
    /// IHospitalRepository
    /// </summary>
    public interface IHospitalRepository : IBaseRepository<Hospital>
    {
        /// <summary>
        /// 根据医院代码获取医院信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<Hospital> GetHospitalByCodeAsync(string code);

        /// <summary>
        /// 获取医院列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        Task<(IEnumerable<Hospital>, int)> GetHospitalListAsync(int page, int rows, string codeOrName, HospitalLevel? level);
    }
}