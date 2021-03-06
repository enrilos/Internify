"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var messagesList = document.getElementById("messagesList");

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    messagesList.prepend(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.

    let senderParagraph = document.createElement('p');
    let strongElement = document.createElement('strong');
    let spanElement = document.createElement('span');

    strongElement.textContent = user;
    spanElement.textContent = ` at ${new Date().toLocaleString()}`;

    senderParagraph.appendChild(strongElement);
    senderParagraph.appendChild(spanElement);

    senderParagraph.classList.add('text-left');

    let messageParagraph = document.createElement('p');

    messageParagraph.textContent = message;
    messageParagraph.classList.add('text-left');

    li.appendChild(senderParagraph);
    li.appendChild(messageParagraph);

    li.classList.add('message-list-element');

    if (messagesList.children.length > 100) {
        messagesList.removeChild(messagesList.lastElementChild);
    }
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (e) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;

    if (user && message) {
        connection
            .invoke("SendMessage", user, message)
            .then(() => {
                document.getElementById("messageInput").value = "";
            })
            .catch(function (err) {
            return console.error(err.toString());
            });
    }

    e.preventDefault();
});