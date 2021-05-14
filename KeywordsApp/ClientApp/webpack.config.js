const path = require('path');

const isDevelopment = process.env.NODE_ENV !== 'production';

console.log('Using NODE_ENV = ' + process.env.NODE_ENV);

module.exports = {
  mode: isDevelopment ? 'development' : 'production',
  entry: {
    site: './src/js/site.js',
    vendor: './src/js/vendor.js',
    vendorcss: './src/css/vendor.scss',
    sitecss: './src/css/site.scss',
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
      {
        test: /\.(scss)$/,
        use: [
          {
            loader: 'style-loader', // inject CSS to page
          },
          {
            loader: 'css-loader', // translates CSS into CommonJS modules
          },
          {
            loader: 'postcss-loader', // Run post css actions
            options: {
              postcssOptions: {
                plugins: function () {
                  // post css plugins, can be exported to postcss.config.js
                  return [require('precss'), require('autoprefixer')];
                },
              },
            },
          },
          {
            loader: 'sass-loader', // compiles Sass to CSS
          },
        ],
      },
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
