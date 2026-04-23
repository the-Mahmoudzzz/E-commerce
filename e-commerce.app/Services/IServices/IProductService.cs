using e_commerce.app.Dto.ProductDto;
using e_commerce.core.entities;
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
            Task UpdateProductAsync(int id, UpdateProductBySellerDto dto, int currentSellerId);
            Task ApproveProductAsync(int id, ApproveProductByAdminDto dto);
            Task DeleteProductAsync(int id, int currentSellerId);
        Task<(IEnumerable<summaryProductDto> Products, int TotalCount)> SearchAsync(ProductSearchDto searchParams);

    }
}
