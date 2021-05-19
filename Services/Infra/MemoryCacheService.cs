using System;
using Infra.Models;
using Infra.Parameters;
using Microsoft.Extensions.Caching.Memory;

namespace Services.Infra
{
    public class MemoryCacheService
    {
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        private readonly IMemoryCache _memoryCache;

        public void Set<T>(string cacheKey, T data)
        {
            ValidateCacheKey(cacheKey);

            _memoryCache.Set(cacheKey, data);
        }

        public void Set<T>(Guid? cacheKey, T data)
        {
            ValidateCacheKey(cacheKey?.ToString());

            _memoryCache.Set(cacheKey.Value.ToString(), data);
        }

        public void Set<T>(string cacheKey, T data, TimeSpan expire)
        {
            ValidateCacheKey(cacheKey);

            _memoryCache.Set(cacheKey, data, expire);
        }

        public void Set<T>(Guid? cacheKey, T data, TimeSpan expire)
        {
            ValidateCacheKey(cacheKey?.ToString());

            _memoryCache.Set(cacheKey.Value.ToString(), data, expire);
        }

        public T Get<T>(string cacheKey)
        {
            return _memoryCache.Get<T>(cacheKey);
        }

        public T Get<T>(Guid? cacheKey)
        {
            if (cacheKey == null)
            {
                return default;
            }

            return _memoryCache.Get<T>(cacheKey.ToString());
        }

        public void Remove(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public void Remove(Guid? cacheKey)
        {
            if (cacheKey == null)
            {
                return;
            }

            _memoryCache.Remove(cacheKey.ToString());
        }

        private static void ValidateCacheKey(string cacheKey)
        {
            if (string.IsNullOrWhiteSpace(cacheKey))
            {
                throw new ErrorCodeException(ErrorCode.E400009);
            }
        }
    }
}
