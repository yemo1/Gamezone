//$(document).ready(function () {
//    //Subscription Successful
//    if (_subscriptionSuccessful != "") {
//        if (_subscriptionSuccessful == "true") {
//            $.notify("Subscription Successful.", 'success');
//        } else {
//            $.notify("Subscription Failed.", 'error');
//        }
//    }
//});
gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {

    //Make Games Page Menu Active
    $("#menuUL li").removeClass("current");
    $("#homeMenu").addClass("current");

    //Hide Loaders by default
    $("#logLoda, #regLoda, #forgotLoda, #ChangeLoda").css("display", "none");

    ("#subType2").checked = true;

    //Get User Object
    var userOBJ = localStorage.getItem("UID");
    var UID = null;
    if (userOBJ) {
        UID = JSON.parse(localStorage.getItem("UID"));
        $scope.redirectURL = flutterWaveRedirectURL + UID.AppUserId;
    } else {
        $('#loginModal').modal('show');
    }

    $.notify("Thank you.", { position: "left" });

    //$('#subscriptionDetailsModal').modal('show');

    
    $scope.basicObj = {};
    $scope.registerObj = {};
    $scope.loginObj = {};
    $scope.forgotpwObj = {};
    $scope.changepwObj = {};
    //100 N Weekly
    $scope.basicObj.sT = 1;

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
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=13",
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
                        //$(window).scroll(function () {
                        //    if ($(window).scrollTop() >= $('#game-area').height()) {
                        //        //$("#pcSubscriptionModal").show();
                        //    }
                        //    else {
                        //        //$("#pcSubscriptionModal").hide("");
                        //    }
                        //});
                    }
                });
            },
            error: function (data) {
                //$.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };
    $scope.getGameData("family");

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
                                $('#loginModal').modal('show');
                            }, 5000);
                        } else {
                            if ($scope.validateSubscription(userData.AppUserId) == "False") {
                                $.notify("Subscription Expired", 'error');
                                localStorage.removeItem("selectedGame");
                                setTimeout(function () {
                                    $scope.keepUserData(userData);
                                    $('#pcSubscriptionModal').modal('show');
                                }, 5000);
                            } else {
                                returnURL = data;
                            }
                        }
                    } else {
                        $.notify("Access Denied", 'error');
                        $('#loginModal').modal('show');
                    }
                }, error: function (data) {
                }
            });
        } else {
            $.notify("Access Denied", 'error');
            $('#loginModal').modal('show');
        }
        return returnURL;
    };



    //CLick handler of Menu Items
    $(".gameMenu").click(function (e) {
        var selCat = $(this).attr("id");
        $scope.getGameData(selCat);
        e.preventDefault();
        e.preventDefault();
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
            e.preventDefault();
            e.preventDefault();

            window.location = retVal;
        }
    });

    $scope.SaveNewSubscriber = function () {

        alert($scope.basicObj.sT);
        return;
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
        var szUsername = $scope.registerObj.szUsername;
        var containsAlphabet = /[a-z]/i.test(szUsername);
        if (containsAlphabet) {
            if (!validateEmail(szUsername)) {
                $.notify("Please enter a valid email address.", 'error');
                return;
            }
        }

        if ($scope.registerObj.szUsername == undefined || $scope.registerObj.szUsername.trim() == "") {
            $.notify("Please enter your username.", 'error');
            $("#txtRegUsername").focus();
            return;
        }
        if ($scope.registerObj.szPassword == undefined || $scope.registerObj.szPassword.trim() == "") {
            $.notify("Please enter your password.", 'error');
            $("#txtRegPassword").focus();
            return;
        }
        if ($scope.registerObj.szConfirmPassword == undefined || $scope.registerObj.szConfirmPassword.trim() == "") {
            $.notify("Please confirm your password.", 'error');
            $("#txtRegPasswordConfirm").focus();
            return;
        }
        if ($scope.registerObj.szConfirmPassword.trim() != $scope.registerObj.szPassword.trim()) {
            $.notify("Passwords mismatch. Please confirm your password again.", 'error');
            $("#txtRegPasswordConfirm").focus();
            return;
        }

        //show loader
        $("#regLoda").css("display", "block");
        //Disable COntrols
        $(".disabledCtrl").attr("disabled", "disabled");
        $scope.registerObj.AppUserId = 0;
        $scope.registerObj.szImgURL = "";
        $scope.registerObj.szPasswordSalt = "";
        $scope.registerObj.iStatus = 0;
        $scope.registerObj.dCreatedOn = "2018-01-01";
        $scope.registerObj.iChangePW = false;
        $scope.registerObj.isDeleted = false;

        setTimeout(function () {
            $.ajax({
                type: "POST",
                data: $scope.registerObj,
                url: apiURL + "/api/AppUser",
                async: false,
                success: function (data) {
                    if (data.Success) {
                        $.notify(data.Message, 'success');
                        //$scope.$apply(function () {
                        //    $scope.productRowCollection = resultData;
                        //});
                        $scope.loginObj.szUsername = $scope.registerObj.szUsername;
                        $scope.loginObj.szPassword = $scope.registerObj.szPassword;
                        $scope.userLogin();

                        $scope.registerObj = {};
                        //$('#loginModal').modal('hide');
                    } else {
                        $.notify(data.Message, 'error');
                    }
                    //Hide Loading Gif
                    $("#regLoda").css("display", "none");
                    $(".disabledCtrl").removeAttr("disabled");
                }, error: function (data) {
                    //Hide Loading Gif
                    $("#regLoda").css("display", "none");
                    $(".disabledCtrl").removeAttr("disabled");
                }
            });
        }, 1000);
    };

    $scope.ResetUserPW = function () {
        var szUsername = $scope.forgotpwObj.szUsername;
        var containsAlphabet = /[a-z]/i.test(szUsername);
        if (containsAlphabet) {
            if (!validateEmail(szUsername)) {
                $.notify("Please enter a valid email address.", 'error');
                return;
            }
        }

        if ($scope.forgotpwObj.szUsername == undefined || $scope.forgotpwObj.szUsername.trim() == "") {
            $.notify("Please enter your username.", 'error');
            $("#txtRegUsername").focus();
            return;
        }

        //show loader
        $("#forgotLoda").css("display", "block");
        //Disable COntrols
        $(".disabledCtrl").attr("disabled", "disabled");

        $scope.forgotpwObj.AppUserId = 0;
        $scope.forgotpwObj.szImgURL = "";
        $scope.forgotpwObj.szPasswordSalt = "";
        $scope.forgotpwObj.iStatus = 0;
        $scope.forgotpwObj.dCreatedOn = "2018-01-01";
        $scope.forgotpwObj.iChangePW = false;
        $scope.forgotpwObj.isDeleted = false;

        setTimeout(function () {
            $.ajax({
                type: "POST",
                data: $scope.forgotpwObj,
                url: apiURL + "/api/AppUser/ResetPw",
                async: false,
                success: function (data) {
                    if (data.Success) {
                        $scope.forgotpwObj = {};
                        $.notify(data.Message, 'success', {
                            autoHide: true,
                            autoHideDelay: 10000
                        });
                        $('#loginModal').modal('hide');
                        $scope.LogUserOut(11000);
                    } else {
                        $.notify(data.Message, 'error');
                    }
                    //Hide Loading Gif
                    $("#forgotLoda").css("display", "none");
                    $(".disabledCtrl").removeAttr("disabled");
                }, error: function (data) {
                    //Hide Loading Gif
                    $("#forgotLoda").css("display", "none");
                    $(".disabledCtrl").removeAttr("disabled");
                }
            });
        }, 1000);
    };

    $scope.ChangeUserPW = function () {
        var szUsername = $scope.changepwObj.szUsername;
        var containsAlphabet = /[a-z]/i.test(szUsername);
        if (containsAlphabet) {
            if (!validateEmail(szUsername)) {
                $.notify("Please enter a valid email address.", 'error');
                return;
            }
        }

        if ($scope.changepwObj.szUsername == undefined || $scope.changepwObj.szUsername.trim() == "") {
            $.notify("Please enter your username.", 'error');
            $("#txtRegUsername").focus();
            return;
        }
        if ($scope.changepwObj.szPassword == undefined || $scope.changepwObj.szPassword.trim() == "") {
            $.notify("Please enter your password.", 'error');
            $("#txtRegPassword").focus();
            return;
        }
        if ($scope.changepwObj.szConfirmPassword == undefined || $scope.changepwObj.szConfirmPassword.trim() == "") {
            $.notify("Please confirm your password.", 'error');
            $("#txtRegPasswordConfirm").focus();
            return;
        }
        if ($scope.changepwObj.szConfirmPassword.trim() != $scope.changepwObj.szPassword.trim()) {
            $.notify("Passwords mismatch. Please confirm your password again.", 'error');
            $("#txtRegPasswordConfirm").focus();
            return;
        }

        //show loader
        $("#ChangeLoda").css("display", "block");
        //Disable COntrols
        $(".disabledChangeCtrl").attr("disabled", "disabled");

        $scope.changepwObj.AppUserId = 0;
        $scope.changepwObj.szImgURL = "";
        $scope.changepwObj.szPasswordSalt = "";
        $scope.changepwObj.iStatus = 0;
        $scope.changepwObj.dCreatedOn = "2018-01-01";
        $scope.changepwObj.iChangePW = false;
        $scope.changepwObj.isDeleted = false;

        setTimeout(function () {
            $.ajax({
                type: "POST",
                data: $scope.changepwObj,
                url: apiURL + "/api/AppUser/ChangePw",
                async: false,
                success: function (data) {
                    if (data.Success) {
                        $scope.forgotpwObj = {};
                        $.notify(data.Message, 'success', {
                            autoHide: true,
                            autoHideDelay: 8000
                        });
                        $('#passwordChangeModal').modal('hide');
                        $(".disabledChangeCtrl").removeAttr("disabled");
                        $scope.LogUserOut(9000);
                    } else {
                        $.notify(data.Message, 'error');
                    }
                    //Hide Loading Gif
                    $("#ChangeLoda").css("display", "none");
                    $(".disabledChangeCtrl").removeAttr("disabled");
                }, error: function (data) {
                    //Hide Loading Gif
                    $("#ChangeLoda").css("display", "none");
                    $(".disabledChangeCtrl").removeAttr("disabled");
                }
            });
        }, 1000);
    };

    //Login Handler
    $scope.userLogin = function () {
        if ($scope.loginObj.szUsername == undefined || $scope.loginObj.szUsername.trim() == "") {
            $.notify("Please enter your username.", 'error');
            $("#txtUsername").focus();
            return;
        }
        if ($scope.loginObj.szPassword == undefined || $scope.loginObj.szPassword.trim() == "") {
            $.notify("Please enter your password.", 'error');
            $("#txtPassword").focus();
            return;
        }

        //show loader
        $("#logLoda").css("display", "block");
        //Disable COntrols
        $(".disabledCtrl").attr("disabled", "disabled");

        setTimeout(function () {
            $.get(apiURL + "/api/AppUser/AuthenticateUser?szUsername=" + $scope.loginObj.szUsername + "&szPassword=" + $scope.loginObj.szPassword)
                .success(function (data) {
                    if (!data.Success) {
                        $.notify(data.Message, 'error');
                        //Hide Loading Gif
                        $("#logLoda").css("display", "none");
                        //Enable Controls
                        $(".disabledCtrl").removeAttr("disabled");
                        $scope.loginObj.szPassword = "";
                        $("#txtPassword").val("");
                        $("#txtUsername").focus();
                    } else {
                        if (data.Data.iChangePW) {
                            $('#loginModal').modal('hide');
                            $('#passwordChangeModal').modal('show');
                        } else {
                            localStorage.setItem("UID", JSON.stringify(data.Data));
                            window.location = "/Home";
                            $('#loginModal').modal('hide');
                        }
                    }
                }).error(function (data) {
                    $.notify(data.statusText, 'error');

                    //Hide Loading Gif
                    $("#logLoda").css("display", "none");
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

    $scope.keepUserData = function (LoginAppUserVM) {
        var userOBJ = localStorage.getItem("UID");
        var UID = null;
        if (userOBJ) {
            UID = JSON.parse(localStorage.getItem("UID"));
            $scope.redirectURL = flutterWaveRedirectURL + UID.AppUserId;

            $.post("/Account/StartValidUserSession", {
                loginAppUserVM: UID
            }).success(function (data) {

            }).error(function (data) {

            });
        }
    };
    //Function to Log User Out
    //$scope.LogUserOut = function (delay) {
    //    $.post("/Account/LogOff").success(function (data) {
    //        if (data != "") {
    //            setTimeout(function () {
    //                window.location = data;
    //            }, delay);
    //        }
    //    }).error(function (data) {
    //        $.notify(data.statusText, 'error');
    //    });
    //};
    ////Logout Handler
    //$(document).on("click", "#logout-target", function (event) {
    //    $scope.LogUserOut(1000);
    //});

    $(document).on("click", ".subType", function (event) {
        var amntSelected = ($(this).attr("id") == "subType1") ? 200 : ($(this).attr("id") == "subType2") ? 100 : 20;
        $(".flwpug_getpaid").attr("data-amount", amntSelected);
    });
});

//Allow Only Numbers into Tel Textbox
//$(document).on("keypress keyup blur", ".allownumericwithoutdecimal", function (event) {
//    $(this).val($(this).val().replace(/[^\d].+/, ""));
//    if ((event.which < 48 || event.which > 57)) {
//        event.preventDefault();
//    }
//});

//$("#pcSubscriptionModal").modal("show");

$("a.flwpug_getpaid").find("button").addClass("btn btn-primary");

//Allow Only Numbers into Tel Textboxes
$(document).on("keypress keyup blur", ".allownumericwithoutdecimal", function (event) {
    //If entry contains alphabet, validate email address format
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

function addHeader() {
    //Set HTTP Header
    var userOBJ = localStorage.getItem("UID");
    var appUserId = 0;
    if (userOBJ) {
        var userData = JSON.parse(localStorage.getItem("UID"));
        appUserId = userData.AppUserId;
    } else {
        return null;
    }
    $.ajaxSetup({
        headers: {
            "appUserId": appUserId
        }
    });
}
