using System;
using System.Threading.Tasks;

namespace Wing.Cache
{
    public interface ICache
    {
        Task<bool> Add(string key, object value);

        Task<bool> Add(string key, object value, DateTime absoluteTime);

        Task<bool> Add(string key, object value, TimeSpan slidingTime);

        Task<T> Get<T>(string key);

        Task<bool> Remove(string key);

        Task<bool> Exists(string key);

        Task<bool> HAdd(string key, string field, object value);

        Task<bool> HExists(string key, string field);

        Task<bool> HRemove(string key, string field);

        Task<bool> SetExpire(string key, TimeSpan span);

        Task<bool> SetExpire(string key, DateTime absoluteTime);

        Task<long> Increase(string key, long step = 1);

        Task<long> Decrease(string key, long step = 1);
    }
}
