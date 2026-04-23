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

        Task<IEnumerable<summaryProductDto>> GetLowStockAsync(int threshold);

        Task AddProductAsync(CreateProductBySellerDto dto, int sellerId);

        Task UpdateProductAsync(int id, UpdateProductBySellerDto dto);

        Task UpdateStockAsync(int id, int quantity);

        Task ApproveProductAsync(int id, ApproveProductByAdminDto dto);

        Task DeleteProductAsync(int id);
    }
}
