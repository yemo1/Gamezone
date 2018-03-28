gamezoneApp.controller('gamezoneCtrlr', ['$scope', '$http', function ($scope, $http) {
    /*Make Games Page Menu Active*/
    $("#menuUL li").removeClass("current");
    $("#homeMenu").addClass("current");

    /*Hide Loaders by default*/
    $("#logLoda, #regLoda, #forgotLoda, #ChangeLoda, #subscribeLoda").css("display", "none");

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
    $scope.HeaderId;
    $scope.userName = "";
    $scope.wellMsg = "";
    $scope.pwResetSuccess = 0;
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
                            window.location = "/Home";
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
            console.log(data);
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
            $scope.payType = 'airtime';
            $('#logout-target, .loginReg').css("display", "none");/*Hide Logout Button*/
        } else {
            $scope.payType = 'card';
        }
    } else {
        $scope.payType = 'card';
    }

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

    /*Authentication Handler*/
    $scope.authHandler = function (retURL) {
        var returnURL = "";
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
            } else {
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
                        $scope.$apply(function () {
                            $scope.pwResetSuccess = 1;
                            $scope.forgotpwObj = {};
                            $scope.wellMsg = data.Message;
                        });
                        $("#pWellMsg").text(data.Message);
                        //$.notify(data.Message, 'success', {
                        //    autoHide: true,
                        //    autoHideDelay: 10000
                        //});
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

}]);

/*Function to Validate Email address format*/
function validateEmail(Email) {
    var pattern = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
    return $.trim(Email).match(pattern) ? true : false;
}
function getRecordStatus(status) {
    return (status == 1) ? "<span class='btn-custom btn-success btn-xs'>True</span>" : (status == 0) ? "<span class='btn-custom btn-warning btn-xs'>False</span>" : "";
}