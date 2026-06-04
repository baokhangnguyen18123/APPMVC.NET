using App.Models;

namespace App.Services;
public class ProductService : List<ProductModel>
{
    public ProductService()
    {
        this.AddRange(new ProductModel[]{
            new ProductModel() { Id = 1, Name = "Sản phẩm A", Price = 100.0 },
            new ProductModel() { Id = 2, Name = "Sản phẩm B", Price = 200.0 },
            new ProductModel() { Id = 3, Name = "Sản phẩm C", Price = 300.0 }
        });
    }
}