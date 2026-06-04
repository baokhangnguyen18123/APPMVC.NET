## 1. Tầng Điều khiển (Controller) & Routing

    - Controller (Điều khiển): Nơi tiếp nhận các HTTP Request từ người dùng. Chứa các hàm xử lý logic (gọi là Action) để trả về các định dạng đa dạng: Giao diện web (View), chuỗi API (Json), văn bản (Content) hoặc tệp tin (File)
    - Routing mặc định: Hệ thống định tuyến URL thường ánh xạ theo mẫu {controller}/{action}/{id?}
    Nếu URL bị bỏ trống, hệ thống sẽ mặc định trỏ tới HomeController và phương thức Index

## View (Hiển thị):

    - Các file .cshtml sử dụng Razor Engine để trộn mã C# vào HTML. Hệ thống sẽ tự động dò tìm giao diện tương ứng theo cấu trúc thư mục Views/{Tên_Controller}/{Tên_Action}.cshtml
    -Thêm thư mục lưu trữ View:
        +{0} -> tên Action
        +{1} -> tên Controller
        +{2} -> tên Area

        -> options.ViewLocationFormats.Add("/MyView/{1}/{0}"+ RazorViewEngine.ViewExtension);

## Cách truyền dữ liệu (Controller -> View):

    - Truyền bằng Model: Đẩy trực tiếp class dữ liệu sang View (Cách chuẩn xác và được khuyên dùng nhất)
    - ViewData: Lưu dữ liệu dạng từ điển (Cặp Khóa - Giá trị)
    - ViewBag: Biến thể của ViewData, cho phép gán thuộc tính động (dynamic) một cách tự do
    - TempData: Dùng Session để giữ dữ liệu sống sót khi chuyển hướng (Redirect) từ trang này sang trang khác, đọc xong sẽ tự xóa
