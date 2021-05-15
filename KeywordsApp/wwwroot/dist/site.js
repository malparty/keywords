/*
 * ATTENTION: An "eval-source-map" devtool has been used.
 * This devtool is neither made for production nor for readable output files.
 * It uses "eval()" calls to create a separate source file with attached SourceMaps in the browser devtools.
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
/***/ (() => {

eval("/// SectionDataLoader\r\n/// Enable to load section content from ajax query\r\n/// <section data-load-ajax=\"#URL\">#LOADING MSG</section>\r\nclass SectionDataLoader {\r\n  initLoader = () => {\r\n    $('section[data-load-ajax]').map((index, elt) => {\r\n      var element = $(elt);\r\n      element.load(element.data('load-ajax'), (rsp, status) => {\r\n        if (status == 'error') {\r\n          element.html('<b>Error:</b> Could not load component');\r\n          element.addClass('text-danger');\r\n        }\r\n      });\r\n    });\r\n  };\r\n}\r\n$(function () {\r\n  new SectionDataLoader().initLoader();\r\n});\r\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9jbGllbnRhcHAvLi9zcmMvanMvY29tcG9uZW50cy9sb2FkZXItYWpheC5qcz84Zjk3Il0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLE9BQU87QUFDUCxLQUFLO0FBQ0w7QUFDQTtBQUNBO0FBQ0E7QUFDQSxDQUFDIiwiZmlsZSI6Ii4vc3JjL2pzL2NvbXBvbmVudHMvbG9hZGVyLWFqYXguanMuanMiLCJzb3VyY2VzQ29udGVudCI6WyIvLy8gU2VjdGlvbkRhdGFMb2FkZXJcclxuLy8vIEVuYWJsZSB0byBsb2FkIHNlY3Rpb24gY29udGVudCBmcm9tIGFqYXggcXVlcnlcclxuLy8vIDxzZWN0aW9uIGRhdGEtbG9hZC1hamF4PVwiI1VSTFwiPiNMT0FESU5HIE1TRzwvc2VjdGlvbj5cclxuY2xhc3MgU2VjdGlvbkRhdGFMb2FkZXIge1xyXG4gIGluaXRMb2FkZXIgPSAoKSA9PiB7XHJcbiAgICAkKCdzZWN0aW9uW2RhdGEtbG9hZC1hamF4XScpLm1hcCgoaW5kZXgsIGVsdCkgPT4ge1xyXG4gICAgICB2YXIgZWxlbWVudCA9ICQoZWx0KTtcclxuICAgICAgZWxlbWVudC5sb2FkKGVsZW1lbnQuZGF0YSgnbG9hZC1hamF4JyksIChyc3AsIHN0YXR1cykgPT4ge1xyXG4gICAgICAgIGlmIChzdGF0dXMgPT0gJ2Vycm9yJykge1xyXG4gICAgICAgICAgZWxlbWVudC5odG1sKCc8Yj5FcnJvcjo8L2I+IENvdWxkIG5vdCBsb2FkIGNvbXBvbmVudCcpO1xyXG4gICAgICAgICAgZWxlbWVudC5hZGRDbGFzcygndGV4dC1kYW5nZXInKTtcclxuICAgICAgICB9XHJcbiAgICAgIH0pO1xyXG4gICAgfSk7XHJcbiAgfTtcclxufVxyXG4kKGZ1bmN0aW9uICgpIHtcclxuICBuZXcgU2VjdGlvbkRhdGFMb2FkZXIoKS5pbml0TG9hZGVyKCk7XHJcbn0pO1xyXG4iXSwic291cmNlUm9vdCI6IiJ9\n//# sourceURL=webpack-internal:///./src/js/components/loader-ajax.js\n");

/***/ }),

/***/ "./src/js/components/parser-updates.js":
/*!*********************************************!*\
  !*** ./src/js/components/parser-updates.js ***!
  \*********************************************/
