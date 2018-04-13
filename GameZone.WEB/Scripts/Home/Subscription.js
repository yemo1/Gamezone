/*Hide Loaders by default*/
$("#logLoda, #regLoda, #forgotLoda, #ChangeLoda, #subscribeLoda").css("display", "none");

gamezoneApp.controller('gamezoneCtrlr', ['$scope', '$http', function ($scope, $http) {
    if (_fltwvSubscription != "") {
        $.notify(_fltwvSubscription, 'info');
    }

    /*Make Games Page Menu Active*/
    $("#menuUL li").removeClass("current");
    $("#homeMenu").addClass("current");

    ("#subType2").checked = true;

    $scope.basicObj = {};
    $scope.registerObj = {};
    $scope.loginObj = {};
    $scope.forgotpwObj = {};
    $scope.changepwObj = {};
    $scope.subDetailOBJ = {};
    $scope.basicObj.sT = 1;
    $scope.payType = {};
    $scope.mtnMSISDN;
    $scope.headerData;
    $scope.HeaderId = 63;
    $scope.selectedGame = "";
    $scope.userName = "";
    var getSubDataInterval = null;
    var goInterval = null;

    var userOBJ = localStorage.getItem("UID");
    var userData = null;
    //if (userOBJ) {
    //    /*Do nothing*/
    //} else {
    //    /*User not logged in, back to home*/
    //    window.location = "/Home/";
    //}

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
                            window.location = window.location.href;
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

    /*Function to handle creation of new users*/
    $scope.AutoRegisterNewUser = function () {
        if (_IsMobile == "True") {
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
                            //window.location = window.location.href;
                        } else {
                            $.notify(data.Message, 'error');
                        }
                    }
                }, error: function (data) {
                }
            });
        }
    };

    /*Function to Log User Out*/
    $scope.LogUserOut = function (UID, delay) {
        $.post("/Account/LogOff", { "UID": UID }).success(function (data) {
            localStorage.removeItem("UID");
            localStorage.removeItem("selectedGame");
            if (data != "") {
                setTimeout(function () {
                    window.location = "/Home";
                }, delay);
            }
        }).error(function (data) {
            $.notify(data.statusText, 'error');
        });
    };

    /*Initialize Phone Number gotten from wap header*/
    if (_IsMobile == "True") {
        if (_mtnNumber != "") {
            $("#userIDSpan").text(_mtnNumber);
            $scope.userName = _mtnNumber;
            $("#txtUsername").val(_mtnNumber);
            $scope.loginObj.szUsername = _mtnNumber;
            $("#txtRegUsername").val(_mtnNumber);
            $scope.registerObj.szUsername = _mtnNumber;
            $scope.registerObj.isMobile = true;
            $scope.mtnMSISDN = _mtnNumber;
            $("#txtPhone").val(_mtnNumber);
            $scope.payType = 'airtime';
            $('#logout-target, .loginReg').css("display", "none");/*Hide Logout Button*/
        } else {
            $scope.payType = 'airtime';
            /* $scope.payType = 'card';*/
        }
    } else {
        $scope.payType = 'airtime';
        // $scope.payType = 'card';
    }

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
                        /*Clear Timer Interval*/
                        clearInterval(getSubDataInterval);
                        $scope.$apply(function () {
                            $scope.subDetailOBJ = data.Data;

                            $scope.subDetailOBJ.PeriodStart = $scope.formatDate(data.Data.PeriodStart);
                            $scope.subDetailOBJ.PeriodEnd = $scope.formatDate(data.Data.PeriodEnd);
                            if ($scope.dateIsEalierThanToday($scope.subDetailOBJ.PeriodEnd)) {
                                $scope.subDetailOBJ.IsActive = 0;
                            } else {
                                $scope.subDetailOBJ.IsActive = 1;
                            }
                            $("#txtIsActive").html(getRecordStatus($scope.subDetailOBJ.IsActive));
                        });
                    } else {
                        $scope.$apply(function () {
                            $scope.subDetailOBJ.PeriodStart = "-";
                            $scope.subDetailOBJ.PeriodEnd = "-";
                            $scope.subDetailOBJ.Period = "-";
                            $scope.subDetailOBJ.Amount = "-";
                            $scope.subDetailOBJ.IsActive = 0;
                            $("#txtIsActive").html(getRecordStatus($scope.subDetailOBJ.IsActive));
                        });
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
            async: false,
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

    $scope.deselectAll = function (index) {
        $.each($scope.headerData, function (i, rec) {
            rec.selected = false;
        });
        $scope.headerData[index].selected = 'true';
        $scope.HeaderId = $scope.headerData[index].HeaderId;
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
                    console.log(data);
                    if (data.Success) {
                        /*Clear Timer Interval*/
                        /*clearInterval(getSubDataInterval);*/
                        $scope.subDetailOBJ = {};
                        //$scope.$apply(function () {
                        $scope.subDetailOBJ = data.Data;
                        $scope.subDetailOBJ.Period = data.Data.ServiceName;
                        $scope.subDetailOBJ.PeriodStart = $scope.formatDate(data.Data.Sub);
                        $scope.subDetailOBJ.PeriodEnd = $scope.formatDate(data.Data.Exp);
                        if ($scope.dateIsEalierThanToday($scope.subDetailOBJ.PeriodEnd)) {
                            $scope.subDetailOBJ.IsActive = 0;
                        } else {
                            $scope.subDetailOBJ.IsActive = 1;
                        }
                        //console.log($scope.subDetailOBJ.PeriodStart);
                        $("#txtIsActive").html(getRecordStatus($scope.subDetailOBJ.IsActive));
                        //});
                    } else {
                        //$scope.$apply(function () {
                        $scope.subDetailOBJ.PeriodStart = "-";
                        $scope.subDetailOBJ.PeriodEnd = "-";
                        $scope.subDetailOBJ.Period = "-";
                        $scope.subDetailOBJ.Amount = "-";
                        $scope.subDetailOBJ.IsActive = 0;
                        $("#txtIsActive").html(getRecordStatus($scope.subDetailOBJ.IsActive));
                        //});
                    }
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

    /*Function to Refresh Page on Intervals*/
    $scope.refreshPage = function () {
        setTimeout(function () {
            window.location = window.location.href;
        }, 20000);
    };

    /*Get User Object and Request User Login*/
    var userOBJ = localStorage.getItem("UID");
    var UID = null;
    if (userOBJ) {
        UID = JSON.parse(userOBJ);
        if (_mtnNumber != "") {
            if (UID.szUsername != _mtnNumber) {
                $scope.AutoRegisterNewUser();
                return;
            } else {
                $scope.mtnMSISDN = _mtnNumber;
                $("#txtPhone").val(_mtnNumber);
            }
        }

        $scope.redirectURL = flutterWaveRedirectURL + UID.AppUserId;
        if ($scope.validateSubscription(UID.AppUserId) == "False") {
            $scope.subDetailOBJ.IsActive = 0;
            $scope.keepUserData(UID);
        } else {
            /*Clear Reload Interval*/
            clearInterval(goInterval);
            /*If Game was licked auto redirect*/
            var gameData = localStorage.getItem("selectedGame");
            if (gameData) {
                if (_frmGame == "True") {
                    var gameString = JSON.parse(gameData);
                    $scope.selectedGame = gameString.URL;
                    /* window.location = gameString.URL;*/
                }
            }
            $scope.subDetailOBJ.IsActive = 1;
        }
    } else {
        //if (_IsMobile == "True") {
        if (_mtnNumber != "") {/*Number is mtn*/
            $scope.AutoRegisterNewUser();
        } else {
            window.location = "/Account/Login";
        }
        //} else {
        //    window.location = "/Account/Login";
        //}
        $scope.subDetailOBJ.IsActive = 0;
    }

    /*Check for Subscription Data*/
    if (_subGo == "True") {
        $scope.HeaderId = parseInt(_HedaID);
        var userOBJ = localStorage.getItem("UID");
        var userData = null;
        if (userOBJ) {
            userData = JSON.parse(userOBJ);

            //#region URL Rewrite
                var myURL = window.location.href;
                var uidIndex = myURL.indexOf("uID");
                var urlWithoutUID = myURL.substr(0, uidIndex);
            //Rewrite UID
                myURL = urlWithoutUID + "uID=" + userData.AppUserId;

                var goIndex = myURL.indexOf("&go=true");
                var urlBeforego = myURL.substr(0, goIndex);
                var urlAftergo = myURL.substr(goIndex + ("&go=true".length), myURL.length);
            //Rewrite Go
                myURL = urlBeforego + "&go=false" + urlAftergo;
            //#endregion

            if (_mtnNumber != "") {
                $(".disabledCtrl").attr("disabled", "disabled");
                if (userData.szUsername != _mtnNumber) {/*Check if user loggedin is same as phone number detected*/
                    $scope.AutoRegisterNewUser();
                }
                $scope.HeaderId = parseInt(_HedaID);
                var refreshLrdy = localStorage.getItem("refreshLrdy");
                if (refreshLrdy) {
                    $(".disabledCtrl").removeAttr("disabled");
                    localStorage.removeItem("refreshLrdy");
                } else {
                    setTimeout(function () {
                        /*Refresh Page every 20 secs*/
                        localStorage.setItem("refreshLrdy", true);

                        window.location = myURL;
                    }, 20000);
                }
            } else {
                $scope.keepUserData(userData);
            }
        } else {
            if (_mtnNumber != "") {
                $scope.AutoRegisterNewUser();
            } else {
                window.location = "/Home/";
            }
        }
    }

    /*Authentication Handler*/
    $scope.authHandler = function (retURL) {
        var returnURL = null;
        var userOBJ = localStorage.getItem("UID");
        var userLoginToken = null;
        if (userOBJ) {
            var userData = JSON.parse(userOBJ),
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
                        } else {
                            returnURL = data;
                        }
                    } else {
                        localStorage.removeItem("UID");
                        /*Clear Username Display*/
                        ResetUsernameToAccount();
                        $('#loginModal').modal('show');
                    }
                }, error: function (data) {
                }
            });
        } else {
            if (_mtnNumber != "") {
                $scope.AutoRegisterNewUser();
            } else {
                localStorage.removeItem("selectedGame");
                /*Clear Username Display*/
                ResetUsernameToAccount();
                $('#loginModal').modal('show');
            }
        }
        return returnURL;
    };

    /*Function to handle creation of new users*/
    $scope.RegisterNewUser = function () {
        if (_IsMobile == "True") {
            if (_mtnNumber != "") {
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
                /*Do Nothing*/
            } else {
                if (userOBJ) {
                    userData = JSON.parse(userOBJ);
                    window.location = "/Home/Subscription?msisdn=" + _mtnNumber + "&go=" + false + "&mobi=" + _IsMobile + "&heda=0&frmGame=false&uID=" + userData.AppUserId;
                } else {
                    localStorage.removeItem("selectedGame");
                    /*Clear Username Display*/
                    ResetUsernameToAccount();
                    window.location = "/Home/";
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
                window.location = "/Home/";
            }
        }
    };

    $scope.USSDSubscription = function () {
        var userOBJ = localStorage.getItem("UID");
        var UID = null,
            appUserID = 0;
        if (userOBJ) {
            UID = JSON.parse(userOBJ);
            appUserID = UID.AppUserId;
        } else {
            if (_mtnNumber != "") {/*Number is mtn*/
                $scope.AutoRegisterNewUser();
            } else {
                window.location = "/Account/Login";
            }
            $scope.subDetailOBJ.IsActive = 0;
        }
        if ($scope.payType == undefined) {
            $.notify("Please select a payment type.", 'error');
            $("#payTypeSel").focus();
            return;
        }
        //if (_mtnNumber != "") {/*Number is mtn*/
        //    $("#txtPhone").val(_mtnNumber);
        //}
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
                AppUserID: appUserID,
                MSISDN: $("#txtPhone").val(), IsMtn: true,
                Shortcode: _SERVICE_SHORTCODE, headerId: $scope.HeaderId
            }).success(function (data) {
                var retVal = JSON.parse(data);
                if (!retVal.Success) {
                    $.notify(retVal.Message, 'error');
                    /*Hide Loading Gif*/
                    $("#subscribeLoda").css("display", "none");
                    /*Enable COntrols*/
                    $(".disabledCtrl").removeAttr("disabled");
                } else {
                    $.notify(retVal.Message, 'success');
                    /*Refresh Page every 20 secs*/
                    refreshPage();
                }
                /*Hide Loading Gif*/
                $("#subscribeLoda").css("display", "none");
                /*Enable COntrols*/
                //$(".disabledCtrl").removeAttr("disabled");
            }).error(function (data) {
                $.notify("Connectivity Error, please try again.", 'error');
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

    $(document).on("click", ".subType", function (event) {
        var amntSelected = ($(this).attr("id") == "subType1") ? 200 : ($(this).attr("id") == "subType2") ? 100 : 20;
        $(".flwpug_getpaid").attr("data-amount", amntSelected);
    });

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
}]);

$("a.flwpug_getpaid").find("button").addClass("btn btn-primary");

/*Function to Validate Email address format*/
function validateEmail(Email) {
    var pattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return $.trim(Email).match(pattern) ? true : false;
}
function getRecordStatus(status) {
    return (status == 1) ? "<span class='btn-custom btn-success btn-xs'>True</span>" : (status == 0) ? "<span class='btn-custom btn-warning btn-xs'>False</span>" : "";
}
function refreshPage() {
    setTimeout(function () {
        window.location = window.location.href;
    }, 20000);
};
$(document).on("click", "#gameBtn", function () {
    var gameData = localStorage.getItem("selectedGame");
    if (gameData) {
        if (_frmGame == "True") {
            var gameString = JSON.parse(gameData);
            window.location = gameString.URL;
        }
    }
});
