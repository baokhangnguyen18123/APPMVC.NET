using App.Services;
using App.Extensions;
using Microsoft.AspNetCore.Routing.Constraints;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Đăng ký dịch vụ Razor Pages
builder.Services.AddRazorPages();

// Đăng ký dịch vụ ProductService
builder.Services.AddSingleton<ProductService>();
// builder.Services.AddSingleton<ProductService, ProductService>();
// builder.Services.AddSingleton(typeof(ProductService), typeof(ProductService));
// builder.Services.AddSingleton(typeof(ProductService));
builder.Services.AddSingleton<PlanetService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCustomStatusCodePages();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();
app.MapGet("/sayhi", async (HttpContext context) =>
{
    await context.Response.WriteAsync($"Hello MVC.NET {DateTime.Now}!");
    
});

//Custom route ->
app.MapControllerRoute(
    name: "FirstRoute",
    pattern: "{url:regex(^((p)|(sp)|(info)|(sanpham))$)}/{id:range(1,3)}",
    defaults: new { controller = "First", action = "ViewProduct" }
    ).WithStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
//app.MapControllerRoute -> Định nghĩa route cho controller, action và tham số id tùy chọn(0 hỗ trợ area)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
