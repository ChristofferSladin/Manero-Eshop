using DataAccessLibrary.Contexts;
using DataAccessLibrary.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using UserAPI.Services;

namespace UserAPI.Tests.Helpers;

public class TestWebApplication : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddDbContext<ManeroDbContext>(x => x.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));
            services.AddScoped<FavoriteRepository>();
            services.AddScoped<FavoriteProductRepository>();
            services.AddScoped<ShoppingCartRepository>();
            services.AddScoped<ShoppingCartProductRepository>();
            services.AddScoped<ProductRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<UserPromoCodeRepository>();
            services.AddScoped<PromoCodeRepository>();
        });
    }
}
