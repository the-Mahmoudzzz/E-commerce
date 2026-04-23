using e_commerce.app.Dto.ProductDto;
using e_commerce.app.interfaces;
using e_commerce.core.entities;
using e_commerce.infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.infra.reposatory
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;
        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.products.Include(p => p.Category)
                .Include(p => p.Seller)
                .ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            await _context.products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _context.products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _context.products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _context.products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetBySellerAsync(int sellerId)
        {
            return await _context.products
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .Where(p => p.SellerId == sellerId)
                .ToListAsync();
        }
        public async Task<IReadOnlyList<Product>> GetProductsByIdsAsync(IEnumerable<int> productIds)
        {
            return await _context.products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();
        }
        public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(ProductSearchDto searchParams)
        {
            // بنبدأ الـ Query وبنجيب المنتجات المتوافق عليها والمفعلة بس
            var query = _context.products
                .Include(p => p.Category) // عشان نجيب اسم القسم
                .Where(p => p.IsApproved && p.IsActive)
                .AsQueryable();

            // 1. فلتر البحث بالاسم أو الوصف
            if (!string.IsNullOrWhiteSpace(searchParams.Q))
            {
                var searchWord = searchParams.Q.ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchWord) ||
                                         p.Description.ToLower().Contains(searchWord));
            }

            // 2. فلتر القسم
            if (searchParams.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == searchParams.CategoryId.Value);
            }

            // 3. فلتر السعر (الأقل والأكثر)
            if (searchParams.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= searchParams.MinPrice.Value);
            }
            if (searchParams.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= searchParams.MaxPrice.Value);
            }

            // بنحسب العدد الكلي للمنتجات اللي طلعت من الفلتر (عشان الـ Pagination في الفرونت اند)
            var totalCount = await query.CountAsync();

            // 4. تطبيق الـ Pagination (تخطي الصفحات اللي فاتت وجلب عدد عناصر الصفحة الحالية)
            var skip = (searchParams.Page - 1) * searchParams.PageSize;
            var products = await query
                .Skip(skip)
                .Take(searchParams.PageSize)
                .ToListAsync();

            return (products, totalCount);
        }

    }
}