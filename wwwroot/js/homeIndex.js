let number = 0;

messengersData.forEach(messenger => {
    number++;
    let div = document.createElement("div");
    div.setAttribute("class", "messenger");
    div.setAttribute("id", "messenger-" + number);

    div.addEventListener("click", function (e) {
        let number = e.currentTarget.id.split('-')[1];
        let messengerId = document.getElementById("messengerId-" + number).value;
        let messengerType = document.getElementById("messengerType-" + number).value;
        hubConnection.invoke("OpenMessenger", messengerId, messengerType);
    });

    let inputId = document.createElement("input");
    inputId.setAttribute("id", "messengerId-" + number);
    inputId.setAttribute("type", "hidden");
    inputId.setAttribute("value", messenger.Id);

    let inputType = document.createElement("input");
    inputType.setAttribute("id", "messengerType-" + number);
    inputType.setAttribute("type", "hidden");
    inputType.setAttribute("value", messenger.Type);

    let name = document.createElement("h3");
    name.setAttribute("id", "openMessenger");
    name.appendChild(document.createTextNode(messenger.Name));

    div.appendChild(inputId);
    div.appendChild(inputType);
    div.appendChild(name);

    var messengersDiv = document.getElementById("messengers").firstChild;
    document.getElementById("messengers").insertBefore(div, messengersDiv);
});

var scrolled = false;
let chatroom = document.getElementById("chatroom");

function updateScroll() {
    if (!scrolled) {
        chatroom.scrollTop = chatroom.scrollHeight;
    }
}

let localLevelLoading = 1;

$(chatroom).scroll(function () {
    if (localLevelLoading == currentlevelLoading && chatroom.scrollTop <= (-1 * chatroom.scrollHeight + 2000)) {
        localLevelLoading++;
        hubConnection.invoke("LoadingContent", "" + localLevelLoading);
    }
});