using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.FeedBackDTO
{
    public class CreateFeedbackDto
    {

        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message cannot exceed 500 characters")]
        public string Message { get; set; } = null!;  

        [Required(ErrorMessage = "UserId is required")]
        [Range(1, int.MaxValue, ErrorMessage = "UserId must be a positive number")]
        [DefaultValue(0)]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Feedback Type is required")]
        [EnumDataType(typeof(FeedbackType), ErrorMessage = "Invalid Feedback Type")]
        public FeedbackType Type { get; set; }
    }
}