/***/ (() => {

"use strict";
eval("\r\n\r\nvar connection = new signalR.HubConnectionBuilder().withUrl('/parser').build();\r\n\r\nconnection.on(\r\n  'KeywordStatusUpdate',\r\n  function (fileId, percent, keywordId, keywordName, status, errorMsg) {\r\n    // Prepend new keyword in the last parsed keyword UI\r\n    const url = $('#parsedKeywordsList').data('single-load');\r\n    const container = $('#parsedKeywordsListContainer');\r\n    $.post(url, { keywordId: keywordId }, (data) => {\r\n      const nbrChildren = container.children().length;\r\n      if (nbrChildren == 0) {\r\n        container.html(data);\r\n      } else {\r\n        container.prepend(data);\r\n        if (nbrChildren >= 8) {\r\n          container.children().last().remove();\r\n        }\r\n      }\r\n\r\n      $('#parsedKeywordsListContainer div.card')\r\n        .first()\r\n        .effect('highlight', { color: '#78ff96' }, 1000);\r\n    });\r\n    // update file percent progress\r\n    const fileContainer = $('#fileCard' + fileId);\r\n    fileContainer.find('.progress-bar').width(percent + '%');\r\n    fileContainer.find('.progress-bar').html(percent + '%');\r\n  }\r\n);\r\n\r\nconnection\r\n  .start()\r\n  .then(function () {\r\n    $('#signalRBadge').removeClass('badge-light');\r\n    $('#signalRBadge').addClass('badge-success');\r\n    $('#signalRBadge').attr('title', 'SignalR connected!');\r\n  })\r\n  .catch(function (err) {\r\n    return console.error(err.toString());\r\n  });\r\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9jbGllbnRhcHAvLi9zcmMvanMvY29tcG9uZW50cy9wYXJzZXItdXBkYXRlcy5qcz84NGM4Il0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiJBQUFhOztBQUViOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGlCQUFpQix1QkFBdUI7QUFDeEM7QUFDQTtBQUNBO0FBQ0EsT0FBTztBQUNQO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLDhCQUE4QixtQkFBbUI7QUFDakQsS0FBSztBQUNMO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQTtBQUNBLEdBQUciLCJmaWxlIjoiLi9zcmMvanMvY29tcG9uZW50cy9wYXJzZXItdXBkYXRlcy5qcy5qcyIsInNvdXJjZXNDb250ZW50IjpbIid1c2Ugc3RyaWN0JztcclxuXHJcbnZhciBjb25uZWN0aW9uID0gbmV3IHNpZ25hbFIuSHViQ29ubmVjdGlvbkJ1aWxkZXIoKS53aXRoVXJsKCcvcGFyc2VyJykuYnVpbGQoKTtcclxuXHJcbmNvbm5lY3Rpb24ub24oXHJcbiAgJ0tleXdvcmRTdGF0dXNVcGRhdGUnLFxyXG4gIGZ1bmN0aW9uIChmaWxlSWQsIHBlcmNlbnQsIGtleXdvcmRJZCwga2V5d29yZE5hbWUsIHN0YXR1cywgZXJyb3JNc2cpIHtcclxuICAgIC8vIFByZXBlbmQgbmV3IGtleXdvcmQgaW4gdGhlIGxhc3QgcGFyc2VkIGtleXdvcmQgVUlcclxuICAgIGNvbnN0IHVybCA9ICQoJyNwYXJzZWRLZXl3b3Jkc0xpc3QnKS5kYXRhKCdzaW5nbGUtbG9hZCcpO1xyXG4gICAgY29uc3QgY29udGFpbmVyID0gJCgnI3BhcnNlZEtleXdvcmRzTGlzdENvbnRhaW5lcicpO1xyXG4gICAgJC5wb3N0KHVybCwgeyBrZXl3b3JkSWQ6IGtleXdvcmRJZCB9LCAoZGF0YSkgPT4ge1xyXG4gICAgICBjb25zdCBuYnJDaGlsZHJlbiA9IGNvbnRhaW5lci5jaGlsZHJlbigpLmxlbmd0aDtcclxuICAgICAgaWYgKG5ickNoaWxkcmVuID09IDApIHtcclxuICAgICAgICBjb250YWluZXIuaHRtbChkYXRhKTtcclxuICAgICAgfSBlbHNlIHtcclxuICAgICAgICBjb250YWluZXIucHJlcGVuZChkYXRhKTtcclxuICAgICAgICBpZiAobmJyQ2hpbGRyZW4gPj0gOCkge1xyXG4gICAgICAgICAgY29udGFpbmVyLmNoaWxkcmVuKCkubGFzdCgpLnJlbW92ZSgpO1xyXG4gICAgICAgIH1cclxuICAgICAgfVxyXG5cclxuICAgICAgJCgnI3BhcnNlZEtleXdvcmRzTGlzdENvbnRhaW5lciBkaXYuY2FyZCcpXHJcbiAgICAgICAgLmZpcnN0KClcclxuICAgICAgICAuZWZmZWN0KCdoaWdobGlnaHQnLCB7IGNvbG9yOiAnIzc4ZmY5NicgfSwgMTAwMCk7XHJcbiAgICB9KTtcclxuICAgIC8vIHVwZGF0ZSBmaWxlIHBlcmNlbnQgcHJvZ3Jlc3NcclxuICAgIGNvbnN0IGZpbGVDb250YWluZXIgPSAkKCcjZmlsZUNhcmQnICsgZmlsZUlkKTtcclxuICAgIGZpbGVDb250YWluZXIuZmluZCgnLnByb2dyZXNzLWJhcicpLndpZHRoKHBlcmNlbnQgKyAnJScpO1xyXG4gICAgZmlsZUNvbnRhaW5lci5maW5kKCcucHJvZ3Jlc3MtYmFyJykuaHRtbChwZXJjZW50ICsgJyUnKTtcclxuICB9XHJcbik7XHJcblxyXG5jb25uZWN0aW9uXHJcbiAgLnN0YXJ0KClcclxuICAudGhlbihmdW5jdGlvbiAoKSB7XHJcbiAgICAkKCcjc2lnbmFsUkJhZGdlJykucmVtb3ZlQ2xhc3MoJ2JhZGdlLWxpZ2h0Jyk7XHJcbiAgICAkKCcjc2lnbmFsUkJhZGdlJykuYWRkQ2xhc3MoJ2JhZGdlLXN1Y2Nlc3MnKTtcclxuICAgICQoJyNzaWduYWxSQmFkZ2UnKS5hdHRyKCd0aXRsZScsICdTaWduYWxSIGNvbm5lY3RlZCEnKTtcclxuICB9KVxyXG4gIC5jYXRjaChmdW5jdGlvbiAoZXJyKSB7XHJcbiAgICByZXR1cm4gY29uc29sZS5lcnJvcihlcnIudG9TdHJpbmcoKSk7XHJcbiAgfSk7XHJcbiJdLCJzb3VyY2VSb290IjoiIn0=\n//# sourceURL=webpack-internal:///./src/js/components/parser-updates.js\n");

/***/ }),

