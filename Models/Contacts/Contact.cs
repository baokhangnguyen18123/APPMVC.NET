using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace App.Models.Contacts;
public class Contact
{
    [Key]
    public int ID {get; set;}
    [Column(TypeName = "nvarchar")]
    [StringLength(50)] 
    [Required(ErrorMessage = "Phải nhập họ tên")] 
    [Display(Name = "Họ tên")] 
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Phải nhập email")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Địa chỉ email phải hợp lệ")] 
    [Display(Name = "Địa chỉ email")]
    public string Email { get; set; } = null!;

    public DateTime DateSent { get; set; }

    [Display(Name = "Số điện thoại")]
    [StringLength(50)]
    [Phone(ErrorMessage = "Số điện thoại phải hợp lệ")] 
    public string? Phone { get; set; }

    [Display(Name = "Nội dung")]
    public string? Message { get; set; }
}

