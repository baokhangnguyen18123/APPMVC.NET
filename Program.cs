using App.Services;
using App.Extensions;
using Microsoft.AspNetCore.Routing.Constraints;
using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


//Đăng ký DBContext -> SQLServer
builder.Services.AddDbContext<AppDbContext>(options =>
{
    // ConnectString from AppSetting
    var connectionString = builder.Configuration.GetConnectionString("AppMvcConnectionString");
    options.UseSqlServer(connectionString);
});
//Đăng kí Identity
builder.Services.AddIdentity<AppUser, IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddAuthentication()
.AddGoogle(options =>
{
    var googleConfig = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleConfig["ClientId"];
    options.ClientSecret = googleConfig["ClientSecret"];
    options.CallbackPath = "/login-google";
})
// .AddTwitter()
.AddFacebook(options =>
{
    // Lấy cấu hình từ appsettings.json
    options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
    options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    options.CallbackPath = "/login-facebook";
})
// .AddMicrosoftAccount()
;
builder.Services.ConfigureApplicationCookie(options =>
{
   options.LoginPath ="/Login/";
   options.LogoutPath="/Logout/";
   options.AccessDeniedPath = "/khongduoctruycap.html";


});

// Truy cập IdentityOptions
builder.Services.Configure<IdentityOptions> (options => {
    // Thiết lập về Password
    options.Password.RequireDigit = false; // Không bắt phải có số
    options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
    options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
    options.Password.RequireUppercase = false; // Không bắt buộc chữ in
    options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
    options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

    // Cấu hình Lockout - khóa user
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (5); // Khóa 5 phút
    options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
    options.Lockout.AllowedForNewUsers = true;

    // Cấu hình về User.
    options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;  // Email là duy nhất

    // Cấu hình đăng nhập.
    options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
    options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
    options.SignIn.RequireConfirmedAccount = true;

});



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
