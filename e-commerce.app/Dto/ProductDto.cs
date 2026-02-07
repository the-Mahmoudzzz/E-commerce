using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Dto
{
    public class CreateProductBySellerDto
    {
        [Required(ErrorMessage = "The name is required")]
        public string Name { get; set; } = string.Empty;


        [Required(ErrorMessage = "The description is required")]
        [StringLength(250, MinimumLength = 40, ErrorMessage = "Description must be between 40 and 250 characters")]
        public string Description { get; set; }


        [Required(ErrorMessage = "The price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "The imageUrl is required")]

        [Url(ErrorMessage = "Image Url must be valid")]
        public string ImageUrl { get; set; } = string.Empty;



        [Required(ErrorMessage = "The quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity Can’t be negative")]
        public int Quantity { get; set; }


        [Required(ErrorMessage = "The categoryId is required ")]
        [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be greater than zero")]
        public int CategoryId { get; set; }

    }

    public class ApproveProductByAdminDto
    {
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
    }

    public class UpdateProductBySellerDto
    {
        public string? Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string ?ImageUrl { get; set; } = string.Empty;
        public int ?Quantity { get; set; }
        public int ?CategoryId { get; set; }

        public bool ?IsActive { get; set; } //admin only


    }
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int Quantity { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int SellerId { get; set; }
        public string SellerName { get; set; } = string.Empty;

    }
    public class summaryProductDto
    {

        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public bool InStock => Quantity >= 1;
    }
}
