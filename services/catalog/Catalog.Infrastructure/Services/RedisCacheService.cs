using Catalog.Application.Interfaces.Services;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace Catalog.Infrastructure.Services;

public class RedisCacheService(IConnectionMultiplexer redis) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        var db = redis.GetDatabase();
        var json = db.JSON();
        var value = await json.GetAsync<T>(key);

        return value ?? default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        if (value is null)
        {
            return;
        }

        var db = redis.GetDatabase();
        var json = db.JSON();

        await json.SetAsync(key, path: "$", value);

        if (expiration.HasValue)
        {
            await db.KeyExpireAsync(key, expiration);
        }
    }

    public async Task RemoveAsync(string key)
    {
        var db = redis.GetDatabase();
        await db.KeyDeleteAsync(key);
    }
}