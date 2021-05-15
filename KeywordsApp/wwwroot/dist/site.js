/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./src/js/components/loader-ajax.js":
/*!******************************************!*\
  !*** ./src/js/components/loader-ajax.js ***!
  \******************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"jquery\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n\r\n\r\n/// SectionDataLoader\r\n/// Enable to load section content from ajax query\r\n/// <section data-load-ajax=\"#URL\">#LOADING MSG</section>\r\nclass SectionDataLoader {\r\n  initLoader = () => {\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('section[data-load-ajax]').map((index, elt) => {\r\n      var element = jquery__WEBPACK_IMPORTED_MODULE_0___default()(elt);\r\n      element.load(element.data('load-ajax'), (rsp, status) => {\r\n        if (status == 'error') {\r\n          element.html('<b>Error:</b> Could not load component');\r\n          element.addClass('text-danger');\r\n        }\r\n      });\r\n    });\r\n  };\r\n}\r\njquery__WEBPACK_IMPORTED_MODULE_0___default()(function () {\r\n  new SectionDataLoader().initLoader();\r\n});\r\n\n\n//# sourceURL=webpack://clientapp/./src/js/components/loader-ajax.js?");

/***/ }),

/***/ "./src/js/components/parser-updates.js":
/*!*********************************************!*\
  !*** ./src/js/components/parser-updates.js ***!
  \*********************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"jquery\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n\r\n\r\nvar connection = new signalR.HubConnectionBuilder().withUrl('/parser').build();\r\n\r\nconnection.on(\r\n  'KeywordStatusUpdate',\r\n  function (fileId, percent, keywordId, keywordName, status, errorMsg) {\r\n    // Prepend new keyword in the last parsed keyword UI\r\n    const url = jquery__WEBPACK_IMPORTED_MODULE_0___default()('#parsedKeywordsList').data('single-load');\r\n    const container = jquery__WEBPACK_IMPORTED_MODULE_0___default()('#parsedKeywordsListContainer');\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default().post(url, { keywordId: keywordId }, (data) => {\r\n      const nbrChildren = container.children().length;\r\n      if (nbrChildren == 0) {\r\n        container.html(data);\r\n      } else {\r\n        container.prepend(data);\r\n        if (nbrChildren >= 8) {\r\n          container.children().last().remove();\r\n        }\r\n      }\r\n\r\n      jquery__WEBPACK_IMPORTED_MODULE_0___default()('#parsedKeywordsListContainer div.card')\r\n        .first()\r\n        .effect('highlight', { color: '#78ff96' }, 1000);\r\n    });\r\n    // update file percent progress\r\n    const fileContainer = jquery__WEBPACK_IMPORTED_MODULE_0___default()('#fileCard' + fileId);\r\n    fileContainer.find('.progress-bar').width(percent + '%');\r\n    fileContainer.find('.progress-bar').html(percent + '%');\r\n  }\r\n);\r\n\r\nconnection\r\n  .start()\r\n  .then(function () {\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#signalRBadge').removeClass('badge-light');\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#signalRBadge').addClass('badge-success');\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default()('#signalRBadge').attr('title', 'SignalR connected!');\r\n  })\r\n  .catch(function (err) {\r\n    return console.error(err.toString());\r\n  });\r\n\n\n//# sourceURL=webpack://clientapp/./src/js/components/parser-updates.js?");

/***/ }),

