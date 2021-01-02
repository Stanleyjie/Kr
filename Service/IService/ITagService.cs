using DataModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service.IService
{
    public interface ITagService : IBaseService<Tag>
    {
        /// <summary>
        /// 获取标签列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Tag>> GetTagListAsync();

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task<Tag> CreateTagAsync(Tag tag);

        /// <summary>
        /// 更新标签信息
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task<bool> UpdateTagAsync(Tag tag);

        /// <summary>
        /// 删除标签
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteTagAsync(int id);
    }
}