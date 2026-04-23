using e_commerce.app.Dto;
using e_commerce.app.Dto.SellerDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.app.Services.Implementation
{
    public class SellerService : ISellerService
    {
        private readonly ISellerRepository _sellerRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SellerService(
            ISellerRepository sellerRepo,
            IHttpContextAccessor httpContextAccessor)
        {
            _sellerRepo = sellerRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetCurrentSellerId()
        {
            var user = _httpContextAccessor.HttpContext.User;

            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public async Task<SellerDashboardDto> GetDashboardAsync()
        {
            var sellerId = GetCurrentSellerId();

            var orders = await _sellerRepo.GetSellerOrdersAsync(sellerId);
            var items = await _sellerRepo.GetSellerOrderDetailsAsync(sellerId);

            var totalRevenue = orders.Sum(o => o.FinalAmount);
            var totalOrders = orders.Count;

            var topProducts = items
                .GroupBy(i => new { i.ProductId, i.Product.Name })
                .Select(g => new TopProductDto
                {
                    ProductId = g.Key.ProductId,
                    Name = g.Key.Name,
                    SoldQuantity = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.SoldQuantity)
                .Take(5)
                .ToList();

            return new SellerDashboardDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = totalOrders,
                TopProducts = topProducts
            };
        }

        public async Task<SellerEarningsDto> GetEarningsAsync()
        {
            var sellerId = GetCurrentSellerId();

            var orders = await _sellerRepo.GetSellerOrdersAsync(sellerId);

            var transactions = orders.Select(o => new EarningTransactionDto
            {
                OrderId = o.Id,
                Amount = o.FinalAmount,
                Date = o.CreatedAt
            }).ToList();

            return new SellerEarningsDto
            {
                TotalEarnings = transactions.Sum(x => x.Amount),
                Transactions = transactions
            };
        }

      
    }
}
