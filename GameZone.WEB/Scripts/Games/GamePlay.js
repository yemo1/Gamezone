gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {
    //Make Games Page Menu Active
    $("#menuUL li").removeClass("current");
    $("#gameMenu").addClass("current");

    $scope.basicObj = {};
    $scope.Game = {};

    //Get ApplicationUser Data from DB
    $scope.getOtherCategoryGames = function (selectedCat) {
        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=5",
            async: false,
            success: function (data) {
                var gameContent = "";
                //Empty Div
                $("#otherCategoryGames").empty();

                $.each(data.Data, function (i, rec) {
                    if (rec.title != "What's My Icon?") {
                        gameContent = "<div class='sp-slide'>";
                        gameContent = gameContent + "<a href='" + rec.url + "' class='OtherCatGame-link'>";
                        gameContent = gameContent + "<p class='game-category hiddenPara'>" + selectedCat + "," + rec.title + "</p>";
                        gameContent = gameContent + "<div class='longDescription hiddenPara'>" + rec.long_description + "</div>";
                        gameContent = gameContent + "<img class='sp-image' src='" + rec.banner_medium + "'data-src='" + rec.banner_medium + "'data-retina='" + rec.banner_medium + "'/>";
                        gameContent = gameContent + "</a>";
                        gameContent = gameContent + "<p class='sp-caption text-center'><label id='game-title' class='dark_c '>" + rec.title + "</label>";
                        gameContent = gameContent + "<br>";
                        gameContent = gameContent + "<label class='text-center small text-muted game-category'>" + selectedCat + "</label>";
                        gameContent = gameContent + "</p></div>";
                        $("#otherCategoryGames").append(gameContent);
                        gameContent = "";
                    }
                });
            },
            error: function (data) {
                //$.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };

    $scope.InstantiateGame = function () {
        var gameData = localStorage.getItem("selectedGame");
        if (gameData) {
            gameData = JSON.parse(gameData);
            gameData.Category = gameData.Category.toUpperCase();
            $scope.Game = gameData;
            $("#gamePlay").attr("src", gameData.URL);
            $("#longDesc").html($.parseHTML(gameData.LongDescription));
            //Load Other Games in same category
            $scope.getOtherCategoryGames(gameData.Category.toLowerCase());
        } else {
            window.location = "/Home/Index";
        }
    };
    $scope.InstantiateGame();

    //Authentication Handler
    $scope.validateSubscription = function (AppUserId) {
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
    $scope.authHandler = function (retURL) {
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
                            if ($scope.validateSubscription(userData.AppUserId) == "False") {
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
    $scope.authHandler("games/gameplay");

    //CLick handler of Menu Items
    $(".gameMenu").click(function (e) {
        var selCat = $(this).attr("id");
        $scope.getGameData(selCat);
    });

    //Game CLick Event Handler
    $(document).on("click", "a.OtherCatGame-link", function (e) {
        e.preventDefault();
        e.preventDefault();

        //Authentication
        var retVal = $scope.authHandler("/games/gameplay");
        if (retVal == null) {
            return;
        }

        var selGameURL = $(this).attr("href");
        var selGameLongDesc = $(this).find('div.longDescription').html();
        var selGameCat = $(this).find('p.game-category').text();
        var selGameTitle = selGameCat.split(',')[1];

        var selectedGame = {
            "URL": selGameURL,
            "Category": selGameCat.split(',')[0],
            "Title": selGameTitle,
            "LongDescription": selGameLongDesc
        };
        localStorage.setItem("selectedGame", JSON.stringify(selectedGame));
        e.preventDefault();
        e.preventDefault();
        window.location = retVal;
    });
});

