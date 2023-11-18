using System.Diagnostics;
using DataAccessLibrary.Contexts;
using DataAccessLibrary.Initializers;
using ManeroWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ManeroDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<ManeroDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<DataInitializer>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IJwtAuthenticationService, JwtAuthenticationService>();
builder.Services.AddScoped<IProductControllerService, ProductControllerService>();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.OnAppendCookie = cookieContext =>
    {
        if (cookieContext.CookieName is not ("Token" or "RefreshToken")) return;
        cookieContext.CookieOptions.Expires = DateTime.UtcNow.AddDays(7);
        cookieContext.CookieOptions.HttpOnly = true;
        cookieContext.CookieOptions.SameSite = SameSiteMode.Strict;
        cookieContext.CookieOptions.Secure = true;
    };
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<DataInitializer>().SeedData();
}

app.Use(async (context, next) =>
{
    context.Request.Headers.Remove("Authorization");
    var authService = context.RequestServices.GetRequiredService<IJwtAuthenticationService>();
    var token = await authService.RefreshTokenIfExpired();
    if (!string.IsNullOrEmpty(token))
    {
        context.Request.Headers.Add("Authorization", "Bearer " + token); 
    }
    await next(context);
});

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
