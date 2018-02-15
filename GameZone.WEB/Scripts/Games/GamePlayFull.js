
var formTitle = _Title;

//Authentication Handler
function validateSubscription(AppUserId) {
    var retVal = false;
    $.ajax({
        type: "POST",
        data: { "UID": AppUserId, "svcName": svcName },
        url: "/Account/ValidSubscription",
        async: false,
        success: function (data) {
            retVal = data;
        }, error: function (data) {
        }
    });
    return retVal;
};

//Authentication Handler
function authHandler(retURL) {
    var userOBJ = localStorage.getItem("UID");
    var userLoginToken = null;
    if (userOBJ) {
        var userData = JSON.parse(localStorage.getItem("UID"));
        userLoginToken = userData.AppUserId + ";" + userData.userLoginToken;

        $.ajax({
            type: "POST",
            data: { "redirectURL": retURL },
            url: "/Account/AuthUserToken",
            headers: {
                "authToken": userLoginToken
            },
            async: false,
            success: function (data) {
                if (data != "") {
                    if (data.includes(";")) {
                            window.location = data.split(';')[0];
                    } else {
                        if (validateSubscription(userData.AppUserId) == "False") {
                            $.notify("Please Subscription to play our games.", 'error');
                            localStorage.removeItem("selectedGame");
                            setTimeout(function () {
                                window.location = "/Home/Index";
                            }, 5000);
                        }
                    }
                } else {
                        window.location = "/Home/Index";
                }
            }, error: function (data) {
            }
        });
    } else {
            window.location = "/Home/Index";
    }
};

//Authenticate
authHandler("/games/gameplayfull");

function InstantiateGame() {
    var gameData = localStorage.getItem("selectedGame");
    if (gameData) {
        gameData = JSON.parse(gameData);
        //loadGameContent(gameData.URL);
        $("#gamePlay").attr("src", gameData.URL);
    } else {
        window.location = "/Home/Index";
    }
};
InstantiateGame();
$("#gamePlay").fullScreen(true);

function loadGameContent() {
    $.ajax({
        type: "GET",
        url: gameURL,
        async: false,
        success: function (data) {
            //$("html head").append("<meta name='viewport' content='width=device-width, initial-scale=1'>");
            //console.log(data);
        }, error: function (data) {
        }
    });
}
loadGameContent();