using e_commerce.app.interfaces;
using e_commerce.app.Interfaces;
using e_commerce.infra.Data;
using e_commerce.infra.reposatory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Database
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("cs")));

        // 2. Redis
        services.AddStackExchangeRedisCache(op =>
        {
            op.Configuration = configuration.GetConnectionString("Redis");
        });
        services.AddSignalR();
        services.AddHttpContextAccessor();

        // 3. Repositories
        services.AddScoped<IFeedBackRepo, FeedbackRepository>();
        services.AddScoped<IReviewProductRepo, ReviewProductRepo>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepo, CategoryRepo>();
        services.AddScoped<IShoppingCartRepo, ShoppingCartRepo>();
        services.AddScoped<IShipmentRepo, ShipmentRepo>();
        services.AddScoped<IOrderRepo, OrderRepo>();
        services.AddScoped<IShippingZoneRepo, ShippingZoneRepo>();
        services.AddScoped<IDiscountRepo, DiscountRepo>();
        services.AddScoped<INotifiRepo, NotitfiRepo>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IWishlistRepository, WishlistRepository>();
        services.AddScoped<IUserAddressRepository, UserAddressRepository>();
        services.AddScoped<IWithdrawalRepository, WithdrawalRepository>();
        services.AddScoped<ISellerWalletRepository, SellerWalletRepository>();
        services.AddScoped<ISellerRepository, SellerRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        return services;
    }
}