/*
 * ATTENTION: The "eval" devtool has been used (maybe by default in mode: "development").
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file in the browser devtools.
 * If you are trying to read the output file, select a different devtool (https://webpack.js.org/configuration/devtool/)
 * or disable the default devtool with "devtool: false".
 * If you are looking for production-ready output files, see mode: "production" (https://webpack.js.org/configuration/mode/).
 */
/******/ (() => { // webpackBootstrap
/******/ 	var __webpack_modules__ = ({

/***/ "./src/js/components/loader-ajax.js":
/*!******************************************!*\
  !*** ./src/js/components/loader-ajax.js ***!
  \******************************************/
/***/ ((__unused_webpack_module, __unused_webpack_exports, __webpack_require__) => {

eval("/* provided dependency */ var $ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\nclass SectionDataLoader {\r\n  /**\r\n   * SectionDataLoader\r\n   * Enable to load content from ajax query\r\n   * @param  {object} container [Jquery object to receive html content]\r\n   */\r\n  constructor(container) {\r\n    container.map((index, elt) => {\r\n      var element = $(elt);\r\n      element.load(element.data('load-ajax'), (rsp, status) => {\r\n        if (status == 'error') {\r\n          element.html('<b>Error:</b> Could not load component');\r\n          element.addClass('text-danger');\r\n        }\r\n      });\r\n    });\r\n  }\r\n}\r\n$(function () {\r\n  const section = $('section[data-load-ajax]');\r\n  new SectionDataLoader(section);\r\n});\r\n\n\n//# sourceURL=webpack://clientapp/./src/js/components/loader-ajax.js?");

/***/ }),

/***/ "./src/js/components/parser-updates.js":
/*!*********************************************!*\
  !*** ./src/js/components/parser-updates.js ***!
  \*********************************************/
/***/ ((__unused_webpack_module, __unused_webpack_exports, __webpack_require__) => {

eval("/* provided dependency */ var $ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\nconst signalR = __webpack_require__(/*! @microsoft/signalr */ \"./node_modules/@microsoft/signalr/dist/esm/index.js\");\r\n\r\nvar connection = new signalR.HubConnectionBuilder().withUrl('/parser').build();\r\n\r\nconnection.on(\r\n  'KeywordStatusUpdate',\r\n  function (fileId, percent, keywordId, keywordName, status, errorMsg) {\r\n    // Prepend new keyword in the last parsed keyword UI\r\n    const url = $('#parsedKeywordsList').data('single-load');\r\n    const container = $('#parsedKeywordsListContainer');\r\n    $.post(url, { keywordId: keywordId }, (data) => {\r\n      const nbrChildren = container.children().length;\r\n      if (nbrChildren == 0) {\r\n        container.html(data);\r\n      } else {\r\n        container.prepend(data);\r\n        if (nbrChildren >= 8) {\r\n          container.children().last().remove();\r\n        }\r\n      }\r\n\r\n      $('#parsedKeywordsListContainer div.card')\r\n        .first()\r\n        .effect('highlight', { color: '#78ff96' }, 1000);\r\n    });\r\n    // update file percent progress\r\n    const fileContainer = $('#fileCard' + fileId);\r\n    fileContainer.find('.progress-bar').width(percent + '%');\r\n    fileContainer.find('.progress-bar').html(percent + '%');\r\n  }\r\n);\r\n\r\nconnection\r\n  .start()\r\n  .then(function () {\r\n    $('#signalRBadge').removeClass('badge-light');\r\n    $('#signalRBadge').addClass('badge-success');\r\n    $('#signalRBadge').attr('title', 'SignalR connected!');\r\n  })\r\n  .catch(function (err) {\r\n    return console.error(err.toString());\r\n  });\r\n\n\n//# sourceURL=webpack://clientapp/./src/js/components/parser-updates.js?");

/***/ }),

/***/ "./src/js/screens/uploadForm.js":
/*!**************************************!*\
  !*** ./src/js/screens/uploadForm.js ***!
  \**************************************/
/***/ ((__unused_webpack_module, __unused_webpack_exports, __webpack_require__) => {

eval("/* provided dependency */ var $ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\nclass UploadForm {\r\n  MAX_UPLOAD_SIZE = 10000; // 10kb\r\n\r\n  constructor(form) {\r\n    if (form.length > 0 && form.prop('nodeName') === 'FORM') {\r\n      this.form = form;\r\n\r\n      this.errorMsgElt = this.form.find('.csv-form-error-msg');\r\n      this.fileNameElt = this.form.find('.csv-form-file-name');\r\n\r\n      form.on('change', this.onFormChange);\r\n    } else {\r\n      console.error('UploadForm needs a <form> object to work properly.');\r\n    }\r\n  }\r\n\r\n  onFormChange = () => {\r\n    const csvFileInput = this.form.find('.csv-form-file-input');\r\n    const uploadedMsgElt = this.form.find('.csv-form-uploaded-msg');\r\n\r\n    // Reset texts\r\n    this.errorMsgElt.fadeOut();\r\n    uploadedMsgElt.fadeOut();\r\n    this.errorMsgElt.html('');\r\n    this.fileNameElt.html('');\r\n    this.fileNameElt.removeClass('text-danger');\r\n\r\n    if (csvFileInput[0].files.length > 0) {\r\n      const file = csvFileInput[0].files[0];\r\n\r\n      // Limit to less than 1mb\r\n      if (file.size > this.MAX_UPLOAD_SIZE) {\r\n        // File too big.\r\n        this.showError(\r\n          `Please select a file under ${this.MAX_UPLOAD_SIZE / 10000}Kb.`\r\n        );\r\n      }\r\n      // Ensure extension is '.csv'\r\n      else if (file.name.match(/\\.[0-9a-z]+$/i)[0] != '.csv') {\r\n        // Wrong file extension\r\n        this.showError(`Please select a .csv file.`);\r\n      } else {\r\n        uploadedMsgElt.fadeIn(250);\r\n        // Submit Ajax form\r\n        this.submitAjax();\r\n      }\r\n      this.fileNameElt.html(file.name);\r\n    }\r\n  };\r\n\r\n  showError = (errMsg) => {\r\n    this.errorMsgElt.html(errMsg);\r\n    this.errorMsgElt.fadeIn(250);\r\n    this.fileNameElt.addClass('text-danger');\r\n  };\r\n\r\n  submitAjax = () => {\r\n    const data = new FormData(this.form[0]);\r\n    const thisForm = this.form;\r\n    $.ajax({\r\n      type: 'POST',\r\n      enctype: 'multipart/form-data',\r\n      url: this.form.attr('action'),\r\n      data: data,\r\n      processData: false,\r\n      contentType: false,\r\n      cache: false,\r\n      timeout: 600000,\r\n      success: function (data) {\r\n        const container = thisForm.parent();\r\n        container.html(data);\r\n        const newForm = container.children('form');\r\n        const uploadForm = new UploadForm(newForm);\r\n\r\n        // Load newly created card\r\n        uploadForm.loadNewCard();\r\n      },\r\n      error: function (e) {\r\n        console.log('ERROR : ', e);\r\n      },\r\n    });\r\n  };\r\n\r\n  loadNewCard = () => {\r\n    // only if no error msg\r\n    const newFileId = $('#previousFileId').val();\r\n    if (newFileId === '0') {\r\n      return;\r\n    }\r\n    const url = $('#csvFilesList').data('single-load');\r\n    const container = $('#csvFilesListContainer');\r\n    $.post(url, { fileId: newFileId }, (data) => {\r\n      const nbrChildren = container.children().length;\r\n      if (nbrChildren == 0) {\r\n        container.html(data);\r\n      } else {\r\n        container.prepend(data);\r\n        if (nbrChildren >= 8) {\r\n          container.children().last().remove();\r\n        }\r\n      }\r\n      $('#csvFilesListContainer div.card')\r\n        .first()\r\n        .effect('highlight', { color: '#78ff96' }, 1000);\r\n    });\r\n  };\r\n}\r\n$(function () {\r\n  const form = $('.csv-form');\r\n  new UploadForm(form);\r\n});\r\n\n\n//# sourceURL=webpack://clientapp/./src/js/screens/uploadForm.js?");

/***/ }),

/***/ "./src/js/site.js":
/*!************************!*\
  !*** ./src/js/site.js ***!
  \************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! jquery */ \"./node_modules/jquery/dist/jquery.js\");\n/* harmony import */ var jquery__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(jquery__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var _microsoft_signalr__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! @microsoft/signalr */ \"./node_modules/@microsoft/signalr/dist/esm/index.js\");\n/* harmony import */ var jquery_validation__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! jquery-validation */ \"./node_modules/jquery-validation/dist/jquery.validate.js\");\n/* harmony import */ var jquery_validation__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(jquery_validation__WEBPACK_IMPORTED_MODULE_2__);\n/* harmony import */ var jquery_validation_unobtrusive__WEBPACK_IMPORTED_MODULE_3__ = __webpack_require__(/*! jquery-validation-unobtrusive */ \"./node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobtrusive.js\");\n/* harmony import */ var jquery_validation_unobtrusive__WEBPACK_IMPORTED_MODULE_3___default = /*#__PURE__*/__webpack_require__.n(jquery_validation_unobtrusive__WEBPACK_IMPORTED_MODULE_3__);\n/* harmony import */ var jquery_ui_ui_effects_effect_highlight__WEBPACK_IMPORTED_MODULE_4__ = __webpack_require__(/*! jquery-ui/ui/effects/effect-highlight */ \"./node_modules/jquery-ui/ui/effects/effect-highlight.js\");\n/* harmony import */ var jquery_ui_ui_effects_effect_highlight__WEBPACK_IMPORTED_MODULE_4___default = /*#__PURE__*/__webpack_require__.n(jquery_ui_ui_effects_effect_highlight__WEBPACK_IMPORTED_MODULE_4__);\n/* harmony import */ var _js_components_parser_updates_js__WEBPACK_IMPORTED_MODULE_5__ = __webpack_require__(/*! ../js/components/parser-updates.js */ \"./src/js/components/parser-updates.js\");\n/* harmony import */ var _js_components_parser_updates_js__WEBPACK_IMPORTED_MODULE_5___default = /*#__PURE__*/__webpack_require__.n(_js_components_parser_updates_js__WEBPACK_IMPORTED_MODULE_5__);\n/* harmony import */ var _js_components_loader_ajax_js__WEBPACK_IMPORTED_MODULE_6__ = __webpack_require__(/*! ../js/components/loader-ajax.js */ \"./src/js/components/loader-ajax.js\");\n/* harmony import */ var _js_components_loader_ajax_js__WEBPACK_IMPORTED_MODULE_6___default = /*#__PURE__*/__webpack_require__.n(_js_components_loader_ajax_js__WEBPACK_IMPORTED_MODULE_6__);\n/* harmony import */ var _js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_7__ = __webpack_require__(/*! ../js/screens/uploadForm.js */ \"./src/js/screens/uploadForm.js\");\n/* harmony import */ var _js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_7___default = /*#__PURE__*/__webpack_require__.n(_js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_7__);\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\n\n//# sourceURL=webpack://clientapp/./src/js/site.js?");

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
/******/ 		__webpack_modules__[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = __webpack_modules__;
/******/ 	
/************************************************************************/
/******/ 	/* webpack/runtime/chunk loaded */
/******/ 	(() => {
/******/ 		var deferred = [];
/******/ 		__webpack_require__.O = (result, chunkIds, fn, priority) => {
/******/ 			if(chunkIds) {
/******/ 				priority = priority || 0;
/******/ 				for(var i = deferred.length; i > 0 && deferred[i - 1][2] > priority; i--) deferred[i] = deferred[i - 1];
/******/ 				deferred[i] = [chunkIds, fn, priority];
/******/ 				return;
/******/ 			}
/******/ 			var notFulfilled = Infinity;
/******/ 			for (var i = 0; i < deferred.length; i++) {
/******/ 				var [chunkIds, fn, priority] = deferred[i];
/******/ 				var fulfilled = true;
/******/ 				for (var j = 0; j < chunkIds.length; j++) {
/******/ 					if ((priority & 1 === 0 || notFulfilled >= priority) && Object.keys(__webpack_require__.O).every((key) => (__webpack_require__.O[key](chunkIds[j])))) {
/******/ 						chunkIds.splice(j--, 1);
/******/ 					} else {
/******/ 						fulfilled = false;
/******/ 						if(priority < notFulfilled) notFulfilled = priority;
/******/ 					}
/******/ 				}
/******/ 				if(fulfilled) {
/******/ 					deferred.splice(i--, 1)
/******/ 					result = fn();
/******/ 				}
/******/ 			}
/******/ 			return result;
/******/ 		};
/******/ 	})();
/******/ 	
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
/******/ 	/* webpack/runtime/jsonp chunk loading */
/******/ 	(() => {
/******/ 		// no baseURI
/******/ 		
/******/ 		// object to store loaded and loading chunks
/******/ 		// undefined = chunk not loaded, null = chunk preloaded/prefetched
/******/ 		// [resolve, reject, Promise] = chunk loading, 0 = chunk loaded
/******/ 		var installedChunks = {
/******/ 			"sitejs": 0
/******/ 		};
/******/ 		
/******/ 		// no chunk on demand loading
/******/ 		
/******/ 		// no prefetching
/******/ 		
/******/ 		// no preloaded
/******/ 		
/******/ 		// no HMR
/******/ 		
/******/ 		// no HMR manifest
/******/ 		
/******/ 		__webpack_require__.O.j = (chunkId) => (installedChunks[chunkId] === 0);
/******/ 		
/******/ 		// install a JSONP callback for chunk loading
/******/ 		var webpackJsonpCallback = (parentChunkLoadingFunction, data) => {
/******/ 			var [chunkIds, moreModules, runtime] = data;
/******/ 			// add "moreModules" to the modules object,
/******/ 			// then flag all "chunkIds" as loaded and fire callback
/******/ 			var moduleId, chunkId, i = 0;
/******/ 			for(moduleId in moreModules) {
/******/ 				if(__webpack_require__.o(moreModules, moduleId)) {
/******/ 					__webpack_require__.m[moduleId] = moreModules[moduleId];
/******/ 				}
/******/ 			}
/******/ 			if(runtime) var result = runtime(__webpack_require__);
/******/ 			if(parentChunkLoadingFunction) parentChunkLoadingFunction(data);
/******/ 			for(;i < chunkIds.length; i++) {
/******/ 				chunkId = chunkIds[i];
/******/ 				if(__webpack_require__.o(installedChunks, chunkId) && installedChunks[chunkId]) {
/******/ 					installedChunks[chunkId][0]();
/******/ 				}
/******/ 				installedChunks[chunkIds[i]] = 0;
/******/ 			}
/******/ 			return __webpack_require__.O(result);
/******/ 		}
/******/ 		
/******/ 		var chunkLoadingGlobal = self["webpackChunkclientapp"] = self["webpackChunkclientapp"] || [];
/******/ 		chunkLoadingGlobal.forEach(webpackJsonpCallback.bind(null, 0));
/******/ 		chunkLoadingGlobal.push = webpackJsonpCallback.bind(null, chunkLoadingGlobal.push.bind(chunkLoadingGlobal));
/******/ 	})();
/******/ 	
/************************************************************************/
/******/ 	
/******/ 	// startup
/******/ 	// Load entry module and return exports
/******/ 	// This entry module depends on other loaded chunks and execution need to be delayed
/******/ 	var __webpack_exports__ = __webpack_require__.O(undefined, ["vendor"], () => (__webpack_require__("./src/js/site.js")))
/******/ 	__webpack_exports__ = __webpack_require__.O(__webpack_exports__);
/******/ 	
/******/ })()
;