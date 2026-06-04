using App.Services;
using App.Models;
using Microsoft.AspNetCore.Mvc;
namespace App.FirstController;


public class FirstController(ILogger<FirstController> _logger, IWebHostEnvironment _env, ProductService _productService) : Controller
{

    public string Index()
    {
        _logger.LogInformation("Đây là log thông tin từ FirstController");
        _logger.LogWarning("Đây là log cảnh báo từ FirstController");
        _logger.LogError("Đây là log lỗi từ FirstController");
        _logger.LogDebug("Đây là log debug từ FirstController");
        return "Xin chào FirstController";
    }
    public void Nothing()
    {
        _logger.LogInformation("Nothing action đã được gọi");
        Response.Headers.Add("Hi", "Xin chao tu Nothing action");
    }
    public object Anything()
    {
        _logger.LogInformation("Anything action đã được gọi");
        return new { Message = "Xin chào từ Anything action", Timestamp = DateTime.Now };
    }
    public IActionResult Readme()
    {
        _logger.LogInformation("Readme action đã được gọi");
        var content = "Xin chào từ Readme action";
        return Content(content,"text/plain");
        // return File("", "image/png");
    }
    public IActionResult ViewImg()
    {
        _logger.LogInformation("Đang xử lý đọc file ảnh...");
        
        // Path.Combine -> auto (/ or \)
        string filepath = Path.Combine(_env.ContentRootPath, "Files", "field.png");

        if (!System.IO.File.Exists(filepath))
        {
            return NotFound("Không tìm thấy hình ảnh yêu cầu.");
        }

        byte[] fileBytes = System.IO.File.ReadAllBytes(filepath);
        
        return File(fileBytes, "image/jpeg");
    }
    public IActionResult ProductInfo()
    {
        _logger.LogInformation("Đang xử lý trả về thông tin sản phẩm dạng json...");
        return Json(
            new { Id = 1, Name = "Sản phẩm A", Price = 100.0 }
        );
    }
    public IActionResult Privacy()
    {
        var url = Url.Action("Privacy", "Home");
        _logger.LogInformation("Chuyển hướng đến trang Privacy...");
        return LocalRedirect(url);
    }
    public IActionResult Google()
    {
        var url = "https://www.google.com";
        _logger.LogInformation("Chuyển hướng đến Google...");
        return Redirect(url);
    }
    public IActionResult HelloView(string userName)
    {
        if(string.IsNullOrEmpty(userName))
        {
            userName = "Khách";
        }
        // return View("/MyView/XinChao1.cshtml",userName);
        return View("XinChao2",userName);
    }
 
    public IActionResult ViewProduct(int? id)
    {
        var product = _productService.FirstOrDefault(p => p.Id == id);
        // if(product == null)
        // {
        //     return NotFound();
        // }
        if (product == null)
        {
            // CÁCH 4: TempData - Dùng để chuyển dữ liệu sang một trang khác (khi Redirect)
            // Dữ liệu lưu qua Session, đọc xong 1 lần sẽ tự xóa.
            TempData["StatusMessage"] = "Sản phẩm bạn yêu cầu không tồn tại!";
            return RedirectToAction("Index", "Home");
        }
        //View/first/ViewProduct.cshtml
        //MyView/First/ViewProduct.cshtml
        // CÁCH 1: Model (Strongly-typed) - Cách chuẩn và khuyên dùng nhất
        // Truyền thẳng đối tượng dữ liệu vào phương thức View()
        // return View(product); 
        // CÁCH 2: ViewData - Dạng Dictionary (Key-Value)
        // Thường dùng cho các cấu hình nhỏ lẻ như tiêu đề trang
        // ViewData["Product"] = product;
        // ViewData["Title"] = product.Name;
        // return View("ViewProduct2");
        
        // CÁCH 3: ViewBag - Dạng Dynamic 
        // Linh hoạt hơn ViewData, truy cập thuộc tính trực tiếp tại thời điểm thực thi (Runtime)
        ViewBag.Product = product;
        return View("ViewProduct3");


    }
}