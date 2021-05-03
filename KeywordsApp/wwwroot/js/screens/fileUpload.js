$(function () {
  // init toasts
  var toastElList = [].slice.call(document.querySelectorAll('.toast'));
  const toastList = toastElList.map(function (toastEl) {
    return new bootstrap.Toast(toastEl, {
      animation: true,
      autohide: false,
      delay: 5000,
    });
  });

  const MAX_UPLOAD_SIZE = 1; // Mb
  const form = $('.csv-form');
  form.change((e, f) => {
    const errorMsgElt = form.find('.csv-form-error-msg');
    const fileNameElt = form.find('.csv-form-file-name');
    const csvFileInput = form.find('.csv-form-file-input');

    // Reset texts
    errorMsgElt.html('');
    fileNameElt.html('');

    if (csvFileInput[0].files.length > 0) {
      const file = csvFileInput[0].files[0];

      // Limit to less than 1mb
      if (file.size > MAX_UPLOAD_SIZE) {
        // File too big.
        errorMsgElt.html(`Please select a file under ${MAX_UPLOAD_SIZE}Mb.`);
        csvFileInput.addClass('is-invalid');
        // errorMsgElt.show();
        return;
      }
      // Ensure extension is '.csv'
      else if (file.name.match(/\.[0-9a-z]+$/i)[0] != '.csv') {
        // Wrong file extension
        alert('EXTENSION');
        return;
      }
      fileNameElt.html(file.name);
      toastList[0].show();
      // $('#fileUploadToast').show(250);
    }
    // check file extension and type + limit in INPUT  first
    // Send upload
    // Show waiter
    // Implement server side model validation
    // Show error or Success message
  });
});
