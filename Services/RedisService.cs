using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class RedisService : IRedisService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisService> _logger;

        public RedisService(IDistributedCache cache, ILogger<RedisService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                var json = await _cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(json))
                    return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache GET failed for key {Key}", key);
            }
            return null;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
        {
            try
            {
                var json = JsonSerializer.Serialize(value);
                var options = new DistributedCacheEntryOptions();
                if (absoluteExpiration.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = absoluteExpiration.Value;
                }

                await _cache.SetStringAsync(key, json, options);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache SET failed for key {Key}", key);
            }
        }

        public async Task<string?> GetStringAsync(string key)
        {
            try
            {
                return await _cache.GetStringAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache GET string failed for key {Key}", key);
                return null;
            }
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? absoluteExpiration = null)
        {
            try
            {
                var options = new DistributedCacheEntryOptions();
                if (absoluteExpiration.HasValue)
                {
                    options.AbsoluteExpirationRelativeToNow = absoluteExpiration.Value;
                }

                await _cache.SetStringAsync(key, value, options);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Cache SET string failed for key {Key}", key);
            }
        }
    }
}
