
var formTitle = _Title;

gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {

    $scope.obj = {};
    $scope.basicObj = {};
    $scope.contactObj = {};
    $scope.rowCollection = [];
    $scope.CountryList = [{}];
    $scope.StateList = [];
    $scope.statusList = [];
    $scope.branchList = [{}];
    $scope.modalBranchList = [{}];
    $scope.employeeList = [{}];
    $scope.modalHeaderText = "New " + formTitle + " Details";
    $scope.roleList = [{}];
    $scope.originalData = [];

    //Get ApplicationUser Data from DB
    $scope.getGameData = function () {
        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=sports&gameCount=20",
            async: false,
            success: function (data) {
                var gameImage = "<div class='row'>";

                $.each(data.Data, function (i, rec) {
                    if ((i + 1) < 3) {
                        gameImage = gameImage + "<div class='col-md-4'><img class='gameImg' src='" + rec.banner_medium + "' style='margin:10px;'/></div>";
                    }
                    else if ((i + 1) == data.Data.length) {
                        gameImage = gameImage + "<div class='col-md-4'><img class='gameImg' src='" + rec.banner_medium + "' style='margin:10px;'/></div>";
                        gameImage = gameImage + "</div>";
                        $("#gameDiv").append(gameImage);
                    }
                    else if ((i + 1) % 3 != 0) {
                        gameImage = gameImage + "<div class='col-md-4'><img class='gameImg' src='" + rec.banner_medium + "' style='margin:10px;'/></div>";
                    }
                    else {
                        gameImage = gameImage + "<div class='col-md-4'><img class='gameImg' src='" + rec.banner_medium + "' style='margin:10px;'/></div>";
                        gameImage = gameImage + "</div>";
                        $("#gameDiv").append(gameImage);
                        gameImage = "<div class='row'>";
                    }
                });
            },
            error: function (data) {
                //$.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };
    $scope.getGameData();

});

