/******/ (() => { // webpackBootstrap
/******/ 	var __webpack_modules__ = ({

/***/ "./src/js/screens/uploadForm.js":
/*!**************************************!*\
  !*** ./src/js/screens/uploadForm.js ***!
  \**************************************/
/***/ (() => {

class UploadForm {
  MAX_UPLOAD_SIZE = 10000; // 10kb
  form = $('.csv-form');
  initForm = () => {
    this.form.change(() => {
      const errorMsgElt = this.form.find('.csv-form-error-msg');
      const fileNameElt = this.form.find('.csv-form-file-name');
      const csvFileInput = this.form.find('.csv-form-file-input');
      const uploadedMsgElt = this.form.find('.csv-form-uploaded-msg');
      // Reset texts
      errorMsgElt.fadeOut();
      uploadedMsgElt.fadeOut();
      errorMsgElt.html('');
      fileNameElt.html('');
      fileNameElt.removeClass('text-danger');

      if (csvFileInput[0].files.length > 0) {
        const file = csvFileInput[0].files[0];

        // Limit to less than 1mb
        if (file.size > this.MAX_UPLOAD_SIZE) {
          // File too big.
          errorMsgElt.html(
            `Please select a file under ${this.MAX_UPLOAD_SIZE / 10000}Kb.`
          );
          errorMsgElt.fadeIn(250);
          fileNameElt.addClass('text-danger');
        }
        // Ensure extension is '.csv'
        else if (file.name.match(/\.[0-9a-z]+$/i)[0] != '.csv') {
          // Wrong file extension
          errorMsgElt.html(`Please select a .csv file.`);
          errorMsgElt.fadeIn(250);
          fileNameElt.addClass('text-danger');
        } else {
          uploadedMsgElt.fadeIn(250);
          // Submit form
          this.submitForm();
        }
        fileNameElt.html(file.name);
      }
    });
  };

  submitForm = () => {
    const data = new FormData(this.form[0]);
    $.ajax({
      type: 'POST',
      enctype: 'multipart/form-data',
      url: this.form.attr('action'),
      data: data,
      processData: false,
      contentType: false,
      cache: false,
      timeout: 600000,
      success: function (data) {
        $('#uploadFormContainer').html(data);
        new UploadForm().initForm();
      },
      error: function (e) {
        console.log('ERROR : ', e);
      },
    });
  };
}
$(function () {
  new UploadForm().initForm();
});


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
var __webpack_exports__ = {};
// This entry need to be wrapped in an IIFE because it need to be in strict mode.
(() => {
"use strict";
/*!************************!*\
  !*** ./src/js/site.js ***!
  \************************/
__webpack_require__.r(__webpack_exports__);
/* harmony import */ var _js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../js/screens/uploadForm.js */ "./src/js/screens/uploadForm.js");
/* harmony import */ var _js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_0__);


})();

/******/ })()
;
//# sourceMappingURL=site.js.map