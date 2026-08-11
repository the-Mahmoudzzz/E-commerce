using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using e_commerce.app.Dto.CtegoriesDto;

namespace e_commerce.app.Services.Cashe
{
    public class RedisCahse : IRedisCahse
    {
        private readonly IDistributedCache _cache;

        public RedisCahse(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetTData<T>(string key)
        {
            var member =await _cache.GetStringAsync(key);
            if(member is not  null)
            {
          
                return  System.Text.Json.JsonSerializer.Deserialize<T>(member);

            }
            return default(T?);
        }

        public async Task SetData<T>(string key, T value)
        {
            var op = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(6)
            };

             await _cache.SetStringAsync(key, System.Text.Json.JsonSerializer.Serialize(value),op);
        }

        
    }
}
