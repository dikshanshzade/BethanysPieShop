using BethanysPieShop.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPieRepository, PieRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp=>ShoppingCart.GetCart(sp));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler=ReferenceHandler.IgnoreCycles;
});

builder.Services.AddRazorPages();
//builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddDbContext<BethanysPieShopDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("BethanysPieShopDbContextConnection")); });

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<BethanysPieShopDbContext>();


var app = builder.Build();

app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapDefaultControllerRoute();
//app.UseAntiForgery();
app.MapRazorPages();
//app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
DbInitializer.Seed(app);
app.Run();
