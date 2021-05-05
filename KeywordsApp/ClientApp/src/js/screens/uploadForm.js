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
