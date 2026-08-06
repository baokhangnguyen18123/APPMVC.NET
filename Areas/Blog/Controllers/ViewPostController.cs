using App.Models;
using App.Models.Blog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Blog.Controllers;

[Area("Blog")]
public class ViewPostController(ILogger<ViewPostController> _logger, AppDbContext _context)
    : Controller
{
    private const int PostsPerPage = 10;

    // GET: ViewPost
    // /post/{category-slug?}
    [Route("/post/{categorySlug?}")]
    public ActionResult Index(string categorySlug, [FromQuery(Name = "p")] int currentPage = 1)
    {
        var categories = GetCategories();
        ViewBag.categories = categories;
        ViewBag.categorySlug = categorySlug;

        Category category = null;
        if (!string.IsNullOrEmpty(categorySlug))
        {
            category = _context
                .Categories.Where(c => c.Slug == categorySlug)
                .Include(c => c.CategoryChildren)
                .FirstOrDefault();
            if (category == null)
            {
                return NotFound("Không tìm thấy danh mục");
            }
        }
        ViewBag.category = category;

        var posts = _context
            .Posts.Include(c => c.Author)
            .Include(c => c.PostCategories)
                .ThenInclude(p => p.Category)
            .AsQueryable();

        

        if(category != null)
        {
            var ids = new List<int>();
            category.ChildCategoryIDs(ids);
            ids.Add(category.Id);
            posts = posts.Where(p => p.PostCategories.Where(pc => ids.Contains(pc.CategoryID)).Any());
        }
                var query = _context
            .Posts.Include(p => p.Author)
            .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
            .OrderByDescending(p => p.DateCreated)
            .ThenByDescending(p => p.PostId);

        posts = posts.OrderByDescending(p => p.DateUpdated);
        int totalPosts =  posts.Count();
        int countPages = (int)Math.Ceiling((double)totalPosts / PostsPerPage);
        currentPage = Math.Clamp(currentPage, 1, Math.Max(countPages, 1));

        ViewBag.TotalPosts = totalPosts;
        ViewBag.CurrentPage = currentPage;
        ViewBag.CountPages = countPages;
        ViewBag.PostIndex = (currentPage - 1) * PostsPerPage;

        var pagedPosts = posts
        .Skip((currentPage - 1) * PostsPerPage)
        .Take(PostsPerPage)
        .ToList();


        return View(pagedPosts);
    }

    [Route("/post/{postSlug}.html")]
    public ActionResult Details(string postSlug)
    {
        var categories = GetCategories();
        ViewBag.categories = categories;
        var post = _context.Posts.Where(p => p.Slug == postSlug)
            .Include(p => p.Author)
            .Include(p => p.PostCategories)
            .ThenInclude(pc => pc.Category)
            .FirstOrDefault();
        if (post == null)
        {
            return NotFound("Không tìm thấy bài viết");
            
        }
        Category category = post.PostCategories.FirstOrDefault()?.Category;
        ViewBag.category = category;

        var orderPosts = _context.Posts
            .Where(p => p.PostCategories.Any(pc => pc.CategoryID == category.Id))
            .Where(p => p.PostId != post.PostId)
            .OrderByDescending(p => p.DateCreated)
            .Take(5);
        ViewBag.orderPosts = orderPosts;
        return View(post);
    }

    private List<Category> GetCategories()
    {
        var categories = _context
            .Categories.Include(c => c.CategoryChildren)
            .AsEnumerable()
            .Where(c => c.ParentCategory == null)
            .ToList();

        return categories;
    }
}