/***/ "./src/js/screens/uploadForm.js":
/*!**************************************!*\
  !*** ./src/js/screens/uploadForm.js ***!
  \**************************************/
/***/ (() => {

eval("class UploadForm {\r\n  MAX_UPLOAD_SIZE = 10000; // 10kb\r\n  form = $('.csv-form');\r\n  initForm = () => {\r\n    this.form.change(() => {\r\n      const errorMsgElt = this.form.find('.csv-form-error-msg');\r\n      const fileNameElt = this.form.find('.csv-form-file-name');\r\n      const csvFileInput = this.form.find('.csv-form-file-input');\r\n      const uploadedMsgElt = this.form.find('.csv-form-uploaded-msg');\r\n      // Reset texts\r\n      errorMsgElt.fadeOut();\r\n      uploadedMsgElt.fadeOut();\r\n      errorMsgElt.html('');\r\n      fileNameElt.html('');\r\n      fileNameElt.removeClass('text-danger');\r\n\r\n      if (csvFileInput[0].files.length > 0) {\r\n        const file = csvFileInput[0].files[0];\r\n\r\n        // Limit to less than 1mb\r\n        if (file.size > this.MAX_UPLOAD_SIZE) {\r\n          // File too big.\r\n          errorMsgElt.html(\r\n            `Please select a file under ${this.MAX_UPLOAD_SIZE / 10000}Kb.`\r\n          );\r\n          errorMsgElt.fadeIn(250);\r\n          fileNameElt.addClass('text-danger');\r\n        }\r\n        // Ensure extension is '.csv'\r\n        else if (file.name.match(/\\.[0-9a-z]+$/i)[0] != '.csv') {\r\n          // Wrong file extension\r\n          errorMsgElt.html(`Please select a .csv file.`);\r\n          errorMsgElt.fadeIn(250);\r\n          fileNameElt.addClass('text-danger');\r\n        } else {\r\n          uploadedMsgElt.fadeIn(250);\r\n          // Submit form\r\n          this.submitForm();\r\n        }\r\n        fileNameElt.html(file.name);\r\n      }\r\n    });\r\n  };\r\n\r\n  submitForm = () => {\r\n    const data = new FormData(this.form[0]);\r\n    $.ajax({\r\n      type: 'POST',\r\n      enctype: 'multipart/form-data',\r\n      url: this.form.attr('action'),\r\n      data: data,\r\n      processData: false,\r\n      contentType: false,\r\n      cache: false,\r\n      timeout: 600000,\r\n      success: function (data) {\r\n        $('#uploadFormContainer').html(data);\r\n        const uploadForm = new UploadForm();\r\n        uploadForm.initForm();\r\n        const newFileId = $('#previousFileId').val();\r\n        // Load newly created card\r\n        uploadForm.loadNewCard(newFileId);\r\n      },\r\n      error: function (e) {\r\n        console.log('ERROR : ', e);\r\n      },\r\n    });\r\n  };\r\n\r\n  loadNewCard = (newFileId) => {\r\n    const url = $('#csvFilesList').data('single-load');\r\n    const container = $('#csvFilesListContainer');\r\n    $.post(url, { fileId: newFileId }, (data) => {\r\n      const nbrChildren = container.children().length;\r\n      if (nbrChildren == 0) {\r\n        container.html(data);\r\n      } else {\r\n        container.prepend(data);\r\n        if (nbrChildren >= 8) {\r\n          container.children().last().remove();\r\n        }\r\n      }\r\n      $('#csvFilesListContainer div.card')\r\n        .first()\r\n        .effect('highlight', { color: '#78ff96' }, 1000);\r\n    });\r\n  };\r\n}\r\n$(function () {\r\n  new UploadForm().initForm();\r\n});\r\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9jbGllbnRhcHAvLi9zcmMvanMvc2NyZWVucy91cGxvYWRGb3JtLmpzP2NkZDYiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IkFBQUE7QUFDQSwwQkFBMEI7QUFDMUI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDBDQUEwQyw2QkFBNkI7QUFDdkU7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsS0FBSztBQUNMOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLE9BQU87QUFDUDtBQUNBO0FBQ0EsT0FBTztBQUNQLEtBQUs7QUFDTDs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxpQkFBaUIsb0JBQW9CO0FBQ3JDO0FBQ0E7QUFDQTtBQUNBLE9BQU87QUFDUDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDhCQUE4QixtQkFBbUI7QUFDakQsS0FBSztBQUNMO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsQ0FBQyIsImZpbGUiOiIuL3NyYy9qcy9zY3JlZW5zL3VwbG9hZEZvcm0uanMuanMiLCJzb3VyY2VzQ29udGVudCI6WyJjbGFzcyBVcGxvYWRGb3JtIHtcclxuICBNQVhfVVBMT0FEX1NJWkUgPSAxMDAwMDsgLy8gMTBrYlxyXG4gIGZvcm0gPSAkKCcuY3N2LWZvcm0nKTtcclxuICBpbml0Rm9ybSA9ICgpID0+IHtcclxuICAgIHRoaXMuZm9ybS5jaGFuZ2UoKCkgPT4ge1xyXG4gICAgICBjb25zdCBlcnJvck1zZ0VsdCA9IHRoaXMuZm9ybS5maW5kKCcuY3N2LWZvcm0tZXJyb3ItbXNnJyk7XHJcbiAgICAgIGNvbnN0IGZpbGVOYW1lRWx0ID0gdGhpcy5mb3JtLmZpbmQoJy5jc3YtZm9ybS1maWxlLW5hbWUnKTtcclxuICAgICAgY29uc3QgY3N2RmlsZUlucHV0ID0gdGhpcy5mb3JtLmZpbmQoJy5jc3YtZm9ybS1maWxlLWlucHV0Jyk7XHJcbiAgICAgIGNvbnN0IHVwbG9hZGVkTXNnRWx0ID0gdGhpcy5mb3JtLmZpbmQoJy5jc3YtZm9ybS11cGxvYWRlZC1tc2cnKTtcclxuICAgICAgLy8gUmVzZXQgdGV4dHNcclxuICAgICAgZXJyb3JNc2dFbHQuZmFkZU91dCgpO1xyXG4gICAgICB1cGxvYWRlZE1zZ0VsdC5mYWRlT3V0KCk7XHJcbiAgICAgIGVycm9yTXNnRWx0Lmh0bWwoJycpO1xyXG4gICAgICBmaWxlTmFtZUVsdC5odG1sKCcnKTtcclxuICAgICAgZmlsZU5hbWVFbHQucmVtb3ZlQ2xhc3MoJ3RleHQtZGFuZ2VyJyk7XHJcblxyXG4gICAgICBpZiAoY3N2RmlsZUlucHV0WzBdLmZpbGVzLmxlbmd0aCA+IDApIHtcclxuICAgICAgICBjb25zdCBmaWxlID0gY3N2RmlsZUlucHV0WzBdLmZpbGVzWzBdO1xyXG5cclxuICAgICAgICAvLyBMaW1pdCB0byBsZXNzIHRoYW4gMW1iXHJcbiAgICAgICAgaWYgKGZpbGUuc2l6ZSA+IHRoaXMuTUFYX1VQTE9BRF9TSVpFKSB7XHJcbiAgICAgICAgICAvLyBGaWxlIHRvbyBiaWcuXHJcbiAgICAgICAgICBlcnJvck1zZ0VsdC5odG1sKFxyXG4gICAgICAgICAgICBgUGxlYXNlIHNlbGVjdCBhIGZpbGUgdW5kZXIgJHt0aGlzLk1BWF9VUExPQURfU0laRSAvIDEwMDAwfUtiLmBcclxuICAgICAgICAgICk7XHJcbiAgICAgICAgICBlcnJvck1zZ0VsdC5mYWRlSW4oMjUwKTtcclxuICAgICAgICAgIGZpbGVOYW1lRWx0LmFkZENsYXNzKCd0ZXh0LWRhbmdlcicpO1xyXG4gICAgICAgIH1cclxuICAgICAgICAvLyBFbnN1cmUgZXh0ZW5zaW9uIGlzICcuY3N2J1xyXG4gICAgICAgIGVsc2UgaWYgKGZpbGUubmFtZS5tYXRjaCgvXFwuWzAtOWEtel0rJC9pKVswXSAhPSAnLmNzdicpIHtcclxuICAgICAgICAgIC8vIFdyb25nIGZpbGUgZXh0ZW5zaW9uXHJcbiAgICAgICAgICBlcnJvck1zZ0VsdC5odG1sKGBQbGVhc2Ugc2VsZWN0IGEgLmNzdiBmaWxlLmApO1xyXG4gICAgICAgICAgZXJyb3JNc2dFbHQuZmFkZUluKDI1MCk7XHJcbiAgICAgICAgICBmaWxlTmFtZUVsdC5hZGRDbGFzcygndGV4dC1kYW5nZXInKTtcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgdXBsb2FkZWRNc2dFbHQuZmFkZUluKDI1MCk7XHJcbiAgICAgICAgICAvLyBTdWJtaXQgZm9ybVxyXG4gICAgICAgICAgdGhpcy5zdWJtaXRGb3JtKCk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGZpbGVOYW1lRWx0Lmh0bWwoZmlsZS5uYW1lKTtcclxuICAgICAgfVxyXG4gICAgfSk7XHJcbiAgfTtcclxuXHJcbiAgc3VibWl0Rm9ybSA9ICgpID0+IHtcclxuICAgIGNvbnN0IGRhdGEgPSBuZXcgRm9ybURhdGEodGhpcy5mb3JtWzBdKTtcclxuICAgICQuYWpheCh7XHJcbiAgICAgIHR5cGU6ICdQT1NUJyxcclxuICAgICAgZW5jdHlwZTogJ211bHRpcGFydC9mb3JtLWRhdGEnLFxyXG4gICAgICB1cmw6IHRoaXMuZm9ybS5hdHRyKCdhY3Rpb24nKSxcclxuICAgICAgZGF0YTogZGF0YSxcclxuICAgICAgcHJvY2Vzc0RhdGE6IGZhbHNlLFxyXG4gICAgICBjb250ZW50VHlwZTogZmFsc2UsXHJcbiAgICAgIGNhY2hlOiBmYWxzZSxcclxuICAgICAgdGltZW91dDogNjAwMDAwLFxyXG4gICAgICBzdWNjZXNzOiBmdW5jdGlvbiAoZGF0YSkge1xyXG4gICAgICAgICQoJyN1cGxvYWRGb3JtQ29udGFpbmVyJykuaHRtbChkYXRhKTtcclxuICAgICAgICBjb25zdCB1cGxvYWRGb3JtID0gbmV3IFVwbG9hZEZvcm0oKTtcclxuICAgICAgICB1cGxvYWRGb3JtLmluaXRGb3JtKCk7XHJcbiAgICAgICAgY29uc3QgbmV3RmlsZUlkID0gJCgnI3ByZXZpb3VzRmlsZUlkJykudmFsKCk7XHJcbiAgICAgICAgLy8gTG9hZCBuZXdseSBjcmVhdGVkIGNhcmRcclxuICAgICAgICB1cGxvYWRGb3JtLmxvYWROZXdDYXJkKG5ld0ZpbGVJZCk7XHJcbiAgICAgIH0sXHJcbiAgICAgIGVycm9yOiBmdW5jdGlvbiAoZSkge1xyXG4gICAgICAgIGNvbnNvbGUubG9nKCdFUlJPUiA6ICcsIGUpO1xyXG4gICAgICB9LFxyXG4gICAgfSk7XHJcbiAgfTtcclxuXHJcbiAgbG9hZE5ld0NhcmQgPSAobmV3RmlsZUlkKSA9PiB7XHJcbiAgICBjb25zdCB1cmwgPSAkKCcjY3N2RmlsZXNMaXN0JykuZGF0YSgnc2luZ2xlLWxvYWQnKTtcclxuICAgIGNvbnN0IGNvbnRhaW5lciA9ICQoJyNjc3ZGaWxlc0xpc3RDb250YWluZXInKTtcclxuICAgICQucG9zdCh1cmwsIHsgZmlsZUlkOiBuZXdGaWxlSWQgfSwgKGRhdGEpID0+IHtcclxuICAgICAgY29uc3QgbmJyQ2hpbGRyZW4gPSBjb250YWluZXIuY2hpbGRyZW4oKS5sZW5ndGg7XHJcbiAgICAgIGlmIChuYnJDaGlsZHJlbiA9PSAwKSB7XHJcbiAgICAgICAgY29udGFpbmVyLmh0bWwoZGF0YSk7XHJcbiAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgY29udGFpbmVyLnByZXBlbmQoZGF0YSk7XHJcbiAgICAgICAgaWYgKG5ickNoaWxkcmVuID49IDgpIHtcclxuICAgICAgICAgIGNvbnRhaW5lci5jaGlsZHJlbigpLmxhc3QoKS5yZW1vdmUoKTtcclxuICAgICAgICB9XHJcbiAgICAgIH1cclxuICAgICAgJCgnI2NzdkZpbGVzTGlzdENvbnRhaW5lciBkaXYuY2FyZCcpXHJcbiAgICAgICAgLmZpcnN0KClcclxuICAgICAgICAuZWZmZWN0KCdoaWdobGlnaHQnLCB7IGNvbG9yOiAnIzc4ZmY5NicgfSwgMTAwMCk7XHJcbiAgICB9KTtcclxuICB9O1xyXG59XHJcbiQoZnVuY3Rpb24gKCkge1xyXG4gIG5ldyBVcGxvYWRGb3JtKCkuaW5pdEZvcm0oKTtcclxufSk7XHJcbiJdLCJzb3VyY2VSb290IjoiIn0=\n//# sourceURL=webpack-internal:///./src/js/screens/uploadForm.js\n");

/***/ }),

