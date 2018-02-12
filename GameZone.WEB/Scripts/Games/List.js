
var formTitle = _Title;

gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {

    //Make Games Page Menu Active
    $("#menuUL li").removeClass("current");
    $("#gameMenu").addClass("current");

    $scope.basicObj = {};
    $scope.Game = {};
    $scope.CategoryGameList = [];


    //Get ApplicationUser Data from DB
    $scope.getGameData = function (selectedCat) {
        //Show Loading Gif
        $("body").find('.game-loader').addClass('show').removeClass('hide');
        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=2000",
            async: true,
            success: function (data) {
                //Empty Div
                $("#isotopeContainer").empty();
                $("body").find('.game-loader').addClass('hide').removeClass('show');
                $.each(data.Data, function (i, rec) {
                    if (rec.title != "What's My Icon?") {
                        gameContent = "<div class='col-sm-2 col-xs-6 isotopeSelector block " + selectedCat + "'>";
                        gameContent = gameContent + "<div class='service-wrap hovereffect panel clearfix animate' data-animate='bounceIn' data-duration='1.0s' data-delay='0.2s'>";

                        gameContent = gameContent + "<a href='" + rec.url + "' class='game-link'>";

                        gameContent = gameContent + "<h3 class='game-title hiddenPara'>" + rec.title + "</h3>";
                        gameContent = gameContent + "<p class='game-category hiddenPara'>" + selectedCat + "</p>";
                        gameContent = gameContent + "<div class='longDescription hiddenPara'>" + rec.long_description + "</div>";

                        gameContent = gameContent + "<div class='lazy'>";
                        gameContent = gameContent + "<img data-original='" + rec.banner_medium + "' alt='" + rec.title + "' class='img-responsive' width='100%' height='186.45' max-width='294.98' max-height='187.7'/>";
                        gameContent = gameContent + "</div><div class='game-description'>";
                        gameContent = gameContent + "<h3 class='game-title text-center'>" + rec.title + "</h3>";
                        gameContent = gameContent + "<a class='pull-right btn bitsumishi game-link' href='" + rec.url + "'>play";
                        gameContent = gameContent + "<p class='game-category hiddenPara'>" + selectedCat + "</p>";
                        gameContent = gameContent + "<div class='longDescription hiddenPara'>" + rec.long_description + "</div>";
                        gameContent = gameContent + "<h3 class='game-title hiddenPara'>" + rec.title + "</h3></a>";

                        gameContent = gameContent + "<small class='game-category pull-left text-muted '>" + selectedCat + "</small>";

                        gameContent = gameContent + "</div></a></div></div>";
                        $("#isotopeContainer").append(gameContent);
                        //gameContent = "";
                        $('body').find('.lazy .img-responsive').lazyload({});
                    }
                });
            },
            error: function (data) {
                $.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };

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
        var returnURL = null;
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
                            $.notify("Access Denied", 'error');
                            setTimeout(function () {
                                window.location = data.split(';')[0];
                            }, 5000);
                        } else {
                            if ($scope.validateSubscription(userData.AppUserId) == "False") {
                                $.notify("Subscription Expired", 'error');
                                localStorage.removeItem("selectedGame");
                                setTimeout(function () {
                                    window.location = "/Home/Index";
                                }, 5000);
                            } else {
                                returnURL = data;
                            }
                        }
                    } else {
                        $.notify("Access Denied", 'error');
                        setTimeout(function () {
                            window.location = "/Home/Index";
                        }, 5000);
                    }
                }, error: function (data) {
                }
            });
        } else {
            $.notify("Access Denied", 'error');
            setTimeout(function () {
                window.location = "/Home/Index";
            }, 5000);
        }
        return returnURL;
    };

    //Populate Page with Games
    $scope.getGameData("family");

    //Function to Log User Out
    $scope.LogUserOut = function () {
        $.post("/Account/LogOff").success(function (data) {
            if (data != "") {
                window.location = data;
            }
        }).error(function (data) {
            $.notify(data.statusText, 'error');
        });
    };
    //Logout Handler
    $(document).on("click", "#logout-target", function (event) {
        $scope.LogUserOut();
    });

    //CLick handler of Menu Items
    $(".gameMenu").click(function (e) {
        var selCat = $(this).attr("id");
        $scope.getGameData(selCat);
    });

    //Game CLick Event Handler
    $(document).on("click", "a.game-link", function (e) {
        e.preventDefault();
        e.preventDefault();

        //Authentication
        var retVal = $scope.authHandler("/games/gameplay");
        if (retVal != null) {
            var selGameURL = $(this).attr("href");
            var selGameLongDesc = $(this).find('div.longDescription').html();
            var selGameCat = $(this).find('p.game-category').text();
            var selGameTitle = $(this).find('h3.game-title').text();

            var selectedGame = {
                "URL": selGameURL,
                "Category": selGameCat,
                "Title": selGameTitle,
                "LongDescription": selGameLongDesc
            };
            localStorage.setItem("selectedGame", JSON.stringify(selectedGame));
            window.location = retVal;
        }
    });


});

function GoPlay(e) {
    //alert("YES");

}