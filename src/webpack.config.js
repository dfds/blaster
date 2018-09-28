const webpack = require("webpack");
const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        dashboard: "./Blaster.WebApi/Features/Dashboards/main.js",
        teams: "./Blaster.WebApi/Features/Teams/main.js",
        aws: "./Blaster.WebApi/Features/AWS/main.js",
    },
    output: {
        path: path.resolve(__dirname, "Blaster.WebApi", "wwwroot"),
        filename: "[name].bundle.js"
    },
    resolve: {
        alias: {
            vue: 'vue/dist/vue.js'
        },
        extensions: [".js", ".scss"]
    },
    devtool: 'source-map',
    plugins: [
        new MiniCssExtractPlugin({
            // Options similar to the same options in webpackOptions.output
            // both options are optional
            filename: "[name].css",
            // chunkFilename: "[id].css"
        })
    ],
    module: {
        rules: [{
                test: /\.js$/,
                loader: 'babel-loader',
                exclude: /node_modules/
            },
            // {
            //     test: /\.css$/,
            //     use: [{
            //             loader: "style-loader"
            //         },
            //         {
            //             loader: "css-loader",
            //             options: {
            //                 sourceMap: true
            //             }
            //         }
            //     ]
            // },
            {
                test: /\.(sa|sc|c)ss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    // "style-loader",
                    "css-loader",
                    "sass-loader",
                ]
                // use: [{
                //         loader: "style-loader"
                //     },
                //     {
                //         loader: "css-loader",
                //         options: {
                //             sourceMap: true
                //         }
                //     },
                //     {
                //         loader: "sass-loader",
                //         options: {
                //             sourceMap: true
                //         }
                //     }
                // ]
            }
        ]
    }
}