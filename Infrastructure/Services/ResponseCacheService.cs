using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Interface;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class ResponseCacheService:IResponseCacheService
    {
        private readonly StackExchange.Redis.IDatabase _dataBase;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _dataBase = redis.GetDatabase();
        }


        public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive)
        {
            if(response == null)
            {
                return;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedResponse = JsonSerializer.Serialize(response,options);

            await _dataBase.StringSetAsync(cacheKey,serializedResponse,timeToLive);
        }

        public async Task<string> GetCacheResponseAsync(string cacheKey)
        {
            var cachedResponse = await _dataBase.StringGetAsync(cacheKey);

            if(cachedResponse.IsNullOrEmpty)
            {
                return null;
            }

            return cachedResponse;
        }
    }
}