using System.ComponentModel.DataAnnotations;
using App.Models.Blog;

namespace AppMvc.Areas.Blog.Models;

public class CreatePostModel : Post
{
    [Display(Name = "Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
    [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
    [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
    public new string? Slug { get; set; }

    [Display(Name = "Chuyên mục")]
    public int[] CategoryIDs { get; set; } = [];
}
