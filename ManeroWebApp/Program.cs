using DataAccessLibrary.Contexts;
using DataAccessLibrary.Initializers;
using ManeroWebApp.DelegatingHandlers;
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
builder.Services.AddScoped<IJwtAuthService, JwtAuthService>();
builder.Services.AddScoped<IProductControllerService, ProductControllerService>();
builder.Services.AddScoped<IPromoCodeService, PromoCodeService>();

builder.Services.AddTransient<AuthHandler>();

//Add this line of code with YOUR Service if using Authorization
builder.Services.AddHttpClient<IShoppingCartService, ShoppingCartService>().AddHttpMessageHandler<AuthHandler>();
builder.Services.AddHttpClient<IFavoriteService, FavoriteService>().AddHttpMessageHandler<AuthHandler>();
builder.Services.AddHttpClient<IUserService, UserService>().AddHttpMessageHandler<AuthHandler>();

builder.Services.AddHttpClient<IPromoCodeService, PromoCodeService>().AddHttpMessageHandler<AuthHandler>();
builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/SignIn");


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.OnAppendCookie = cookieContext =>
    {
        if (cookieContext.CookieName is not ("Token" or "RefreshToken")) return;
        cookieContext.CookieOptions.Expires = DateTime.UtcNow.AddMonths(12);
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
