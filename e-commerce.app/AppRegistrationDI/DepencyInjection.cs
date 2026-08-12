
using AutoMapper;
using e_commerce.app.Services.Cashe;
using e_commerce.app.Services.ExternalService;
using e_commerce.app.Services.Implementation;
using e_commerce.app.Services.IServices;
using e_commerce.app.servieses.impelmentaion;
using e_commerce.app.servieses.iserviese;
using MailKit;
using Microsoft.Extensions.DependencyInjection;
using Web.App.Services;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // 1. AutoMapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // 2. Core Services
        services.AddScoped<IFeedbackService, FeedBackService>();
        services.AddScoped<IReviewProductService, ProductReviewService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IShoppingServiece, ShoppingServiece>();
        services.AddScoped<IShipmentService, ShipmentService>();
        services.AddScoped<IShippingService, ShippingService>();
        services.AddScoped<IOrderService, OrderServiece>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IWishlistService, WishlistService>();
        services.AddScoped<IUserAddressService, UserAddressService>();
        services.AddScoped<IWithdrawalService, WithdrawalService>();
        services.AddScoped<ISellerWalletService, SellerWalletService>();
        services.AddScoped<ISellerService, SellerService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddHttpClient<IPaymobService, PaymobService>();
        // 3. External & Singletons
        services.AddScoped<GetTokenServices>();
        services.AddScoped<GoogleTokenValidator>();
        services.AddScoped<SendEmailService>();
        services.AddScoped<ISendEmailService, SendEmailService>();
        services.AddScoped<IRedisCahse, RedisCahse>();

        services.AddSingleton<IEmailChannel, EmailChannel>();
        services.AddSingleton<INotificationChannel, NotificationChannel>();


        return services;
    }
}