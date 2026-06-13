using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Areas.ProductManage.Controllers;
[Area("ProductManage")]
public class ProductController(ProductService _productService, ILogger<ProductController> _logger) : Controller
{
    [Route("/cac-sp")]
    public IActionResult Index()
    {
        var products = _productService.OrderBy(p => p.Name).ToList();
        return View(products);
    }
}