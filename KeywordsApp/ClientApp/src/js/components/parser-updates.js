const signalR = require('@microsoft/signalr');

var connection = new signalR.HubConnectionBuilder().withUrl('/parser').build();

connection.on(
  'KeywordStatusUpdate',
  function (fileId, percent, keywordId, keywordName, status, errorMsg) {
    let effectColor = '#78ff96';
    if (errorMsg.length > 0) {
      console.warn(`Failed to parse keyword ${keywordName}`);
      console.warn(errorMsg);
      effectColor = '#FF4C0C';
    }
    // Prepend new keyword in the last parsed keyword UI
    const url = $('#parsedKeywordsList').data('single-load');
    const container = $('#parsedKeywordsListContainer');
    $.post(url, { keywordId: keywordId }, (data) => {
      const nbrChildren = container.children().length;
      if (nbrChildren == 0) {
        container.html(data);
      } else {
        container.prepend(data);
        if (nbrChildren >= 8) {
          container.children().last().remove();
        }
      }

      $('#parsedKeywordsListContainer div.card')
        .first()
        .effect('highlight', { color: effectColor }, 1000);
    });
    // update file percent progress
    const fileContainer = $('#fileCard' + fileId);
    fileContainer.find('.progress-bar').width(percent + '%');
    fileContainer.find('.progress-bar').html(percent + '%');
  }
);

connection
  .start()
  .then(function () {
    $('#signalRBadge').removeClass('badge-light');
    $('#signalRBadge').addClass('badge-success');
    $('#signalRBadge').attr('title', 'SignalR connected!');
  })
  .catch(function (err) {
    return console.error(err.toString());
  });
