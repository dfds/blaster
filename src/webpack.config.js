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
    devtool: 'source-map',
    module: {
        rules: [
            {
                test: /\.js$/,
                loader: 'babel-loader',
                exclude: /node_modules/
            },
            {
                test: /\.css$/,
                use: [
                    {
                        loader: "style-loader"
                    },
                    {
                        loader: "css-loader",
                        options: {
                            sourceMap: true
                        }
                    }
                ]
            },
            {
                test: /\.s(c|a)ss$/,
                use: [
                    {
                        loader: "style-loader"
                    },
                    {
                        loader: "css-loader",
                        options: {
                            sourceMap: true
                        }
                    },
                    {
                        loader: "sass-loader",
                        options: {
                            sourceMap: true
                        }
                    }]
            }
        ]
    }
}