/***/ "./src/js/screens/uploadForm.js":
/*!**************************************!*\
  !*** ./src/js/screens/uploadForm.js ***!
  \**************************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"jquery\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n\r\nclass UploadForm {\r\n  MAX_UPLOAD_SIZE = 10000; // 10kb\r\n  form = jquery__WEBPACK_IMPORTED_MODULE_0___default()('.csv-form');\r\n  initForm = () => {\r\n    this.form.change(() => {\r\n      const errorMsgElt = this.form.find('.csv-form-error-msg');\r\n      const fileNameElt = this.form.find('.csv-form-file-name');\r\n      const csvFileInput = this.form.find('.csv-form-file-input');\r\n      const uploadedMsgElt = this.form.find('.csv-form-uploaded-msg');\r\n      // Reset texts\r\n      errorMsgElt.fadeOut();\r\n      uploadedMsgElt.fadeOut();\r\n      errorMsgElt.html('');\r\n      fileNameElt.html('');\r\n      fileNameElt.removeClass('text-danger');\r\n\r\n      if (csvFileInput[0].files.length > 0) {\r\n        const file = csvFileInput[0].files[0];\r\n\r\n        // Limit to less than 1mb\r\n        if (file.size > this.MAX_UPLOAD_SIZE) {\r\n          // File too big.\r\n          errorMsgElt.html(\r\n            `Please select a file under ${this.MAX_UPLOAD_SIZE / 10000}Kb.`\r\n          );\r\n          errorMsgElt.fadeIn(250);\r\n          fileNameElt.addClass('text-danger');\r\n        }\r\n        // Ensure extension is '.csv'\r\n        else if (file.name.match(/\\.[0-9a-z]+$/i)[0] != '.csv') {\r\n          // Wrong file extension\r\n          errorMsgElt.html(`Please select a .csv file.`);\r\n          errorMsgElt.fadeIn(250);\r\n          fileNameElt.addClass('text-danger');\r\n        } else {\r\n          uploadedMsgElt.fadeIn(250);\r\n          // Submit form\r\n          this.submitForm();\r\n        }\r\n        fileNameElt.html(file.name);\r\n      }\r\n    });\r\n  };\r\n\r\n  submitForm = () => {\r\n    const data = new FormData(this.form[0]);\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default().ajax({\r\n      type: 'POST',\r\n      enctype: 'multipart/form-data',\r\n      url: this.form.attr('action'),\r\n      data: data,\r\n      processData: false,\r\n      contentType: false,\r\n      cache: false,\r\n      timeout: 600000,\r\n      success: function (data) {\r\n        jquery__WEBPACK_IMPORTED_MODULE_0___default()('#uploadFormContainer').html(data);\r\n        const uploadForm = new UploadForm();\r\n        uploadForm.initForm();\r\n        const newFileId = jquery__WEBPACK_IMPORTED_MODULE_0___default()('#previousFileId').val();\r\n        // Load newly created card\r\n        uploadForm.loadNewCard(newFileId);\r\n      },\r\n      error: function (e) {\r\n        console.log('ERROR : ', e);\r\n      },\r\n    });\r\n  };\r\n\r\n  loadNewCard = (newFileId) => {\r\n    const url = jquery__WEBPACK_IMPORTED_MODULE_0___default()('#csvFilesList').data('single-load');\r\n    const container = jquery__WEBPACK_IMPORTED_MODULE_0___default()('#csvFilesListContainer');\r\n    jquery__WEBPACK_IMPORTED_MODULE_0___default().post(url, { fileId: newFileId }, (data) => {\r\n      const nbrChildren = container.children().length;\r\n      if (nbrChildren == 0) {\r\n        container.html(data);\r\n      } else {\r\n        container.prepend(data);\r\n        if (nbrChildren >= 8) {\r\n          container.children().last().remove();\r\n        }\r\n      }\r\n      jquery__WEBPACK_IMPORTED_MODULE_0___default()('#csvFilesListContainer div.card')\r\n        .first()\r\n        .effect('highlight', { color: '#78ff96' }, 1000);\r\n    });\r\n  };\r\n}\r\njquery__WEBPACK_IMPORTED_MODULE_0___default()(function () {\r\n  new UploadForm().initForm();\r\n});\r\n\n\n//# sourceURL=webpack://clientapp/./src/js/screens/uploadForm.js?");

/***/ }),

/***/ "./src/js/site.js":
/*!************************!*\
  !*** ./src/js/site.js ***!
  \************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _js_components_parser_updates_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../js/components/parser-updates.js */ \"./src/js/components/parser-updates.js\");\n/* harmony import */ var _js_components_loader_ajax_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../js/components/loader-ajax.js */ \"./src/js/components/loader-ajax.js\");\n/* harmony import */ var _js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../js/screens/uploadForm.js */ \"./src/js/screens/uploadForm.js\");\n\r\n\r\n\r\n\n\n//# sourceURL=webpack://clientapp/./src/js/site.js?");

/***/ }),

/***/ "jquery":
/*!*************************!*\
  !*** external "jQuery" ***!
  \*************************/
/***/ ((module) => {

module.exports = jQuery;

/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId](module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/compat get default export */
/******/ 	(() => {
/******/ 		// getDefaultExport function for compatibility with non-harmony modules
/******/ 		__webpack_require__.n = (module) => {
/******/ 			var getter = module && module.__esModule ?
/******/ 				() => (module['default']) :
/******/ 				() => (module);
/******/ 			__webpack_require__.d(getter, { a: getter });
/******/ 			return getter;
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/define property getters */
/******/ 	(() => {
/******/ 		// define getter functions for harmony exports
/******/ 		__webpack_require__.d = (exports, definition) => {
/******/ 			for(var key in definition) {
/******/ 				if(__webpack_require__.o(definition, key) && !__webpack_require__.o(exports, key)) {
/******/ 					Object.defineProperty(exports, key, { enumerable: true, get: definition[key] });
/******/ 				}
/******/ 			}
/******/ 		};
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/hasOwnProperty shorthand */
/******/ 	(() => {
/******/ 		__webpack_require__.o = (obj, prop) => (Object.prototype.hasOwnProperty.call(obj, prop))
/******/ 	})();
/******/ 	
/******/ 	/* webpack/runtime/make namespace object */
/******/ 	(() => {
/******/ 		// define __esModule on exports
/******/ 		__webpack_require__.r = (exports) => {
/******/ 			if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 				Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 			}
/******/ 			Object.defineProperty(exports, '__esModule', { value: true });
/******/ 		};
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module can't be inlined because the eval devtool is used.
/******/ 	var __webpack_exports__ = __webpack_require__("./src/js/site.js");
/******/ 	
/******/ })()
;