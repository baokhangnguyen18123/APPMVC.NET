using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Data;
using App.Models;
using App.Models.Blog;
using App.Utilities;
using AppMvc.Areas.Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Blog.Controllers;

[Area("Blog")]
[Route("admin/blog/post/[action]/{id?}")]
[Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
public class PostController(AppDbContext _context, UserManager<AppUser> _userManager) : Controller
{
    private const int PostsPerPage = 10;

    [TempData]
    public string StatusMessage { get; set; }

    // GET: Post
    public async Task<IActionResult> Index([FromQuery(Name = "p")] int currentPage = 1)
    {
        var query = _context
            .Posts.Include(p => p.Author)
            .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
            .OrderByDescending(p => p.DateCreated)
            .ThenByDescending(p => p.PostId);

        int totalPosts = await query.CountAsync();
        int countPages = (int)Math.Ceiling((double)totalPosts / PostsPerPage);
        currentPage = Math.Clamp(currentPage, 1, Math.Max(countPages, 1));

        ViewBag.TotalPosts = totalPosts;
        ViewBag.CurrentPage = currentPage;
        ViewBag.CountPages = countPages;
        ViewBag.PostIndex = (currentPage - 1) * PostsPerPage;

        var posts = await query
            .Skip((currentPage - 1) * PostsPerPage)
            .Take(PostsPerPage)
            .ToListAsync();

        return View(posts);
    }

    // GET: Post/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context
            .Posts.Include(p => p.Author)
            .FirstOrDefaultAsync(m => m.PostId == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // GET: Post/Create
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        var categories = await _context.Categories.ToListAsync();

        ViewData["CategoryIds"] = new MultiSelectList(
            categories,
            nameof(Category.Id),
            nameof(Category.Title)
        );
        return View();
    }

    // POST: Post/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Title,Description,Slug,Content,Published,CategoryIDs")] CreatePostModel post
    )
    {
        var categories = await _context.Categories.ToListAsync();
        ViewData["CategoryIds"] = new MultiSelectList(
            categories,
            nameof(Category.Id),
            nameof(Category.Title),
            post.CategoryIDs
        );
        ModelState.Remove(nameof(Post.PostCategories));
        ModelState.Remove(nameof(Post.AuthorId));
        ModelState.Remove(nameof(Post.Author));

        if (string.IsNullOrWhiteSpace(post.Slug) && !string.IsNullOrWhiteSpace(post.Title))
        {
            post.Slug = AppUtilities.GenerateSlug(post.Title, hierarchical: false);
            ModelState.Remove(nameof(post.Slug));
            if (string.IsNullOrEmpty(post.Slug))
            {
                ModelState.AddModelError(nameof(post.Slug), "Không thể tạo slug từ tiêu đề.");
            }
        }
        if (
            !string.IsNullOrEmpty(post.Slug)
            && await _context.Posts.AnyAsync(p => p.Slug == post.Slug)
        )
        {
            ModelState.AddModelError(
                nameof(post.Slug),
                "Slug đã tồn tại. Vui lòng chọn slug khác."
            );
        }
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return Challenge();
            }

            var now = DateTime.Now;

            var newPost = new Post
            {
                Title = post.Title,
                Description = post.Description,
                Slug = post.Slug!,
                Content = post.Content,
                Published = post.Published,
                AuthorId = user.Id,
                DateCreated = now,
                DateUpdated = now,
                PostCategories = new List<PostCategory>(),
            };

            var selectedCategoryIds = post.CategoryIDs.ToHashSet();
            foreach (var category in categories.Where(c => selectedCategoryIds.Contains(c.Id)))
            {
                newPost.PostCategories.Add(
                    new PostCategory
                    {
                        Post = newPost,
                        CategoryID = category.Id,
                        Category = category,
                    }
                );
            }

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();
            StatusMessage = "Vừa tạo bài viết mới";
            return RedirectToAction(nameof(Index));
        }
        return View(post);
    }

    // GET: Post/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context
            .Posts.Include(p => p.PostCategories)
            .FirstOrDefaultAsync(p => p.PostId == id);
        if (post == null)
        {
            return NotFound();
        }
        var postEdit = new CreatePostModel
        {
            PostId = post.PostId,
            Title = post.Title,
            Description = post.Description,
            Slug = post.Slug,
            Content = post.Content,
            Published = post.Published,
            CategoryIDs = post.PostCategories.Select(pc => pc.CategoryID).ToArray(),
        };
        var categories = await _context.Categories.ToListAsync();
        ViewData["CategoryIds"] = new MultiSelectList(
            categories,
            nameof(Category.Id),
            nameof(Category.Title)
        );
        return View(postEdit);
    }

    // POST: Post/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("PostId,Title,Description,Slug,Content,Published,CategoryIDs")] CreatePostModel post
    )
    {
        if (id != post.PostId)
        {
            return NotFound();
        }
        var categories = await _context.Categories.ToListAsync();
        ViewData["CategoryIds"] = new MultiSelectList(
            categories,
            nameof(Category.Id),
            nameof(Category.Title),
            post.CategoryIDs
        );
        ModelState.Remove(nameof(Post.PostCategories));
        ModelState.Remove(nameof(Post.AuthorId));
        ModelState.Remove(nameof(Post.Author));

        if (string.IsNullOrWhiteSpace(post.Slug) && !string.IsNullOrWhiteSpace(post.Title))
        {
            post.Slug = AppUtilities.GenerateSlug(post.Title, hierarchical: false);
            ModelState.Remove(nameof(post.Slug));
            if (string.IsNullOrEmpty(post.Slug))
            {
                ModelState.AddModelError(nameof(post.Slug), "Không thể tạo slug từ tiêu đề.");
            }
        }
        if (
            !string.IsNullOrEmpty(post.Slug)
            && await _context.Posts.AnyAsync(p => p.PostId != id && p.Slug == post.Slug)
        )
        {
            ModelState.AddModelError(
                nameof(post.Slug),
                "Slug đã tồn tại. Vui lòng chọn slug khác."
            );
        }
        if (ModelState.IsValid)
        {
            try
            {
                var postUpdate = await _context
                    .Posts.Include(p => p.PostCategories)
                    .FirstOrDefaultAsync(p => p.PostId == id);
                if (postUpdate == null)
                {
                    return NotFound();
                }
                postUpdate.Title = post.Title;
                postUpdate.Description = post.Description;
                postUpdate.Content = post.Content;
                postUpdate.Published = post.Published;
                postUpdate.Slug = post.Slug;
                postUpdate.DateUpdated = DateTime.Now;

                // Update PostCategory
                if (post.CategoryIDs == null)
                    post.CategoryIDs = new int[] { };

                var oldCateIds = postUpdate.PostCategories.Select(c => c.CategoryID).ToArray();
                var newCateIds = post.CategoryIDs;

                var removeCatePosts =
                    from postCate in postUpdate.PostCategories
                    where (!newCateIds.Contains(postCate.CategoryID))
                    select postCate;
                _context.PostCategories.RemoveRange(removeCatePosts);

                var addCateIds =
                    from CateId in newCateIds
                    where !oldCateIds.Contains(CateId)
                    select CateId;

                foreach (var CateId in addCateIds)
                {
                    _context.PostCategories.Add(
                        new PostCategory() { PostID = id, CategoryID = CateId }
                    );
                }
                _context.Update(postUpdate);
                _context.Entry(postUpdate).Property(p => p.DateCreated).IsModified = false;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.PostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "UserName", post.AuthorId);
        return View(post);
    }

    // GET: Post/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var post = await _context
            .Posts.Include(p => p.Author)
            .FirstOrDefaultAsync(m => m.PostId == id);
        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    // POST: Post/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        if (post != null)
        {
            _context.Posts.Remove(post);
        }

        await _context.SaveChangesAsync();
        StatusMessage = $"Bạn vừa xóa bài viết có tiêu đề: {post?.Title}";
        return RedirectToAction(nameof(Index));
    }

    private bool PostExists(int id)
    {
        return _context.Posts.Any(e => e.PostId == id);
    }
}
