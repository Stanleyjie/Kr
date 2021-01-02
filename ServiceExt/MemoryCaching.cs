using Microsoft.Extensions.Caching.Memory;
using System;

namespace ServiceExt
{
    /// <summary>
    /// 实例化缓存 
    /// </summary>
    public class MemoryCaching 
    {
        //引用Microsoft.Extensions.Caching.Memory;这个和.net 还是不一样，没有了Httpruntime了
        private readonly IMemoryCache _cache;
        //还是通过构造函数的方法，获取
        public MemoryCaching(IMemoryCache cache)
        {
            _cache = cache;
        }

        public object Get(string cacheKey)
        {
            return _cache.Get(cacheKey);
        }

        public void Set(string cacheKey, object cacheValue,int times=7200)
        {
            _cache.Set(cacheKey, cacheValue, TimeSpan.FromSeconds(times));
        }
    }

}
