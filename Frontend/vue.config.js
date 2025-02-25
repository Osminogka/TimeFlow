const { defineConfig } = require('@vue/cli-service');

module.exports = defineConfig({
  transpileDependencies: true,
  devServer: {
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true,
        secure: false,
      }
    },
    host: '0.0.0.0',  // Allow access from any device on LAN
    port: 8080,        // Change this if needed
    allowedHosts: 'all' // Allow connections from any host
  }
});
