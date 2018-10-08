let gulp = require("gulp");

// Sub tasks.
require("./gulpfile.vendor");

// Main tasks to use.
gulp.task("vendor", ["vendor-js", "vendor-css", "vendor-fonts"]);

gulp.task("prod", ["vendor"]);