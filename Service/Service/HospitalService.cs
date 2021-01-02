using DataModel;
using DataModel.Other;
using Repository.IRepository;
using Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Service
{
    public class HospitalService : BaseService<Hospital>, IHospitalService
    {
        private readonly IHospitalRepository _hospitalRepository;
        private readonly IHospitalTagRepository _hospitalTagRepository;

        public HospitalService(IHospitalRepository hospitalRepository, IHospitalTagRepository hospitalTagRepository)
        {
            _hospitalRepository = hospitalRepository ?? throw new System.ArgumentNullException(nameof(hospitalRepository));
            _hospitalTagRepository = hospitalTagRepository ?? throw new System.ArgumentNullException(nameof(hospitalTagRepository));
            _baseRepository = _hospitalRepository;
        }

        /// <summary>
        /// 创建医院
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public Task<Hospital> CreateHospitalAsync(Hospital hospital)
        {
            var result = Submit(() =>
             {
                 hospital.Tags ??= Enumerable.Empty<Tag>();
                 hospital.imageUrls ??= Enumerable.Empty<string>().ToList();
                 foreach (var tag in hospital.Tags)
                 {
                     _hospitalTagRepository.Insert(new HospitalTag
                     {
                         hospitalCode = hospital.code,
                         tagId = tag.id
                     });
                 }
                 hospital = _hospitalRepository.Insert(hospital);
             });
            return Task.FromResult(result ? hospital : null);
        }

        /// <summary>
        /// 根据医院代码删除医院信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public Task<bool> DeleteHospitalByCodeAsync(string code)
        {
            var result = Submit(() =>
            {
                _hospitalRepository.Delete(h => h.code == code);
                _hospitalTagRepository.Delete(ht => ht.hospitalCode == code);
            });
            return Task.FromResult(result);
        }

        /// <summary>
        /// 根据医院代码获取医院信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<Hospital> GetHospitalByCodeAsync(string code)
        {
            var hospital = await _hospitalRepository.GetHospitalByCodeAsync(code);
            return hospital;
        }

        /// <summary>
        /// 获取医院列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public async Task<(IEnumerable<Hospital>, int)> GetHospitalListAsync(int page, int rows, string codeOrName, HospitalLevel? level)
        {
            var result = await _hospitalRepository.GetHospitalListAsync(page, rows, codeOrName, level);
            return result;
        }

        /// <summary>
        /// 根据医院代码更新医院信息
        /// </summary>
        /// <param name="hospital"></param>
        /// <returns></returns>
        public Task<bool> UpdateHospitalByCodeAsync(Hospital hospital)
        {
            var oldHospital = _hospitalRepository.FindByClause(h => h.code == hospital.code);
            if (oldHospital is null)
            {
                return Task.FromResult(false);
            }
            var result = Submit(() =>
             {
                 hospital.code = oldHospital.code;
                 hospital.id = oldHospital.id;
                 _hospitalRepository.Update(hospital);
                 _hospitalTagRepository.Delete(ht => ht.hospitalCode == hospital.code);
                 foreach (var tag in hospital.Tags)
                 {
                     _hospitalTagRepository.Insert(new HospitalTag
                     {
                         hospitalCode = hospital.code,
                         tagId = tag.id
                     });
                 }
             });

            return Task.FromResult(result);
        }
    }
}