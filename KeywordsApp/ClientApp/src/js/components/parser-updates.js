'use strict';

var connection = new signalR.HubConnectionBuilder().withUrl('/parser').build();

connection.on(
  'KeywordStatusUpdate',
  function (fileId, percent, keywordId, keywordName, status, errorMsg) {
    // Prepend new keyword in the last parsed keyword UI
    const url = $('#parsedKeywordsList').data('single-load');
    const container = $('#parsedKeywordsListContainer');
    $.post(url, { keywordId: keywordId }, (data) => {
      container.children().last().remove();
      container.prepend(data);
      $('#parsedKeywordsListContainer div.card')
        .first()
        .effect('highlight', { color: '#78ff96' }, 1000);
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
