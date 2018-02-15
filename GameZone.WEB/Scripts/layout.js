//Get User Object
var userOBJ = localStorage.getItem("UID");
var UID = null;
if (userOBJ) {
    UID = JSON.parse(localStorage.getItem("UID"));
    $('#userIDSpan').text(UID.szUsername);
    $('.loginReg').css("display", "none");
    $('.logout').css("display", "block");
} else {
    ResetUsernameToAccount();
}

//Function to Log User Out
function LogUserOut(delay, UID) {
    $.post("/Account/LogOff", { "UID": UID }).success(function (data) {
        localStorage.removeItem("UID");
        localStorage.removeItem("selectedGame");
        if (data != "") {
            setTimeout(function () {
                window.location = data;
            }, delay);
        }
    }).error(function (data) {
        $.notify(data.statusText, 'error');
    });
}

//Logout Handler
$(document).on("click", "#logout-target", function (event) {
    var userOBJ = localStorage.getItem("UID");
    var UID = null;
    if (userOBJ) {
        UID = JSON.parse(localStorage.getItem("UID"));
        LogUserOut(1000, UID.szUsername);
    }
});

//Function to Reset Username Display to Display Account
function ResetUsernameToAccount() {
    $('#userIDSpan').text("Account");
    $('.logout').css("display", "none");
    $('.loginReg').css("display", "block");
}