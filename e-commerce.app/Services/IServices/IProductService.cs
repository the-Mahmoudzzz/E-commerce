using e_commerce.app.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.servieses.iserviese
{
    public interface IProductService
    {
 
            Task<ProductDto> GetByIdAsync(int id);
            Task<IEnumerable<summaryProductDto>> GetAllAsync();
            Task<IEnumerable<summaryProductDto>> GetBySellerAsync(int sellerId);
            Task AddProductAsync(CreateProductBySellerDto dto, int sellerId);
            Task UpdateProductAsync(int id, UpdateProductBySellerDto dto);
            Task ApproveProductAsync(int id, ApproveProductByAdminDto dto);
            Task DeleteProductAsync(int id);
      
    }
}
