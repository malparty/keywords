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
        uploadedMsgElt.show(250);
        // Submit form
        submitForm(form);
      }
      fileNameElt.html(file.name);
    }
  });

  const submitForm = (form) => {
    var data = new FormData(form[0]);
    $.ajax({
      type: 'POST',
      enctype: 'multipart/form-data',
      url: form.attr('action'),
      data: data,
      processData: false,
      contentType: false,
      cache: false,
      timeout: 600000,
      success: function (data) {
        console.log('SUCCESS : ', data);
      },
      error: function (e) {
        console.log('ERROR : ', e);
      },
    });
  };
});
