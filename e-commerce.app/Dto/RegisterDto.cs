using System.ComponentModel.DataAnnotations;
using  e_commerce.core.Enum;

namespace e_commerce.app.Dto
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; }=string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }=string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }= string.Empty;
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }= string.Empty;
        [Required]
        [RegularExpression("^[0-9]*$")]
        
        public string PhoneNumber { get; set; } = string.Empty;
        public  UserRole UserRole { get; set; }=UserRole.User;
    }
}
