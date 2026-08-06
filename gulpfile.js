const { src, dest, watch, series } = require("gulp");
const sass = require("gulp-sass")(require("sass"));

const paths = {
  scssEntry: "./Assets/scss/site.scss",
  scssFiles: "./Assets/scss/**/*.scss",
  cssOutput: "./wwwroot/css/",
};

// Biên dịch SCSS dùng trong quá trình phát triển
function compileScss() {
  return src(paths.scssEntry)
    .pipe(sass.sync().on("error", sass.logError))
    .pipe(dest(paths.cssOutput));
}

// Biên dịch và thu gọn CSS để triển khai
function buildScss() {
  return src(paths.scssEntry)
    .pipe(
      sass
        .sync({
          style: "compressed",
        })
        .on("error", sass.logError),
    )
    .pipe(dest(paths.cssOutput));
}

// Theo dõi tất cả file SCSS
function watchScss() {
  return watch(paths.scssFiles, compileScss);
}

// Chạy compile trước, sau đó mới bắt đầu theo dõi
const development = series(compileScss, watchScss);

exports.scss = compileScss;
exports.build = buildScss;
exports.watch = development;
exports.default = development;
