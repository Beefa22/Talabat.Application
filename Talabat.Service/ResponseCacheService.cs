
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task cacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if (response is null) return;// To block the code if there is no response\

           var options =new  JsonSerializerOptions(){ PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var serializedResponse = JsonSerializer.Serialize(response,options);

            await _database.StringSetAsync(cacheKey, serializedResponse, timeToLive);
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            var cachedResponse = await _database.StringGetAsync(cacheKey);
            if (cachedResponse.IsNullOrEmpty) return null;
            return cachedResponse;
        }
    }
}
