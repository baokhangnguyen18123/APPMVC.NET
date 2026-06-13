using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace App.Extensions;

// Bắt buộc -> static class để chứa extension method
public static class AppExtends
{
    // Phương thức mở rộng cho IApplicationBuilder -> use this
    public static void UseCustomStatusCodePages(this IApplicationBuilder app)
    {
        // UseStatusCodePages -> Truyền vào một hàm xử lý lỗi tùy chỉnh
        app.UseStatusCodePages(appError =>
        {
            appError.Run(async context =>
            {
                // Lấy thông tin mã trạng thái HTTP bị lỗi (VD: 404, 403, 500...)
                var code = context.Response.StatusCode;
                // Ép kiểu mã số sang dạng Enum HttpStatusCode để lấy tên lỗi (VD: NotFound, Forbidden)
                var codeName = ((HttpStatusCode)code).ToString();
                // Thiết lập header định dạng trả về là HTML có hỗ trợ tiếng Việt (UTF-8)
                context.Response.ContentType = "text/html; charset=utf-8";
                // Dựng giao diện HTML/CSS tùy chỉnh
                var html = $@"
                    <html>
                        <head>
                            <title>Xin lỗi, đã xảy ra lỗi {code}</title>
                            <style>
                                .error-text {{ 
                                    color: red; 
                                    font-size: 30px; 
                                    font-weight: bold;
                                    text-align: center;
                                    margin:0 auto; 
                                }}
                            </style>
                        </head>
                        <body>
                            <p class='error-text'>Có lỗi xảy ra: {code} - {codeName}</p>
                        </body>
                    </html>";

                // Viết chuỗi HTML này trả về cho Client bằng luồng bất đồng bộ
                await context.Response.WriteAsync(html);
            });
        });
    }
}