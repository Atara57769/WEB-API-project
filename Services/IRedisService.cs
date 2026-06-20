using System;
using System.Threading.Tasks;

namespace Services
{
    public interface IRedisService
    {
        Task<T?> GetAsync<T>(string key) where T : class;
        Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null);
        Task<string?> GetStringAsync(string key);
        Task SetStringAsync(string key, string value, TimeSpan? absoluteExpiration = null);
    }
}
