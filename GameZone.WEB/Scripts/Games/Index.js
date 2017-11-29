
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
            url: apiURL + "/api/Game?gameCategory=sports&gameCount=5",
            async: false,
            success: function (data) {
                $.each(data.Data, function (i, rec) {
                    var gameImage = "<img class='gameImg' src='" + rec.banner_medium + "' style='margin:10px;'/>";
                    $("#gameDiv").append(gameImage);
                });
                console.log(data);
                //$scope.rowCollection = data.Data;
                //$scope.loadDataTable();
            },
            error: function (data) {
                //$.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };
    $scope.getGameData();

});

