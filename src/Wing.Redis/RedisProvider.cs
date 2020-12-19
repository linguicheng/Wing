using System;
using System.Threading.Tasks;
using CSRedis;
using Wing.Cache;

namespace Wing.Redis
{
    public class RedisProvider : ICache
    {
        public RedisProvider(Config config)
        {
            CSRedisClient csredis;
            var connectionString = config.ConnectionString;
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                csredis = new CSRedisClient(connectionString);
            }
            else
            {
                csredis = new CSRedisClient(connectionString, config.Sentinels);
            }

            RedisHelper.Initialization(csredis);
        }

        public async Task<bool> Add(string key, object value)
        {
            return await RedisHelper.SetAsync(key, value);
        }

        public async Task<bool> Add(string key, object value, DateTime absoluteTime)
        {
            return await RedisHelper.SetAsync(key, value, absoluteTime - DateTime.Now);
        }

        public async Task<bool> Add(string key, object value, TimeSpan slidingTime)
        {
            return await RedisHelper.SetAsync(key, value, slidingTime);
        }

        public async Task<long> Decrease(string key, long step = 1)
        {
            return await RedisHelper.IncrByAsync(key, -step);
        }

        public async Task<bool> Exists(string key)
        {
            return await RedisHelper.ExistsAsync(key);
        }

        public async Task<T> Get<T>(string key)
        {
            return await RedisHelper.GetAsync<T>(key);
        }

        public async Task<bool> HAdd(string key, string field, object value)
        {
            return await RedisHelper.HSetAsync(key, field, value);
        }

        public async Task<bool> HExists(string key, string field)
        {
            return await RedisHelper.HExistsAsync(key, field);
        }

        public async Task<bool> HRemove(string key, string field)
        {
            return await RedisHelper.HDelAsync(key, field) > 0;
        }

        public async Task<long> Increase(string key, long step = 1)
        {
            return await RedisHelper.IncrByAsync(key, step);
        }

        public async Task<bool> Remove(string key)
        {
            return await RedisHelper.DelAsync(key) > 0;
        }

        public async Task<bool> SetExpire(string key, TimeSpan span)
        {
            return await RedisHelper.ExpireAsync(key, span);
        }

        public async Task<bool> SetExpire(string key, DateTime absoluteTime)
        {
            return await RedisHelper.ExpireAtAsync(key, absoluteTime);
        }
    }
}
