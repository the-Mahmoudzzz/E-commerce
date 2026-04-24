using e_commerce.app.Dto;
using e_commerce.app.Dto.SellerDTO;
using e_commerce.app.Interfaces;
using e_commerce.app.Services.IServices;
using e_commerce.core.Exceptions;          // ← ضيف ده
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

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

        // ✅ Helper — نفس pattern الـ OrderService
        private int GetCurrentSellerId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?
                .FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                throw new AuthenticationException("Seller is not authenticated.");

            return int.Parse(userIdClaim);
        }

        public async Task<SellerDashboardDto> GetDashboardAsync()
        {
            var sellerId = GetCurrentSellerId();

            var orders = await _sellerRepo.GetSellerOrdersAsync(sellerId);
            var items = await _sellerRepo.GetSellerOrderDetailsAsync(sellerId);

            // ✅ مش error لو مفيش أوردرات — بس نرجع zeros
            var totalRevenue = orders.Any() ? orders.Sum(o => o.FinalAmount) : 0;
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
                TotalEarnings = transactions.Any() ? transactions.Sum(x => x.Amount) : 0,
                Transactions = transactions
            };
        }
    }
}