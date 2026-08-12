using AutoMapper;
using e_commerce.api.Extensions;
using e_commerce.api.Middleware;
using e_commerce.app.External;
using e_commerce.app.Services.Implementation;
using e_commerce.app.Services.IServices;
using Microsoft.AspNetCore.HttpOverrides;



namespace e_commerce.api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddInfrastructureServices(builder.Configuration); // بيكلم الـ Infra
            builder.Services.AddApplicationServices();                         // بيكلم الـ App
            builder.Services.AddApiConfigurations(builder.Configuration);
            
            builder.Services.AddScoped<IPhotoService, PhotoService>();

            var app = builder.Build();
            //            using (var scope = app.Services.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    db.Database.Migrate();
            //}
            app.UseGlobalExceptionHandler();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() ||
    app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "E-Commerce API v1");
                });
            }


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapHub<NotificationHub>("/notificationHub");
            app.MapControllers();

            app.Run();
        }
    }
}
