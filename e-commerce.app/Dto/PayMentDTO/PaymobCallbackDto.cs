using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.PayMentDTO
{
    public class PaymobCallbackDto
    {
        [JsonPropertyName("obj")]
        public PaymobObj Obj { get; set; }
    }
    public class PaymobObj
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("order")]
        public PaymobOrder Order { get; set; }
    }

    public class PaymobOrder
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
