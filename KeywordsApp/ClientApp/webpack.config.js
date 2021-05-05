const path = require('path');

const isDevelopment = process.env.NODE_ENV !== 'production';

console.log('Using NODE_ENV = ' + process.env.NODE_ENV);

module.exports = {
  mode: isDevelopment ? 'development' : 'production',
  entry: {
    site: './src/js/site.js',
    csssite: './src/css/site.js',
  },
  output: {
    filename: isDevelopment ? '[name].js' : '[name].min.js',
    path: path.resolve(__dirname, '..', 'wwwroot', 'dist'),
  },
  devtool: 'source-map',
  optimization: {
    minimize: !isDevelopment,
  },
  module: {
    rules: [
      { test: /\.css$/, use: ['style-loader', 'css-loader'] },
      { test: /\.eot(\?v=\d+\.\d+\.\d+)?$/, use: ['file-loader'] },
      {
        test: /\.(woff|woff2)$/,
        use: [
          {
            loader: 'url-loader',
            options: {
              limit: 5000,
            },
          },
        ],
      },
      {
        test: /\.ttf(\?v=\d+\.\d+\.\d+)?$/,
        use: [
          {
            loader: 'url-loader',
            options: {
              limit: 10000,
              mimetype: 'application/octet-stream',
            },
          },
        ],
      },
      {
        test: /\.svg(\?v=\d+\.\d+\.\d+)?$/,
        use: [
          {
            loader: 'url-loader',
            options: {
              limit: 10000,
              mimetype: 'image/svg+xml',
            },
          },
        ],
      },
    ],
  },
};
