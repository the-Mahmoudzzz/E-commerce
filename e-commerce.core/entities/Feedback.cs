using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.core.entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int UserId { get; set; }
        public virtual User Customer { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public FeedbackType Type { get; set; }

    }
    public enum FeedbackType
    {
        Complaint, 
        Suggestion, 
        BugReport, 
        General 
    }
}
