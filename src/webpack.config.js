const webpack = require("webpack");
const path = require("path");

module.exports = {
    entry: {
        dashboard: "./Blaster.WebApi/Features/Dashboards/main.js"
    },
    output: {
        path: path.resolve(__dirname, "Blaster.WebApi", "wwwroot"),
        filename: "[name].bundle.js"
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                loader: 'babel-loader',
                exclude: /node_modules/
            }
        ]
    }
}