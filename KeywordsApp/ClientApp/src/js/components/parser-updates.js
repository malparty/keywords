'use strict';

var connection = new signalR.HubConnectionBuilder().withUrl('/parser').build();

connection.on('ReceiveMessage', function (user, message) {
  var msg = message
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;');
  var encodedMsg = user + ' says ' + msg;
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

// document
//   .getElementById('sendButton')
//   .addEventListener('click', function (event) {
//     var user = document.getElementById('userInput').value;
//     var message = document.getElementById('messageInput').value;
//     connection.invoke('SendMessage', user, message).catch(function (err) {
//       return console.error(err.toString());
//     });
//     event.preventDefault();
//   });
