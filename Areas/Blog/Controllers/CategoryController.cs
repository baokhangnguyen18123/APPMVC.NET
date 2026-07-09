using App.Data;
using App.Models;
using App.Models.Blog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace App.Areas.Blog.Controllers;

[Area("Blog")]
[Route("admin/blog/category/[action]/{id?}")]
[Authorize(Roles = RoleName.Administrator)]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var qr = (from c in _context.Categories select c)
            .Include(c => c.ParentCategory)
            .Include(c => c.CategoryChildren);

        var categories = (await qr.ToListAsync())
            .Where(c => c.ParentCategory == null)
            .OrderBy(c => c.Title)
            .ToList();

        return View(categories);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context
            .Categories.Include(c => c.ParentCategory)
            .Include(c => c.CategoryChildren)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    public async Task<IActionResult> Create()
    {
        await LoadParentCategories();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Title,Description,Slug,ParentCategoryId")] Category category
    )
    {
        if (ModelState.IsValid)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        await LoadParentCategories(category.ParentCategoryId);
        return View(category);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context.Categories.FindAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        await LoadParentCategories(category.ParentCategoryId, category.Id);
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Title,Description,Slug,ParentCategoryId")] Category category
    )
    {
        if (id != category.Id)
        {
            return NotFound();
        }

        if (category.ParentCategoryId == category.Id)
        {
            ModelState.AddModelError(
                "ParentCategoryId",
                "Danh mục cha không được trùng với chính nó."
            );
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        await LoadParentCategories(category.ParentCategoryId, category.Id);
        return View(category);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var category = await _context
            .Categories.Include(c => c.ParentCategory)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var category = await _context
            .Categories.Include(c => c.CategoryChildren)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category == null)
        {
            return NotFound();
        }
        foreach (var childCate in category.CategoryChildren)
        {
            childCate.ParentCategoryId = category.ParentCategoryId;
        }
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(c => c.Id == id);
    }

    private async Task LoadParentCategories(int? selectedId = null, int? excludeId = null)
    {
        var categories = await _context
            .Categories.Where(c => excludeId == null || c.Id != excludeId)
            .OrderBy(c => c.Title)
            .ToListAsync();

        ViewData["ParentCategoryId"] = new SelectList(categories, "Id", "Title", selectedId);
    }
}
