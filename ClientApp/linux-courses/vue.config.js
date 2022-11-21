const { defineConfig } = require("@vue/cli-service");
module.exports = defineConfig({
  devServer: {
    proxy: {
      "^/api": {
        target: "https://localhost:7005",
        changeOrigin: true,
      },
    },
    open: process.platform === 'darwin',
    host: '0.0.0.0',
    port: 8085,
    https: true,
    hot: false,
  },
  transpileDependencies: true,
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