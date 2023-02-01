const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

let currentlevelLoading = 0;

hubConnection.on("Load", function (message, time, senderNickName, activeProfileEmail, senderEmail, levelLoading) {
    chatroom.append(CreateMessageElem(message, time, senderNickName, activeProfileEmail, senderEmail));
    currentlevelLoading = levelLoading;
});

hubConnection.on("Send", function (message, time, senderNickName, activeProfileEmail, senderEmail) {
    chatroom.prepend(CreateMessageElem(message, time, senderNickName, activeProfileEmail, senderEmail));
    updateScroll();
});

function CreateMessageElem(message, time, senderNickName, activeProfileEmail, senderEmail) {
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

    return messageElem;
}

hubConnection.on("Clear", function () {
    document.getElementById("chatroom").innerText = "";
});

hubConnection.on("SendDate", function (date) {
    let dataElem = document.createElement("h6");
    dataElem.setAttribute("class", "date");

    dataElem.appendChild(document.createTextNode(date));

    chatroom.prepend(dataElem);
});

hubConnection.on("LoadDate", function (date) {
    let dataElem = document.createElement("h6");
    dataElem.setAttribute("class", "date");

    dataElem.appendChild(document.createTextNode(date));

    chatroom.append(dataElem);
});

hubConnection.start()
    .then(() => hubConnection.invoke("Load"));