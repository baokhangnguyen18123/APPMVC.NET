using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/db-manage/[Action]")]
    public class DbManage(
        AppDbContext _dbContext,
        ILogger<DbManage> _logger,
        UserManager<AppUser> _userManager,
        RoleManager<IdentityRole> _roleManager
    ) : Controller
    {
        [TempData]
        public string StatusMessage { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }

        [HttpPost]
        [ActionName("DeleteDb")]
        public async Task<IActionResult> DeleteDbAsync()
        {
            bool success = await _dbContext.Database.EnsureDeletedAsync();
            StatusMessage = success ? "Xóa database thành công!" : "Không thể xóa data";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Migrate()
        {
            await _dbContext.Database.MigrateAsync();
            StatusMessage = "Cập nhật database thành công!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SeedDataAsync()
        {
            var roleNames = typeof(RoleName).GetFields().ToList();
            foreach (var r in roleNames)
            {
                var roleName = (string)r.GetRawConstantValue();
                var rfound = await _roleManager.FindByNameAsync(roleName);
                if (rfound == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            //admin - pass= Admin@123!, admin@example.com
            var adminUser = await _userManager.FindByNameAsync("admin");
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                };
                var result = await _userManager.CreateAsync(adminUser, "Admin@123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, RoleName.Administrator);
                    StatusMessage = "Đã tạo dữ liệu mẫu và tài khoản Admin thành công!";
                }
                else
                {
                    // Bắt lỗi nếu Password quá yếu hoặc User không hợp lệ
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    StatusMessage = $"Lỗi tạo Admin: {errors}";
                    _logger.LogError("Không thể tạo admin user. Lỗi: {Errors}", errors);
                }
                await _userManager.AddToRoleAsync(adminUser, RoleName.Administrator);
            }
            else
            {
                StatusMessage = "Tài khoản Admin đã tồn tại từ trước.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
