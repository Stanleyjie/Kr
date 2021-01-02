using DataModel;
using Repository.IRepository;
using Service.IService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.Service
{
    public class TagService : BaseService<Tag>, ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IHospitalTagRepository _hospitalTagRepository;

        public TagService(ITagRepository tagRepository, IHospitalTagRepository hospitalTagRepository)
        {
            _tagRepository = tagRepository ?? throw new System.ArgumentNullException(nameof(tagRepository));
            _hospitalTagRepository = hospitalTagRepository ?? throw new System.ArgumentNullException(nameof(hospitalTagRepository));
            _baseRepository = _tagRepository;
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Task<Tag> CreateTagAsync(Tag tag)
        {
            _tagRepository.Insert(tag);
            return Task.FromResult(tag);
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteTagAsync(int id)
        {
            var result = Submit(() =>
             {
                 _tagRepository.DeleteById(id);
                 _hospitalTagRepository.Delete(ht => ht.tagId == id);
             });
            return Task.FromResult(result);
        }

        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<Tag>> GetTagListAsync()
        {
            var tags = _tagRepository.FindAll();
            return Task.FromResult(tags);
        }

        /// <summary>
        /// 更新标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public Task<bool> UpdateTagAsync(Tag tag)
        {
            var result = _tagRepository.Update(tag);
            return Task.FromResult(result);
        }
    }
}