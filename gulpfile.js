const gulp = require("gulp");
const sass = require("gulp-sass")(require("sass"));
const cssmin = require("gulp-cssmin");

// Tác vụ 1: Biên dịch và tối ưu hóa SCSS thành CSS (Tác vụ mặc định)
gulp.task("default", function () {
  // Chỉ định đường dẫn đọc toàn bộ các tệp .scss
  return (
    gulp
      .src("./assets/scss/site.scss")
      .pipe(sass().on("error", sass.logError)) // Biên dịch SCSS sang CSS
      // .pipe(cssmin()) // Xóa bỏ khoảng trắng và ký tự thừa để thu gọn dung lượng
      .pipe(gulp.dest("./wwwroot/css/"))
  ); // Xuất tệp CSS đích vào thư mục wwwroot để ASP.NET Core sử dụng
});

// Tác vụ 2: Giám sát mã nguồn (Watch)
gulp.task("watch", function () {
  // Treo tiến trình và giám sát liên tục, gọi lại tác vụ 'default' ngay khi có sự thay đổi
  gulp.watch("./assets/scss/site.scss", gulp.series("default"));
});
