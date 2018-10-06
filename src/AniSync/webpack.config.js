var path = require("path");
var webpack = require("webpack");

module.exports = function (env) {
    env = env || {};

    const config = {
        mode: env.NODE_ENV === "production" ? "" : "development",
        entry: "./Client/js/vendor.js",
        output: {
            path: path.resolve(__dirname, "wwwroot", "dist"),
            filename: "vendor.js"
        },
        module: {
            rules: [{
                test: require.resolve("jquery"),
                use: [{
                    loader: "expose-loader",
                    options: "jQuery"
                }, {
                    loader: "expose-loader",
                    options: "$"
                }]
            }]
        }
    };

    return config;
};