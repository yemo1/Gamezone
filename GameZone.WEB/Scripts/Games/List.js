﻿gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {
    /*Make Games Page Menu Active*/
    $("#menuUL li").removeClass("current");
    $("#gameMenu").addClass("current");
    /*Hide Loaders by default*/
    $("#logLoda, #regLoda, #forgotLoda, #ChangeLoda, #subscribeLoda").css("display", "none");

    $scope.basicObj = {};
    $scope.Game = {};
    $scope.CategoryGameList = [];
    $scope.subDetailOBJ = {};

    $scope.registerObj = {};
    $scope.loginObj = {};
    $scope.forgotpwObj = {};
    $scope.changepwObj = {};
    $scope.basicObj.sT = 1;
    $scope.payType = {};
    $scope.mtnMSISDN;
    $scope.headerData;
    $scope.HeaderId;
    $scope.userName ="";


    /*Login Handler*/
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

        /*show loader*/
        $("#logLoda").css("display", "block");
        /*Disable COntrols*/
        $(".disabledCtrl").attr("disabled", "disabled");

        setTimeout(function () {
            $.get(apiURL + "/api/AppUser/AuthenticateUser?szUsername=" + $scope.loginObj.szUsername + "&szPassword=" + $scope.loginObj.szPassword)
                .success(function (data) {
                    if (!data.Success) {
                        $.notify(data.Message, 'error');
                        /*Hide Loading Gif*/
                        $("#logLoda").css("display", "none");
                        /*Enable Controls*/
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
                            window.location.reload(true);
                            $('#loginModal').modal('hide');
                        }
                    }
                }).error(function (data) {
                    $.notify(data.statusText, 'error');
                    /*Hide Loading Gif*/
                    $("#logLoda").css("display", "none");
                    /*Enable COntrols*/
                    $(".disabledCtrl").removeAttr("disabled");
                });
        }, 1000);
    };

    /*Function to Log User Out*/
    $scope.LogUserOut = function (UID, delay) {
        $.post("/Account/LogOff", { "UID": UID }).success(function (data) {
            localStorage.removeItem("UID");
            localStorage.removeItem("selectedGame");
            if (data != "") {
                setTimeout(function () {
                    window.location = "/Games/List";
                }, delay);
            }
        }).error(function (data) {
            $.notify(data.statusText, 'error');
        });
    };

    /*Function to handle creation of new users*/
    $scope.AutoRegisterNewUser = function () {
        if (_IsMobile) {
            if (_mtnNumber != "") {
                $scope.registerObj.szUsername = _mtnNumber;
                $scope.registerObj.szPassword = "password";
                $scope.registerObj.isMobile = true;
            }

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
                        $scope.loginObj.szUsername = $scope.registerObj.szUsername;
                        $scope.loginObj.szPassword = $scope.registerObj.szPassword;
                        $scope.userLogin();

                        $scope.registerObj = {};
                    } else {
                        if (data.ID == "101") {/*MTN Number*/
                            $scope.loginObj.szUsername = data.Data.szUsername;
                            $scope.loginObj.szPassword = data.Data.szPassword;

                            localStorage.setItem("UID", JSON.stringify(data.Data));
                            $scope.registerObj = {};
                            window.location = "/Games/List";
                        } else {
                            $.notify(data.Message, 'error');
                        }
                    }
                }, error: function (data) {
                }
            });
        }
    };

    /*Initialize Phone Number gotten from wap header*/
    if (_IsMobile == "True") {
        if (_mtnNumber != "") {
            $("#userIDSpan").val(_mtnNumber);
            $scope.userName = _mtnNumber;
            $("#txtUsername").val(_mtnNumber);
            $scope.loginObj.szUsername = _mtnNumber;
            $("#txtRegUsername").val(_mtnNumber);
            $scope.registerObj.szUsername = _mtnNumber;
            $scope.registerObj.isMobile = true;
            $scope.mtnMSISDN = _mtnNumber;
            $scope.payType = 'airtime';
            $('#logout-target').css("display", "none");/*Hide Logout Button*/
        } else {
            $scope.payType = 'card';
        }
    } else {
        $scope.payType = 'card';
    }

    /*Function to handle creation of new users*/
    $scope.RegisterNewUser = function () {
        if (_IsMobile == "True") {
            if (_mtnNumber != null) {
                $scope.registerObj.szUsername = _mtnNumber;
                $scope.registerObj.isMobile = true;
            }
        }
        var szUsername = $scope.registerObj.szUsername;
        var containsAlphabet = /[a-z]/i.test(szUsername);
        if (containsAlphabet) {
            if (!validateEmail(szUsername)) {
                $.notify("Please enter a valid email address.", 'error');
                return;
            }
        }

        if ($scope.registerObj.szUsername == undefined) {
            $.notify("Please enter your username.", 'error');
            $("#txtRegUsername").focus();
            return;
        }
        if ($scope.registerObj.szUsername.trim() == "") {
            $.notify("Please enter your username.", 'error');
            $("#txtRegUsername").focus();
            return;
        }
        if ($scope.registerObj.szPassword == undefined) {
            $.notify("Please enter your password.", 'error');
            $("#txtRegPassword").focus();
            return;
        }
        if ($scope.registerObj.szPassword.trim() == "") {
            $.notify("Please enter your password.", 'error');
            $("#txtRegPassword").focus();
            return;
        }
        if ($scope.registerObj.szConfirmPassword == undefined) {
            $.notify("Please confirm your password.", 'error');
            $("#txtRegPasswordConfirm").focus();
            return;
        }
        if ($scope.registerObj.szConfirmPassword.trim() == "") {
            $.notify("Please confirm your password.", 'error');
            $("#txtRegPasswordConfirm").focus();
            return;
        }
        if ($scope.registerObj.szConfirmPassword.trim() != $scope.registerObj.szPassword.trim()) {
            $.notify("Passwords mismatch. Please confirm your password again.", 'error');
            $("#txtRegPasswordConfirm").focus();
            return;
        }

        /*show loader*/
        $("#regLoda").css("display", "block");
        /*Disable COntrols*/
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
                        $scope.loginObj.szUsername = $scope.registerObj.szUsername;
                        $scope.loginObj.szPassword = $scope.registerObj.szPassword;
                        $scope.userLogin();

                        $scope.registerObj = {};
                    } else {
                        $.notify(data.Message, 'error');
                    }
                    /*Hide Loading Gif*/
                    $("#regLoda").css("display", "none");
                    $(".disabledCtrl").removeAttr("disabled");
                }, error: function (data) {
                    /*Hide Loading Gif*/
                    $("#regLoda").css("display", "none");
                    $(".disabledCtrl").removeAttr("disabled");
                }
            });
        }, 1000);
    };

    /*Function to handle Reset of user's password*/
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

        /*show loader*/
        $("#forgotLoda").css("display", "block");
        /*Disable COntrols*/
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
                        localStorage.removeItem("UID");
                        localStorage.removeItem("selectedGame");
                        $scope.LogUserOut($scope.forgotpwObj.szUsername, 5000);
                    } else {
                        $.notify(data.Message, 'error');
                    }
                    /*Hide Loading Gif*/
                    $("#forgotLoda").css("display", "none");
                    $(".disabledCtrl").removeAttr("disabled");
                }, error: function (data) {
                    /*Hide Loading Gif*/
                    $("#forgotLoda").css("display", "none");
                    $(".disabledCtrl").removeAttr("disabled");
                }
            });
        }, 1000);
    };

    /*Function to handle Change of user's password*/
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

        /*show loader*/
        $("#ChangeLoda").css("display", "block");
        /*Disable COntrols*/
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
                        $.notify(data.Message, 'success', {
                            autoHide: true,
                            autoHideDelay: 8000
                        });
                        $('#passwordChangeModal').modal('hide');
                        $(".disabledChangeCtrl").removeAttr("disabled");
                        $("#ChangeLoda").css("display", "none");
                        $scope.LogUserOut($scope.changepwObj.szUsername, 5000);
                        $scope.changepwObj = {};
                    } else {
                        $.notify(data.Message, 'error');
                    }
                    /*Hide Loading Gif*/
                    $("#ChangeLoda").css("display", "none");
                    $(".disabledChangeCtrl").removeAttr("disabled");
                }, error: function (data) {
                    /*Hide Loading Gif*/
                    $("#ChangeLoda").css("display", "none");
                    $(".disabledChangeCtrl").removeAttr("disabled");
                }
            });
        }, 1000);
    };

    /*Get ApplicationUser Data from DB*/
    $scope.getGameData = function (selectedCat) {
        $("#isotopeContainer").empty();
        /*Show Loading Gif*/
        $("body").find('.game-loader').addClass('show').removeClass('hide');
        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=2000",
            async: true,
            success: function (data) {
                /*Empty Div*/
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
                        /*gameContent = "";*/
                        $('body').find('.lazy .img-responsive').lazyload({});
                    }
                });
            },
            error: function (data) {
                $.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };

    /*EPayment Subscriber*/
    $scope.GetSubData = function () {
        var userOBJ = localStorage.getItem("UID");
        var UID = null;
        if (userOBJ) {
            UID = JSON.parse(localStorage.getItem("UID"));

            $.ajax({
                type: "GET",
                url: apiURL + "/api/AppUser/SubscriptionDetails?UID=" + UID.AppUserId + "&svcName=" + svcName,
                async: false,
                success: function (data) {
                    if (data.Success) {
                        $scope.subDetailOBJ = data.Data;

                        $scope.subDetailOBJ.PeriodStart = $scope.formatDate(data.Data.PeriodStart);
                        $scope.subDetailOBJ.PeriodEnd = $scope.formatDate(data.Data.PeriodEnd);
                        if ($scope.dateIsEalierThanToday($scope.subDetailOBJ.PeriodEnd)) {
                            $scope.subDetailOBJ.IsActive = 0;
                        } else {
                            $scope.subDetailOBJ.IsActive = 1;
                        }
                        $("#txtIsActive").html(getRecordStatus($scope.subDetailOBJ.IsActive));
                    } else {
                        $scope.subDetailOBJ.PeriodStart = "-";
                        $scope.subDetailOBJ.PeriodEnd = "-";
                        $scope.subDetailOBJ.Period = "-";
                        $scope.subDetailOBJ.Amount = "-";
                        $scope.subDetailOBJ.IsActive = 0;
                        $("#txtIsActive").html(getRecordStatus($scope.subDetailOBJ.IsActive));
                    }
                }, error: function (data) {
                }
            });
        }
    };

    /*MTN Subscriber*/
    $scope.GetMTNSubData = function () {
        var userOBJ = localStorage.getItem("UID");
        var UID = null;
        if (userOBJ) {
            UID = JSON.parse(localStorage.getItem("UID"));
            $.ajax({
                type: "GET",
                url: apiURL + "/api/AppUser/MTNSubscriptionDetails?MSISDN=" + UID.szUsername + "&Shortcode=" + _SERVICE_SHORTCODE,
                async: false,
                success: function (data) {
                    if (data.Success) {
                        $scope.subDetailOBJ = data.Data;
                        $scope.subDetailOBJ.Period = data.Data.ServiceName;
                        $scope.subDetailOBJ.PeriodStart = $scope.formatDate(data.Data.Sub);
                        $scope.subDetailOBJ.PeriodEnd = $scope.formatDate(data.Data.Exp);
                        if ($scope.dateIsEalierThanToday($scope.subDetailOBJ.PeriodEnd)) {
                            $scope.subDetailOBJ.IsActive = 0;
                        } else {
                            $scope.subDetailOBJ.IsActive = 1;
                        }
                        $("#txtIsActive").html(getRecordStatus($scope.subDetailOBJ.IsActive));
                    } else {
                        $scope.subDetailOBJ.PeriodStart = "-";
                        $scope.subDetailOBJ.PeriodEnd = "-";
                        $scope.subDetailOBJ.Period = "-";
                        $scope.subDetailOBJ.Amount = "-";
                        $scope.subDetailOBJ.IsActive = 0;
                        $("#txtIsActive").html(getRecordStatus($scope.subDetailOBJ.IsActive));
                    }
                }, error: function (data) {
                }
            });
        }
    };

    /*MTN Subscriber*/
    $scope.getHeaderData = function () {
        $.ajax({
            type: "GET",
            url: "/Home/GetHeaderByServiceName?serviceName=" + svcName,
            async: true,
            success: function (data) {
                $scope.headerData = JSON.parse(data);
            }, error: function (data) {
            }
        });
    };
    $scope.getHeaderData();

    /*Hold User Data for Subscription Purpose*/
    $scope.keepUserData = function (LoginAppUserVM) {
        var userOBJ = localStorage.getItem("UID");
        var UID = null;
        if (userOBJ) {
            UID = JSON.parse(localStorage.getItem("UID"));
            $scope.redirectURL = flutterWaveRedirectURL + UID.AppUserId;

            $.ajax({
                type: "POST",
                data: { loginAppUserVM: UID },
                url: "/Account/StartValidUserSession",
                async: true,
                success: function (data) {
                }, error: function (data) {
                }
            });
        }
    };

    /*Return data for subscription validation based on user type*/
    $scope.validateSubscriptionData = function (AppUserId) {
        if (_mtnNumber != "") {/*Number is an MTN No*/
            return { "UID": AppUserId, "MSISDN": _mtnNumber, "svcName": svcName, "Shortcode": _SERVICE_SHORTCODE, "IsMtn": true };
        } else {
            return { "UID": AppUserId, "MSISDN": null, "svcName": svcName, "Shortcode": null, "IsMtn": false };
        }
    };

    /*Subscription Authentication Handler*/
    $scope.validateSubscription = function (AppUserId) {
        var retVal = false;
        $.ajax({
            type: "POST",
            data: $scope.validateSubscriptionData(AppUserId),
            url: "/Account/ValidSubscription",
            async: false,
            success: function (data) {
                retVal = data;
            }, error: function (data) {
            }
        });
        return retVal;
    };

    /*Get User Object*/
    var userOBJ = localStorage.getItem("UID");
    var UID = null;
    if (userOBJ) {
        UID = JSON.parse(userOBJ);
        $scope.redirectURL = flutterWaveRedirectURL + UID.AppUserId;
        if ($scope.validateSubscription(UID.AppUserId) == "False") {
            $scope.subDetailOBJ.IsActive = 0;
        } else {
            $scope.subDetailOBJ.IsActive = 1;
        }
    } else {
        if (_IsMobile == "True") {
            if (_mtnNumber != "") {/*Number is mtn*/
                /*Do nothing*/
            } else {
                $('#loginModal').modal('show');
            }
        } else {
            $('#loginModal').modal('show');
        }
        $scope.subDetailOBJ.IsActive = 0;
        $('.subscribeBTN').css("display", "none");
    }

    /*Authentication Handler*/
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
                            localStorage.removeItem("UID");
                            /*Clear Username Display*/
                            ResetUsernameToAccount();
                            if (_mtnNumber != "") {
                                /*Do nothing*/
                            } else {
                                localStorage.removeItem("selectedGame");
                                $('#loginModal').modal('show');
                            }
                           /* window.location = data.split(';')[0];*/
                        } else {
                            returnURL = data;
                        }
                    } else {
                        if (_mtnNumber != "") {
                            /*Do nothing*/
                        } else {
                            localStorage.removeItem("UID");
                            /*Clear Username Display*/
                            ResetUsernameToAccount();
                            $('#loginModal').modal('show');
                        }
                    }
                }, error: function (data) {
                }
            });
        } else {
            if (_mtnNumber != "") {
                /*Do nothing*/
            } else {
                /*Clear Username Display*/
                ResetUsernameToAccount();
                $('#loginModal').modal('show');
            }
        }
        return returnURL;
    };

    /*Get Application Roles Data for Dropdown*/
    $scope.StartValidUserSession = function (LoginAppUserVM) {
        $.post("/Account/StartValidUserSession", {
            loginAppUserVM: LoginAppUserVM
        }).success(function (data) {
            if (data != "") {
                window.location = data;
            }
        }).error(function (data) {
            $.notify(data.statusText, 'error');
            /*Hide Loading Gif*/
            $("#loadingGif").css("display", "none");
            /*Enable COntrols*/
            $(".disabledCtrl").removeAttr("disabled");
        });
    };

    $scope.subscriptionBtnClickEventHandler = function () {
        var userOBJ = localStorage.getItem("UID");
        var userData = null;
        if (_IsMobile == "True") {
            if (_mtnNumber != "") {/*Number is mtn*/
                window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=false&uID=0";
            } else {
                if (userOBJ) {
                    userData = JSON.parse(userOBJ);
                    window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=false&uID=" + userData.AppUserId;
                } else {
                    localStorage.removeItem("selectedGame");
                    /*Clear Username Display*/
                    ResetUsernameToAccount();
                    $('#loginModal').modal('show');
                }
            }
        } else {
            if (userOBJ) {
                userData = JSON.parse(userOBJ);
                window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=false&uID=" + userData.AppUserId;
            } else {
                localStorage.removeItem("selectedGame");
                /*Clear Username Display*/
                ResetUsernameToAccount();
                $('#loginModal').modal('show');
            }
        }
    };

    $scope.USSDSubscription = function () {
        if ($scope.payType == undefined) {
            $.notify("Please select a payment type.", 'error');
            $("#payTypeSel").focus();
            return;
        }
        if ($("#txtPhone").val().trim() == "") {
            $.notify("Please enter your phone no.", 'error');
            $("#txtPhone").focus();
            return;
        }
        if ($scope.HeaderId == undefined) {
            $.notify("Please select a subscription package.", 'error');
            $("#packageSel").focus();
            return;
        }
        if ($scope.HeaderId == "") {
            $.notify("Please select a subscription package.", 'error');
            $("#packageSel").focus();
            return;
        }
        /*show loader*/
        $("#subscribeLoda").css("display", "block");
        /*Disable COntrols*/
        $(".disabledCtrl").attr("disabled", "disabled");
        setTimeout(function () {
            $.post("/Home/MTNUSSDSubscription", {
                MSISDN: $("#txtPhone").val(), IsMtn: true,
                Shortcode: _SERVICE_SHORTCODE, headerId: $scope.HeaderId
            }).success(function (data) {
                var retVal = JSON.parse(data);
                if (!retVal.Success) {
                    $.notify(retVal.Message, 'error');
                } else {
                    $.notify(retVal.Message, 'success');
                    $('#pcSubscriptionModal').modal('hide');
                }
                /*Hide Loading Gif*/
                $("#subscribeLoda").css("display", "none");
                /*Enable COntrols*/
                $(".disabledCtrl").removeAttr("disabled");
            }).error(function (data) {
                $.notify(data, 'error');
                /*Hide Loading Gif*/
                $("#subscribeLoda").css("display", "none");
                /*Enable COntrols*/
                $(".disabledCtrl").removeAttr("disabled");
            });
        }, 1000);
    };

    $scope.UnSubscribe = function () {
        if (_IsMobile == "True") {
            if (_mtnNumber != "") {/*Number is mtn*/
                alert("Please Text 'Stop Game' to " + _SERVICE_SHORTCODE + " to Unsubscribe (MTN Only).");
                return;
            }
        }

        var data = JSON.stringify(false);

        var xhr = new XMLHttpRequest();
        xhr.withCredentials = true;

        xhr.addEventListener("readystatechange", function () {
            if (this.readyState === this.DONE) {
                console.log(this.responseText);
            }
        });

        xhr.open("POST", "http://flw-pms-dev.eu-west-1.elasticbeanstalk.com/merchant/subscriptions/stop");
        xhr.setRequestHeader("content-type", "application/json");

        xhr.send(data);
    };

    /*Method to Format Date to format (DD/MM/YYYY)*/
    $scope.formatDate = function (dDate) {
        if (dDate != "" && dDate != null) {
            var dDate = new Date(dDate),
            month = dDate.getMonth() + 1,
            day = dDate.getDate(),
            year = dDate.getFullYear();
            day = (day < 10) ? '0' + day.toString() : day;
            month = (month < 10) ? '0' + month.toString() : month;
            return day + "/" + month + "/" + year;
        } else {
            return "";
        }
    };

    /*Check for Date Greater than today*/
    $scope.dateIsEalierThanToday = function (dDate) {
        var date = dDate.substring(0, 2);
        var month = dDate.substring(3, 5);
        var year = dDate.substring(6, 10);
        var convDate = new Date(year, month - 1, date);
        var todaysDate = new Date();
        return convDate < todaysDate;
    };

    /*Populate Page with Games*/
    $scope.getGameData("family");

    /*Logout Handler*/
    $(document).on("click", "#logout-target", function (event) {
        $scope.LogUserOut();
    });

    /*CLick handler of Menu Items*/
    $(".gameMenu").click(function (e) {
        var selCat = $(this).attr("id");
        $scope.getGameData(selCat);
    });

    /*Game CLick Event Handler*/
    $(document).on("click", "a.game-link", function (e) {
        e.preventDefault();
        e.preventDefault();

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

        $('#lodaModal').modal('show');

        setTimeout(function () {
            /*Authentication*/
            var retVal = $scope.authHandler("/games/gameplay");
            if (retVal != null) {
                var userOBJ = localStorage.getItem("UID");
                var userData = null;
                if (userOBJ) {
                    userData = JSON.parse(userOBJ);
                    if ($scope.validateSubscription(userData.AppUserId) == "False") {
                        var uID = 0;
                        if (_IsMobile == "True") {
                            if (_mtnNumber != "") {/*Number is mtn*/
                                localStorage.setItem("selectedGame", JSON.stringify(selectedGame));
                                window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=true&uID=0";
                            } else {
                                window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=true&uID=" + userData.AppUserId;
                            }
                        } else {
                            window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=false&uID=" + userData.AppUserId;
                        }
                    } else {
                        localStorage.setItem("selectedGame", JSON.stringify(selectedGame));
                        e.preventDefault();
                        e.preventDefault();
                        window.location = selGameURL;
                    }
                } else {
                    if (_mtnNumber != "") {
                        localStorage.setItem("selectedGame", JSON.stringify(selectedGame));
                        window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=true&uID=0";
                    } else {
                        localStorage.removeItem("selectedGame");
                        /*Clear Username Display*/
                        ResetUsernameToAccount();
                        $('#lodaModal').modal('hide');
                        $('#loginModal').modal('show');
                    }
                }
            } else {
                if (_mtnNumber != "") {
                    localStorage.setItem("selectedGame", JSON.stringify(selectedGame));
                    e.preventDefault();
                    e.preventDefault();
                    window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=true&uID=0";
                } else {
                    if (_mtnNumber != "") {
                        localStorage.setItem("selectedGame", JSON.stringify(selectedGame));
                        window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=true&uID=0";
                    } else {
                        localStorage.removeItem("selectedGame");
                        /*Clear Username Display*/
                        ResetUsernameToAccount();
                        $('#lodaModal').modal('hide');
                        $('#loginModal').modal('show');
                    }
                }
            }
        }, 1000);
    });
});
/*Function to Validate Email address format*/
function validateEmail(Email) {
    var pattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return $.trim(Email).match(pattern) ? true : false;
}
function getRecordStatus(status) {
    return (status == 1) ? "<span class='btn-custom btn-success btn-xs'>True</span>" : (status == 0) ? "<span class='btn-custom btn-warning btn-xs'>False</span>" : "";
}