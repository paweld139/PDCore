

// use local storage for messaging. Set message in local storage and clear it right away
// This is a safe way how to communicate with other tabs while not leaving any traces
//
function message_broadcast(message) {
    localStorage.setItem('message', JSON.stringify(message));
    localStorage.removeItem('message');
}


// receive message
//
function message_receive(ev) {
    if (ev.originalEvent.key != 'message') return; // ignore other keys
    var message = JSON.parse(ev.originalEvent.newValue);
    if (!message) return; // ignore empty msg or msg reset

    // here you act on messages.
    // you can send objects like { 'command': 'doit', 'data': 'abcd' }
    if (message.command == 'doit') location.href = message.data;

    // etc.
}