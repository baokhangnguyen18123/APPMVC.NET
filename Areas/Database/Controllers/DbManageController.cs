using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/db-manage/[Action]")]
    public class DbManage(AppDbContext _dbContext, ILogger<DbManage> _logger) : Controller
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


    }
}
