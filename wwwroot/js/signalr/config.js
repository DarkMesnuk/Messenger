﻿const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

hubConnection.on("Load", function (message, time, senderNickName, activeProfileEmail, senderEmail) {
    let messageElem = document.createElement("p");
    let timeElem = document.createElement("p");
    timeElem.setAttribute("class", "time");
    timeElem.appendChild(document.createTextNode("\n" + time));

    if (activeProfileEmail == senderEmail) {
        messageElem.setAttribute("class", "sender");
    }
    else {
        let nickNameElem = document.createElement("h6");
        nickNameElem.appendChild(document.createTextNode(senderNickName));
        nickNameElem.setAttribute("class", "recipienti");

        messageElem.appendChild(nickNameElem);
        messageElem.setAttribute("class", "recipienti");
    }

    messageElem.appendChild(document.createTextNode(message));
    messageElem.appendChild(timeElem);


    var firstElem = document.getElementById("chatroom");
    firstElem.append(messageElem);
    updateScroll();
});

hubConnection.on("Clear", function () {
    document.getElementById("chatroom").innerText = "";
});

hubConnection.on("Send", function (message, time, senderNickName, activeProfileEmail, senderEmail) {
    let messageElem = document.createElement("p");
    let timeElem = document.createElement("p");
    timeElem.setAttribute("class", "time");
    timeElem.appendChild(document.createTextNode("\n" + time));

    if (activeProfileEmail == senderEmail) {
        messageElem.setAttribute("class", "sender");
    }
    else {
        let nickNameElem = document.createElement("h6");
        nickNameElem.appendChild(document.createTextNode(senderNickName));
        nickNameElem.setAttribute("class", "recipienti");

        messageElem.appendChild(nickNameElem);
        messageElem.setAttribute("class", "recipienti");
    }

    messageElem.appendChild(document.createTextNode(message));
    messageElem.appendChild(timeElem);


    var firstElem = document.getElementById("chatroom");
    firstElem.prepend(messageElem);
    updateScroll();
});

hubConnection.on("SendDataDay", function (day, month, year, yearSee) {
    let dataElem = document.createElement("h6");
    dataElem.setAttribute("class", "sender");
    var data = day + month;

    if (yearSee) {
        data += year
    }

    dataElem.appendChild(document.createTextNode(data));

    var firstElem = document.getElementById("chatroom");
    firstElem.prepend(dataElem);
    updateScroll();
});

document.getElementById("sendContent").addEventListener("click", function (e) {
    let message = document.getElementById("message").value;
    document.getElementById("message").value = "";
    hubConnection.invoke("SendMessage", message);
});

hubConnection.start()
    .then(() => hubConnection.invoke("Load"));