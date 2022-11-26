const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
  devServer: {
    proxy: {
      "^/api": {
        target: "https://localhost:7005",
        changeOrigin: true,
      },
    },
    open: process.platform === "darwin",
    host: "localhost",
    port: 8085,
    https: true,
    hot: false,
  },

  transpileDependencies: true,
  lintOnSave: false,
});

// module.exports += {
//   devServer: {
//     proxy: {
//       "^/api": {
//         target: "http://localhost:7005",
//         changeOrigin: true,
//       },
//     },
//   },
// };
