using App.Data;
using App.Models;
using App.Models.Blog;
using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Database.Controllers;

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
        // SeedPostCategories();
        await _dbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [ActionName(nameof(SeedPostCategories))]
    public async Task<IActionResult> SeedPostCategoriesAsync()
    {
        SeedPostCategories();
        await _dbContext.SaveChangesAsync();
        StatusMessage = "Đã tạo dữ liệu category mẫu thành công!";
        return RedirectToAction(nameof(Index));
    }

    private void SeedPostCategories()
    {
        _dbContext.Categories.RemoveRange(
            _dbContext.Categories.Where(c => c.Description.Contains("[fakeData]"))
        );
        _dbContext.Posts.RemoveRange(_dbContext.Posts.Where(p => p.Content.Contains("[fakeData]")));

        var fakerCategory = new Faker<Category>();
        int cm = 1;
        fakerCategory.RuleFor(c => c.Title, fk => $"CM{cm++} " + fk.Lorem.Sentence(1, 2).Trim('.'));
        fakerCategory.RuleFor(c => c.Description, fk => fk.Lorem.Sentences(5) + "[fakeData]");
        fakerCategory.RuleFor(c => c.Slug, fk => fk.Lorem.Slug());

        var cate1 = fakerCategory.Generate();
        var cate11 = fakerCategory.Generate();
        var cate12 = fakerCategory.Generate();
        var cate2 = fakerCategory.Generate();
        var cate21 = fakerCategory.Generate();
        var cate211 = fakerCategory.Generate();

        cate11.ParentCategory = cate1;
        cate12.ParentCategory = cate1;
        cate21.ParentCategory = cate2;
        cate211.ParentCategory = cate21;

        var categories = new Category[] { cate1, cate2, cate12, cate11, cate21, cate211 };
        _dbContext.Categories.AddRange(categories);

        // POST
        var rCateIndex = new Random();
        int bv = 1;

        var user = _userManager.GetUserAsync(this.User).Result;
        var fakerPost = new Faker<Post>();
        fakerPost.RuleFor(p => p.AuthorId, f => user.Id);
        fakerPost.RuleFor(p => p.Content, f => f.Lorem.Paragraphs(7) + "[fakeData]");
        fakerPost.RuleFor(
            p => p.DateCreated,
            f => f.Date.Between(new DateTime(2021, 1, 1), new DateTime(2021, 7, 1))
        );
        fakerPost.RuleFor(p => p.Description, f => f.Lorem.Sentences(3));
        fakerPost.RuleFor(p => p.Published, f => true);
        fakerPost.RuleFor(p => p.Slug, f => f.Lorem.Slug());
        fakerPost.RuleFor(p => p.Title, f => $"Bài {bv++} " + f.Lorem.Sentence(3, 4).Trim('.'));

        List<Post> posts = new List<Post>();
        List<PostCategory> post_categories = new List<PostCategory>();

        for (int i = 0; i < 40; i++)
        {
            var post = fakerPost.Generate();
            post.DateUpdated = post.DateCreated;
            posts.Add(post);
            post_categories.Add(
                new PostCategory() { Post = post, Category = categories[rCateIndex.Next(5)] }
            );
        }

        _dbContext.AddRange(posts);
        _dbContext.AddRange(post_categories);
        // END POST
        _dbContext.SaveChanges();
    }
}
