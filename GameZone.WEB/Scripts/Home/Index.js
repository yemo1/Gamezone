
var formTitle = _Title;

gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {
    //Make Games Page Menu Active
    $("#menuUL li").removeClass("current");
    $("#homeMenu").addClass("current");

    $scope.basicObj = {};
    $scope.basicObj.sT = 0;
    //Get ApplicationUser Data from DB
    $scope.getGameData = function (selectedCat) {
        //Show Loading Gif
        $("body").find('.game-loader').addClass('show').removeClass('hide');

        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=60",
            async: true,
            success: function (data) {
                var gameContent = "";
                //Empty Div
                $("#isotopeContainer").empty();
                $("body").find('.game-loader').addClass('hide').removeClass('show');
                $.each(data.Data, function (i, rec) {
                    if (rec.title != "What's My Icon?") {
                        gameContent = "<div class='col-sm-2 col-xs-6 isotopeSelector block " + selectedCat + "'>";
                        gameContent = gameContent + "<div class='service-wrap hovereffect panel clearfix animate' data-animate='bounceIn' data-duration='1.0s' data-delay='0.2s'>";
                        gameContent = gameContent + "<a href='" + rec.url + "' class='game-link'>";
                        //gameContent = gameContent + "<p class='game-category hiddenPara'>" + selectedCat + "</p>";
                        //gameContent = gameContent + "<div class='longDescription hiddenPara'>" + rec.long_description + "</div>";
                        gameContent = gameContent + "<div class='lazy'>";
                        gameContent = gameContent + "<img data-original='" + rec.banner_medium + "' alt='" + rec.title + "' class='img-responsive' width='100%' height='186.45' max-width='294.98' max-height='187.7'/>";
                        gameContent = gameContent + "</div><div class='game-description'>";
                        gameContent = gameContent + "<a class='pull-right btn  bitsumishi' href'" + rec.url + "'>play</a><h5 class='game-title pull-left'>" + rec.title + "</h5>";
                        //gameContent = gameContent + "<p class='text-justify'>" + rec.short_description + "</p>";
                        gameContent = gameContent + "</div></a></div></div>";
                        $("#isotopeContainer").append(gameContent);
                        //gameContent = "";
                        $('body').find('.lazy .img-responsive').lazyload({});
                    }
                });
            },
            error: function (data) {
                //$.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };
    $scope.getGameData("family");

    //CLick handler of Menu Items
    $(".gameMenu").click(function (e) {
        var selCat = $(this).attr("id");
        $scope.getGameData(selCat);
        e.preventDefault();
        e.preventDefault();
    });

    //Game CLick Event Handler
    $(document).on("click", "a.game-link", function (e) {
        //$("a.game-link").click(function (e) {
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
        sessionStorage.setItem("selectedGame", JSON.stringify(selectedGame));
        e.preventDefault();
        e.preventDefault();
        window.location = "games/gameplay";
    });
    //if ((i + 1) < 3) {
    //    gameImage = gameImage + "<div class='col-md-4'><img class='gameImg' src='" + rec.banner_medium + "' style='margin:10px;'/></div>";
    //}
    //else if ((i + 1) == data.Data.length) {
    //    gameImage = gameImage + "<div class='col-md-4'><img class='gameImg' src='" + rec.banner_medium + "' style='margin:10px;'/></div>";
    //    gameImage = gameImage + "</div>";
    //    $("#gameDiv").append(gameImage);
    //}
    //else if ((i + 1) % 3 != 0) {
    //    gameImage = gameImage + "<div class='col-md-4'><img class='gameImg' src='" + rec.banner_medium + "' style='margin:10px;'/></div>";
    //}
    //else {
    //    gameImage = gameImage + "<div class='col-md-4'><img class='gameImg' src='" + rec.banner_medium + "' style='margin:10px;'/></div>";
    //    gameImage = gameImage + "</div>";
    //    $("#gameDiv").append(gameImage);
    //    gameImage = "<div class='row'>";
    //}

    $scope.SaveNewSubscriber = function () {
        //Enable COntrols
        $(".disabledCtrl").attr("disabled", "disabled");
        $.ajax({
            type: "POST",
            data: $scope.basicObj,
            url: apiURL + "/api/Game?NewSubscriber",
            async: false,
            success: function (data) {
                if (data.Success) {
                    $.notify(data.Message, 'success');
                    $('#exampleModal').modal('hide');
                }

                //if (data.Success) {
                //    $.Notification.notify('success', 'right top', formTitle, data.message);
                //    $scope.GetProductLocations();
                //    //Hide Loading Gif
                //    $scope.processing = 0;
                //    $("#loadingGif").css("display", "none");
                //    //Enable COntrols
                //    $(".disabledCtrl").removeAttr("disabled");
                //    $scope.ProductLocationObj = {};
                //    $('#myModal').modal('hide');
                //} else {
                //    $.Notification.notify('error', 'right top', formTitle, "Error Encountered: " + data.message);
                //    //Hide Loading Gif
                //    $scope.processing = 0;
                //    $("#loadingGif").css("display", "none");
                //    //Enable COntrols
                //    $(".disabledCtrl").removeAttr("disabled");
                //}
                //validator.resetForm();
            }, error: function (data) {
                //$.Notification.notify('error', 'right top', formTitle, "Error Encountered: " + data.statusText);
                ////Hide Loading Gif
                //$scope.processing = 0;
                //$("#loadingGif").css("display", "none");
                ////Enable COntrols
                //$(".disabledCtrl").removeAttr("disabled");
            }
        });
        $(".disabledCtrl").removeAttr("disabled");
    };
});

//Allow Only Numbers into Tel Textboxes
$(document).on("keypress keyup blur", ".allownumericwithoutdecimal", function (event) {
    $(this).val($(this).val().replace(/[^\d].+/, ""));
    if ((event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});