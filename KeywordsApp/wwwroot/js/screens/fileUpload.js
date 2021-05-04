$(function () {
  const MAX_UPLOAD_SIZE = 10000; // 10kb
  const form = $('.csv-form');
  form.change((e, f) => {
    const errorMsgElt = form.find('.csv-form-error-msg');
    const fileNameElt = form.find('.csv-form-file-name');
    const csvFileInput = form.find('.csv-form-file-input');
    const uploadedMsgElt = form.find('.csv-form-uploaded-msg');
    // Reset texts
    errorMsgElt.hide();
    uploadedMsgElt.hide();
    errorMsgElt.html('');
    fileNameElt.html('');
    fileNameElt.removeClass('text-danger');

    if (csvFileInput[0].files.length > 0) {
      const file = csvFileInput[0].files[0];

      // Limit to less than 1mb
      if (file.size > MAX_UPLOAD_SIZE) {
        // File too big.
        errorMsgElt.html(
          `Please select a file under ${MAX_UPLOAD_SIZE / 10000}Kb.`
        );
        errorMsgElt.show(250);
        fileNameElt.addClass('text-danger');
      }
      // Ensure extension is '.csv'
      else if (file.name.match(/\.[0-9a-z]+$/i)[0] != '.csv') {
        // Wrong file extension
        errorMsgElt.html(`Please select a .csv file.`);
        errorMsgElt.show(250);
        fileNameElt.addClass('text-danger');
      } else {
        // Run Post request
        uploadedMsgElt.show(250);
      }
      fileNameElt.html(file.name);
    }
    // check file extension and type + limit in INPUT  first
    // Send upload
    // Show waiter
    // Implement server side model validation
    // Show error or Success message
  });
});
