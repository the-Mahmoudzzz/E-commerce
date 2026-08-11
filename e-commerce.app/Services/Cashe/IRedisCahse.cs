using e_commerce.app.Dto.CtegoriesDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Cashe
{
    public interface IRedisCahse
    {
        public Task <T>? GetTData<T>(string key);
        public Task SetData<T>(string key, T value);
        
    }
}
