const webpack = require("webpack");
const path = require("path");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = {
    entry: {
        main: "./Blaster.WebApi/Features/Shared/index.js",
        capabilities: "./Blaster.WebApi/Features/Capabilities/main.js",
        frontpage: "./Blaster.WebApi/Features/Frontpage/main.js",
        login: "./Blaster.WebApi/Features/Login/main.js",
//        topics: "./Blaster.WebApi/Features/Topic/main.js",
        capabilitydashboard: "./Blaster.WebApi/Features/CapabilityDashboard/main.js",
        featureflags: "./Blaster.WebApi/Features/FeatureFlag/main.js"
//        topicdetails: "./Blaster.WebApi/Features/TopicDetails/main.js",
    },
    output: {
        path: path.resolve(__dirname, "Blaster.WebApi", "wwwroot"),
        filename: "[name].bundle.js"
    },
    resolve: {
        alias: {
            vue: 'vue/dist/vue.js',
            httpclient$: path.resolve(__dirname, "Blaster.WebApi/Features/Shared/httpclient.js"),
            userservice$: path.resolve(__dirname, "Blaster.WebApi/Features/Shared/userservice.js"),
            alertdialog$: path.resolve(__dirname, "Blaster.WebApi/Features/Shared/alert-dialog.js"),
            modeleditor$: path.resolve(__dirname, "Blaster.WebApi/Features/Shared/model-editor.js"),
            capabilityservice$: path.resolve(__dirname, "Blaster.WebApi/Features/Shared/capabilityservice.js"),
            topicservice$: path.resolve(__dirname, "Blaster.WebApi/Features/Shared/topicservice.js"),
            featureflag$: path.resolve(__dirname, "Blaster.WebApi/Features/Shared/feature-flag.js"),
            channelservice$: path.resolve(__dirname, "Blaster.WebApi/Features/Shared/channelservice.js"),
            connectionservice$: path.resolve(__dirname, "Blaster.WebApi/Features/Shared/connectionservice.js"),
            "keypattern-shortcut$": path.resolve(__dirname, "Blaster.WebApi/Features/Shared/keypattern_shortcut.js")
        },
        extensions: [".js", ".scss", ".css"]
    },
    devtool: 'source-map',
    plugins: [
        new MiniCssExtractPlugin({
            filename: "[name].css",
        })
	],
    watchOptions: {
	    aggregateTimeout: 300,
	    poll: 1000
    },
    module: {
        rules: [{
                test: /\.js$/,
                loader: 'babel-loader',
                exclude: /node_modules/
            },
            {
                test: /\.(sa|sc|c)ss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    "css-loader",
                    "sass-loader"
                ]
            },
            {
                test: /\.(eot|woff|ttf|woff2)$/,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]',
                        outputPath: 'fonts/'
                    }
                }]                
            },
            {
                test: /\.(jpe?g|png|gif|svg|ico|webmanifest|xml)$/,
                use: [{
                    loader: 'file-loader',
                    options: {
                        name: '[name].[ext]',
                        outputPath: 'img/'
                    }
                }]                
            }
        ]
    }
}
