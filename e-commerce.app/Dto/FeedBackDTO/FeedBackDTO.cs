using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.FeedBackDTO
{
    public class FeedBackDTO
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string CustomerName { get; set; }
        public FeedbackType Type { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
