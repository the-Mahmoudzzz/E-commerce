using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto.CtegoriesDto
{
    public class CreateSubCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
        public int CreatedByAdminId { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
