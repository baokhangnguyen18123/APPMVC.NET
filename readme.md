## 1. Tầng Điều khiển (Controller) & Routing

- **Controller (Điều khiển):** Nơi tiếp nhận các HTTP Request từ người dùng. Chứa các hàm xử lý logic (gọi là Action) để trả về các định dạng đa dạng: Giao diện web (View), chuỗi API (`Json`), văn bản (`Content`) hoặc tệp tin (`File`).
- **Routing mặc định:** Hệ thống định tuyến URL thường ánh xạ theo mẫu `{controller}/{action}/{id?}`. Nếu URL bị bỏ trống, hệ thống sẽ mặc định trỏ tới `HomeController` và phương thức `Index`.

## 2. View (Hiển thị)

- Các file `.cshtml` sử dụng Razor Engine để trộn mã C# vào HTML. Hệ thống sẽ tự động dò tìm giao diện tương ứng theo cấu trúc thư mục `Views/{Tên_Controller}/{Tên_Action}.cshtml`.
- **Cấu hình thêm thư mục lưu trữ View:**
  - `{0}` -> Tên Action
  - `{1}` -> Tên Controller
  - `{2}` -> Tên Area
  - _Cú pháp:_ `options.ViewLocationFormats.Add("/MyView/{1}/{0}" + RazorViewEngine.ViewExtension);`

## 3. Cách truyền dữ liệu (Controller -> View)

- **Truyền bằng Model:** Đẩy trực tiếp class dữ liệu sang View (Cách chuẩn xác và được khuyên dùng nhất).
- **ViewData:** Lưu dữ liệu dạng từ điển (Cặp Khóa - Giá trị).
- **ViewBag:** Biến thể của `ViewData`, cho phép gán thuộc tính động (`dynamic`) một cách tự do.
- **TempData:** Dùng Session để giữ dữ liệu sống sót khi chuyển hướng (`Redirect`) từ trang này sang trang khác, đọc xong sẽ tự xóa.

## 4. Tùy biến Middleware Báo Lỗi & Điểm cuối (Endpoints)

- Thay vì để trình duyệt hiện lỗi mặc định, sử dụng `UseStatusCodePages` (thường bọc trong một Extension Method) để tóm các lỗi HTTP (400-599, ví dụ 404) và trả về giao diện HTML thân thiện bằng luồng bất đồng bộ.
- Cấu hình thêm các điểm cuối cơ bản bằng `MapGet` (cho văn bản thuần) và `MapRazorPages` (để hiển thị các trang `.cshtml` độc lập không qua Controller).

## 5. Định tuyến Truyền thống (Conventional Routing)

- Dùng phương thức `MapControllerRoute` để thiết lập quy tắc URL tập trung theo mẫu mặc định `{controller=Home}/{action=Index}/{id?}`.
- Để bảo vệ dữ liệu đầu vào, áp dụng Ràng buộc (Route Constraints) trực tiếp lên chuỗi URL như giới hạn kiểu số nguyên (`:int`), khoảng giá trị (`:range`), hoặc dùng biểu thức chính quy (Regex). Nếu dữ liệu sai, hệ thống lập tức trả về lỗi 404.

## 6. Định tuyến bằng Thuộc tính (Attribute Routing)

- Gắn trực tiếp thẻ `[Route("...")]`, `[HttpGet]`, `[HttpPost]` lên các lớp Controller và Action để linh hoạt tạo ra các URL tùy biến (như `/he-mat-troi/sao-hoa`).
- Định tuyến bằng Attribute sẽ làm vô hiệu hóa định tuyến truyền thống đối với Controller đó. Nếu một Action có nhiều đường dẫn, dùng thuộc tính `Order` để phân định độ ưu tiên.

## 7. Phân vùng Ứng dụng (Area)

- **Chia nhỏ dự án:** Bắt buộc phải khai báo thuộc tính `[Area("Tên_Area")]` ngay trên khai báo lớp Controller.
- **Định tuyến riêng biệt:** Bắt buộc phải có một bộ định tuyến độc lập là `MapAreaControllerRoute` (với tham số `areaName`) để hệ thống nhận diện được phân vùng.
- **Cấu trúc thư mục:** Thay đổi cấu trúc và phải tuân thủ nghiêm ngặt định dạng `Areas/Tên_Area/Views/Tên_Controller/` (có thể sử dụng lệnh `dotnet-aspnet-codegenerator area` để tự động tạo).

## 8. Khởi tạo Liên kết Tự động (URL Generation)

- **Nguyên tắc tuyệt đối:** Không gõ cứng (hard-code) địa chỉ URL vào mã nguồn.
- **Trong mã C#:** Dùng đối tượng `UrlHelper` (viết là `@Url.Action` hoặc `@Url.RouteUrl`) để hệ thống tự tính toán đường dẫn.
- **Trong mã HTML:** Sử dụng các Tag Helpers như `asp-controller`, `asp-action` và `asp-area`. Nếu cần truyền hàng loạt tham số, hãy đóng gói chúng vào một đối tượng `Dictionary` và truyền qua thuộc tính `asp-all-route-data`.