/***/ "./src/js/site.js":
/*!************************!*\
  !*** ./src/js/site.js ***!
  \************************/
/***/ ((__unused_webpack_module, __webpack_exports__, __webpack_require__) => {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _js_components_parser_updates_js__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ../js/components/parser-updates.js */ \"./src/js/components/parser-updates.js\");\n/* harmony import */ var _js_components_parser_updates_js__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(_js_components_parser_updates_js__WEBPACK_IMPORTED_MODULE_0__);\n/* harmony import */ var _js_components_loader_ajax_js__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ../js/components/loader-ajax.js */ \"./src/js/components/loader-ajax.js\");\n/* harmony import */ var _js_components_loader_ajax_js__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(_js_components_loader_ajax_js__WEBPACK_IMPORTED_MODULE_1__);\n/* harmony import */ var _js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ../js/screens/uploadForm.js */ \"./src/js/screens/uploadForm.js\");\n/* harmony import */ var _js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_2___default = /*#__PURE__*/__webpack_require__.n(_js_screens_uploadForm_js__WEBPACK_IMPORTED_MODULE_2__);\n\r\n\r\n\r\n//# sourceURL=[module]\n//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly9jbGllbnRhcHAvLi9zcmMvanMvc2l0ZS5qcz8yOGYwIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7Ozs7QUFBNEM7QUFDSDtBQUNKIiwiZmlsZSI6Ii4vc3JjL2pzL3NpdGUuanMuanMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgJy4uL2pzL2NvbXBvbmVudHMvcGFyc2VyLXVwZGF0ZXMuanMnO1xyXG5pbXBvcnQgJy4uL2pzL2NvbXBvbmVudHMvbG9hZGVyLWFqYXguanMnO1xyXG5pbXBvcnQgJy4uL2pzL3NjcmVlbnMvdXBsb2FkRm9ybS5qcyc7XHJcbiJdLCJzb3VyY2VSb290IjoiIn0=\n//# sourceURL=webpack-internal:///./src/js/site.js\n");

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
/******/ 	// This entry module can't be inlined because the eval-source-map devtool is used.
/******/ 	var __webpack_exports__ = __webpack_require__("./src/js/site.js");
/******/ 	
/******/ })()
;