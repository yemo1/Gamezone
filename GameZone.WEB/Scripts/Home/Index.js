﻿
var formTitle = _Title;

gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {
    //Make Games Page Menu Active
    $("#menuUL li").removeClass("current");
    $("#homeMenu").addClass("current");

    $scope.basicObj = {};
    $scope.registerObj = {};
    $scope.loginObj = {};
    $scope.basicObj.sT = 0;

    // Detect Device Type and Display Appropriate Subscription Modal
   

    //Detect Device Type and Display Appropriate Subscription Modal
    //if (_IsMobile == "False") {
    //    $("#pcSubscriptionModal").modal("show");
    //}

    //Get ApplicationUser Data from DB
    $scope.getGameData = function (selectedCat) {
        //Show Loading Gif
        $("body").find('.game-loader').addClass('show').removeClass('hide');

        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=1000",
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

                        gameContent = gameContent + "<h3 class='game-title hiddenPara'>" + rec.title + "</h3>";
                        gameContent = gameContent + "<p class='game-category hiddenPara'>" + selectedCat + "</p>";
                        gameContent = gameContent + "<div class='longDescription hiddenPara'>" + rec.long_description + "</div>";

                        gameContent = gameContent + "<div class='lazy'>";
                        gameContent = gameContent + "<img data-original='" + rec.banner_medium + "' alt='" + rec.title + "' class='img-responsive' width='100%' height='186.45' max-width='294.98' max-height='187.7'/>";
                        gameContent = gameContent + "</div><div class='game-description'>";
                        
                        gameContent = gameContent + "<h3 class='game-title text-center '>" + rec.title + "</h3>";
                        
                        gameContent = gameContent + "<a class='pull-right btn  bitsumishi game-link' href='" + rec.url + "'>play";
                        gameContent = gameContent + "<p class='game-category hiddenPara'>" + selectedCat + "</p>";
                        gameContent = gameContent + "<div class='longDescription hiddenPara'>" + rec.long_description + "</div>";
                        gameContent = gameContent + "<h3 class='game-title hiddenPara'>" + rec.title + "</h3></a>";

                        gameContent = gameContent + "<small class='game-category pull-left text-muted'>" + selectedCat + "</small>";
                        gameContent = gameContent + "</div></a></div></div>";
                        $("#isotopeContainer").append(gameContent);
                        //gameContent = "";
                        $('body').find('.lazy .img-responsive').lazyload({});
                        $(window).scroll(function () {
                            if ($(window).scrollTop() >= $('#game-area').height()) {
                                $("#pcSubscriptionModal").show();
                            }
                            else {
                                $("#pcSubscriptionModal").hide("");
                            }
                        });
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
   
    $scope.SaveNewSubscriber = function () {

        $scope.basicObj.nO = $('select#nOSelect option:selected').val();

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
                    $('#pcSubscriptionModal').modal('hide');
                }
            }, error: function (data) {
            }
        });
        $(".disabledCtrl").removeAttr("disabled");
    };

    $scope.RegisterNewUser = function () {
        //if (!validateEmail(userName)) {
        //    alert("Please enter a valid email address");
        //    return;
        //}
        //Enable COntrols
        $(".disabledCtrl").attr("disabled", "disabled");
        $scope.registerObj.AppUserId = 0;
        $scope.registerObj.szImgURL = "";
        $scope.registerObj.szPasswordSalt = "";
        $scope.registerObj.iStatus = 0;
        $scope.registerObj.dCreatedOn = "2018-01-01";
        $scope.registerObj.iChangePW = false;
        $scope.registerObj.isDeleted = false;

        $.ajax({
            type: "POST",
            data: $scope.registerObj,
            url: apiURL + "/api/AppUser",
            async: false,
            success: function (data) {
                if (data.Success) {
                    $.notify(data.Message, 'success');
                    $('#loginModal').modal('hide');
                }
            }, error: function (data) {
            }
        });
        $(".disabledCtrl").removeAttr("disabled");
    };

    //Login Handler
    $scope.userLogin = function () {
        if ($scope.loginObj.szUsername == "") {
            $.notify("Please enter your username.", 'error');
            return;
        }
        if ($scope.loginObj.szPassword == "") {
            $.notify("Please enter your password.", 'error');
            return;
        }
        //show loader
        $("#loadingGif").css("display", "block");
        //Disable COntrols
        $(".disabledCtrl").attr("disabled", "disabled");

        setTimeout(function () {
            $.get(apiURL + "/api/AppUser/AuthenticateUser?szUsername=" + $scope.loginObj.szUsername + "&szPassword=" + $scope.loginObj.szPassword)
                .success(function (data) {
                    if (!data.Success) {
                        $.notify(data.Message, 'error');
                        //Hide Loading Gif
                        $("#loadingGif").css("display", "none");
                        //Enable COntrols
                        $(".disabledCtrl").removeAttr("disabled");
                        $scope.loginObj.szPassword = "";
                        $("#txtPassword").val("");
                        $("#txtUsername").focus();
                    } else {
                        $scope.StartValidUserSession(data.Data);
                    }
                }).error(function (data) {
                    $.notify(data.statusText, 'error');
                    //Hide Loading Gif
                    $("#loadingGif").css("display", "none");
                    //Enable COntrols
                    $(".disabledCtrl").removeAttr("disabled");
                });
        }, 1000);
    };

    //Get Application Roles Data for Dropdown
    $scope.StartValidUserSession = function (LoginAppUserVM) {
        $.post("/Account/StartValidUserSession", {
            loginAppUserVM: LoginAppUserVM
        }).success(function (data) {
            if (data != "") {
                window.location = data;
            }
        }).error(function (data) {
            $.notify(data.statusText, 'error');
            //Hide Loading Gif
            $("#loadingGif").css("display", "none");
            //Enable COntrols
            $(".disabledCtrl").removeAttr("disabled");
        });
    };
});

//});

$("#pcSubscriptionModal").modal("show");
$("a.flwpug_getpaid").find("button").addClass("btn btn-primary");

//Allow Only Numbers into Tel Textboxes
$(document).on("keypress keyup blur", ".allownumericwithoutdecimal", function (event) {
    ////If entry contains alphabet, validate email address format
    //var userName = $(this).val();
    //if (!/[a-z]/i.test(userName)) {
    //     $(this).val(userName.replace(/[^\d].+/, ""));
    //    if ((event.which < 48 || event.which > 57)) {
    //        event.preventDefault();
    //    }
    //}
});

//Function to Validate Email address format
function validateEmail(Email) {
    var pattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return $.trim(Email).match(pattern) ? true : false;
}