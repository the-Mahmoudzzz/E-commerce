using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.UserDTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public string Email { get; set; }= string.Empty;
        public bool IsApproved  { get; set; }

    }
}
