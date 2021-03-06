class UploadForm {
  MAX_UPLOAD_SIZE = 10000; // 10kb

  constructor(form) {
    if (form.length > 0 && form.prop('nodeName') === 'FORM') {
      this.form = form;

      this.errorMsgElt = this.form.find('.csv-form-error-msg');
      this.fileNameElt = this.form.find('.csv-form-file-name');
      this.csvFileInput = this.form.find('.csv-form-file-input');
      this.uploadedMsgElt = this.form.find('.csv-form-uploaded-msg');

      form.on('change', this.onFormChange);
    } else {
      console.error('UploadForm needs a <form> object to work properly.');
    }
  }

  // Reset info & errors
  resetText = () => {
    this.errorMsgElt.fadeOut();
    this.uploadedMsgElt.fadeOut();
    this.errorMsgElt.html('');
    this.fileNameElt.html('');
    this.fileNameElt.removeClass('text-danger');
  };

  // 'change' form event handler
  onFormChange = () => {
    this.resetText();

    // Perform client side Validations
    const isFileValid = this.validateFile();

    if (isFileValid) {
      // Show loading msg
      this.uploadedMsgElt.fadeIn(250);
      // Submit Ajax form
      this.submitAjax();
    }
  };

  validateFile = () => {
    if (this.csvFileInput[0].files.length <= 0) return false;

    const file = this.csvFileInput[0].files[0];

    // Limit to less than 1mb
    if (file.size > this.MAX_UPLOAD_SIZE) {
      // File too big.
      this.showError(
        `Please select a file under ${this.MAX_UPLOAD_SIZE / 10000}Kb.`
      );
      return false;
    }
    // Ensure extension is '.csv'
    if (file.name.match(/\.[0-9a-z]+$/i)[0] != '.csv') {
      // Wrong file extension
      this.showError(`Please select a .csv file.`);
      return false;
    }
    this.fileNameElt.html(file.name);
    return true;
  };

  showError = (errMsg) => {
    this.errorMsgElt.html(errMsg);
    this.errorMsgElt.fadeIn(250);
    this.fileNameElt.addClass('text-danger');
  };

  submitAjax = () => {
    const data = new FormData(this.form[0]);
    const thisForm = this.form;
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
        const container = thisForm.parent();
        container.html(data);
        const newForm = container.children('form');
        const uploadForm = new UploadForm(newForm);

        // Load newly created card
        uploadForm.loadNewCard();
      },
      error: function (e) {
        console.log('ERROR : ', e);
      },
    });
  };

  loadNewCard = () => {
    // only if no error msg
    const newFileId = $('#previousFileId').val();
    if (newFileId === '0') {
      return;
    }
    const url = $('#csvFilesList').data('single-load');
    const container = $('#csvFilesListContainer');
    $.post(url, { fileId: newFileId }, (data) => {
      const nbrChildren = container.children().length;
      if (nbrChildren == 0) {
        container.html(data);
      } else {
        container.prepend(data);
        if (nbrChildren >= 8) {
          container.children().last().remove();
        }
      }
      $('#csvFilesListContainer div.card')
        .first()
        .effect('highlight', { color: '#78ff96' }, 1000);
    });
  };
}

// Init at page loaded time
$(function () {
  const form = $('.csv-form');
  new UploadForm(form);
});
