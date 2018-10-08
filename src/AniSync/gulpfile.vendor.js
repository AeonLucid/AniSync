let gulp = require("gulp");
let concat = require("gulp-concat");
let uglify = require("gulp-uglify");
let replace = require("gulp-replace");
let cleancss = require("gulp-clean-css");

gulp.task("vendor-js", function () {
    return gulp.src([
            "./node_modules/jquery/dist/jquery.js",
            "./node_modules/popper.js/dist/umd/popper.js",
            "./node_modules/bootstrap/dist/js/bootstrap.js",
            "./node_modules/vue/dist/vue.js"
        ])
        .pipe(concat("vendor.min.js"))
        .pipe(uglify())
        .pipe(gulp.dest(__dirname + "/wwwroot/dist/js"));
});

gulp.task("vendor-css", function () {
    return gulp.src([
            "./node_modules/bootswatch/dist/darkly/bootstrap.css",
            "./node_modules/font-awesome/css/font-awesome.css"
        ])
        .pipe(concat("vendor.min.css"))
        .pipe(replace("../fonts/fontawesome", "/dist/fonts/fontawesome"))
        .pipe(cleancss({ compatibility: "ie8" }))
        .pipe(gulp.dest(__dirname + "/wwwroot/dist/css"));
});

gulp.task("vendor-fonts", function () {
    return gulp.src([
        "./node_modules/font-awesome/fonts/*.*"
    ]).pipe(gulp.dest(__dirname + "/wwwroot/dist/fonts"));
});