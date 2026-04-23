using e_commerce.app.Dto.ProductDto;
using e_commerce.core.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);

        Task<IEnumerable<Product>> GetAllAsync();

        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);

        Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);

        Task<IEnumerable<Product>> GetBySellerAsync(int sellerId);

        Task<IReadOnlyList<Product>> GetProductsByIdsAsync(IEnumerable<int> productIds);
        Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(ProductSearchDto searchParams);


        Task<IEnumerable<Product>> GetLowStockAsync(int threshold);


    }
}
