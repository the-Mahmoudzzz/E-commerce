using e_commerce.core.entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerce.infra.Data
{
    public class AppDbContext:IdentityDbContext<User,IdentityRole<int> ,int>
    {
        public AppDbContext (DbContextOptions<AppDbContext> dbContext):base(dbContext) {}

        public DbSet<User> users { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<DiscountCategry> DiscountCategries { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<SellerWallet> SellerWallets { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetail> orderDetails { get; set; }
        public DbSet<Payment> payments { get; set; }
        public DbSet<Shipment> shipments { get; set; }
        public DbSet<ShippingZone> shippingZones { get; set; }
        public DbSet<ShopingCart>shopingCarts { get; set; }
        public DbSet<ShoppingCartItem> shoppingCartItems { get; set; }
        public DbSet<UserAddresse> userAddresses { get; set; }
        public DbSet<Withdrawal> withdrawals { get; set; }




    }
}
