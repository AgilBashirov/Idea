// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
"use strict";

document.getElementsByClassName("notif-content-icon")[0].addEventListener("click", function (e) {
    e.preventDefault();
    this.nextElementSibling.classList.toggle("show");
});

let adjustScroll = () => {
    document.getElementsByClassName("cont")[0].scrollTop = (parseInt(document.getElementById("message-box").clientHeight) - 300);
}

if (document.getElementsByClassName("chat-page").length > 0) {
    adjustScroll();
}

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
if (document.getElementById("sendButton") != null) {
    document.getElementById("sendButton").disabled = true;
}

connection.on("ReceiveMessage", function (message, date, countUp, userId) {
    //jsbdv
    if (countUp) {
        let oldCount = parseInt(document.getElementsByClassName("badge-danger")[0].innerText);
        if (isNaN(oldCount)) {
            document.getElementsByClassName("badge-danger")[0].innerText = 1
        } else {
            document.getElementsByClassName("badge-danger")[0].innerText = (oldCount + 1)
        }
    }

    let friend = document.getElementById("friend-id").value;
    if (userId == friend) {
        var div = document.createElement("div");
        div.classList.add("message");
        div.classList.add("reciever");

        var p = document.createElement("p");
        p.classList.add("message-text");
        p.innerText = message;

        let time = date.split("T")[1].substring(0, 5);
        let date2 = date.split("T")[0].split("-")[2] + "." + date.split("T")[0].split("-")[1] + "." + date.split("T")[0].split("-")[0];
        let resultDate = date2 + " " + time;
        var span = document.createElement("span");
        span.classList.add("message-date");
        span.innerText = resultDate;

        div.appendChild(p);
        div.appendChild(span);
        document.getElementById("message-box").appendChild(div);

        adjustScroll();
    }
});

connection.on("ChangeActiveStatus", function (isActive, userId) {
    let user = document.getElementById(userId);

    if (isActive) {
        user.classList.add("isActive");
    } else {
        if (user.classList.contains("isActive")) {
            user.classList.remove("isActive");
        }
    }
})

connection.start().then(function () {
    if (document.getElementById("sendButton") != null) {
        document.getElementById("sendButton").disabled = false;

    }
}).catch(function (err) {
    return console.error(err.toString());
});
if (document.getElementById("sendButton") != null) {
    document.getElementById("sendButton").addEventListener("click", function (event) {
        event.preventDefault();

        var friendId = document.getElementById("friend-id").value;
        var message = document.getElementById("message").value;

        if (message != "") {
            connection.invoke("SendMessage", friendId, message).catch(function (err) {
                return console.error(err.toString());
            });

            var div = document.createElement("div");
            div.classList.add("message");
            div.classList.add("sender");

            var p = document.createElement("p");
            p.classList.add("message-text");
            p.innerText = message;

            var span = document.createElement("span");
            span.classList.add("message-date");
            var date = new Date();

            //month
            let month = 0;
            if (date.getMonth() + 1 < 10) {
                month = 0 + "" + (date.getMonth() + 1);
            } else {
                month = date.getMonth() + 1
            }

            //hours
            let hours = 0;
            if (date.getHours() < 10) {
                hours = "0" + date.getHours();
            } else {
                hours = date.getHours();
            }

            //minutes
            let minutes = 0;
            if (date.getMinutes() < 10) {
                minutes = "0" + date.getMinutes();
            } else {
                minutes = date.getMinutes();
            }
            span.innerText = date.getDate() + "." + month + "." + date.getFullYear() + " " + hours + ":" + minutes;

            div.appendChild(p);
            div.appendChild(span);
            document.getElementById("message-box").appendChild(div);

            document.getElementById("message").value = "";
        }


        adjustScroll();



    });

}

var input = document.getElementById("message");
if (input != null) {
    input.addEventListener("keyup", function (event) {
        if (event.keyCode === 13) {

            document.getElementById("sendButton").click();
        }
    });

}

