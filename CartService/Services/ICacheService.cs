using System;
using System.Threading.Tasks;

namespace CartService.Services
{
    public interface ICacheService
    {
        Task<T> GetDataAsync<T>(string key);
        Task SetDataAsync<T>(string key, T value, TimeSpan cacheDuration);
        Task RemoveDataAsync(string key);
    }
}
