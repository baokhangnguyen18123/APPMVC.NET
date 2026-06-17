using Microsoft.EntityFrameworkCore;
namespace App.Models;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
    {
    
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        //gọi base đầu tiên để Entity Framework dựng sẵn các bảng Identity
        base.OnModelCreating(builder);

        // Tùy chỉnh Fluent API: Xóa bỏ tiền tố "AspNet" ở tên các bảng Identity để tên bảng đẹp hơn
        // foreach (var entityType in builder.Model.GetEntityTypes())
        // {
        //     var tableName = entityType.GetTableName();
        //     if (tableName != null && tableName.StartsWith("AspNet"))
        //     {
        //         entityType.SetTableName(tableName.Substring(6));
        //     }
        // }
    }
}