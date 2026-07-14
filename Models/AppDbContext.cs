using App.Models.Blog;
using App.Models.Contacts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.Models;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        //gọi base đầu tiên để Entity Framework dựng sẵn các bảng Identity
        base.OnModelCreating(builder);

        // Tùy chỉnh Fluent API: Xóa bỏ tiền tố "AspNet" ở tên các bảng Identity để tên bảng đẹp hơn
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName != null && tableName.StartsWith("AspNet"))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }
        builder.Entity<Category>(entity =>
        {
            entity.HasIndex(c => c.Slug);
        });
        builder.Entity<PostCategory>(entity =>
        {
            entity.HasKey(pc => new { pc.PostID, pc.CategoryID });
        });
        builder.Entity<Post>(entity =>
        {
            entity.HasIndex(p => p.Slug);
        });
    }

    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
}
