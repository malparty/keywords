'use strict';

var connection = new signalR.HubConnectionBuilder().withUrl('/parser').build();

connection.on('KeywordStatusUpdate', function (user, message) {
  console.log('YEAAAAH!!!');
  console.log(user);
  console.log(message);
  var msg = message
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;');
  var encodedMsg = user + ' KeywordStatusUpdate ' + msg;
  alert(encodedMsg);
});

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
