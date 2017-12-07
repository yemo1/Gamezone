
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
    $scope.getGameData = function (selectedCat) {
        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=12",
            async: false,
            success: function (data) {

                console.log(data);
                
                var gameContent = "";

                //$("#bonusGame").attr("url", data.Data[0].url);

                //var gameImage = "<div class='row'>";

                //Empty Div
                $("#isotopeContainer").empty();

                $.each(data.Data, function (i, rec) {
                    if (rec.title != "What's My Icon?") {
                        gameContent = "<div class='col-xs-12 col-sm-8 col-md-4 isotopeSelector block " + selectedCat + "'>";
                        gameContent = gameContent + "<div class='service-wrap hovereffect panel clearfix animate' data-animate='bounceIn' data-duration='1.0s' data-delay='0.2s'>";
                        gameContent = gameContent + "<a href='' class='game-link'>";
                        gameContent = gameContent + "<img src='" + rec.banner_medium + "' alt='Sweet Candy Land' class='img-responsive'/>";
                        gameContent = gameContent + "<div class='overlay description'>";
                        gameContent = gameContent + "<h3 class='game-title'>" + rec.title + "</h3>";
                        gameContent = gameContent + "<p class='text-justify'>" + rec.short_description + "</p>";
                        gameContent = gameContent + "</div></a></div></div>";
                        $("#isotopeContainer").append(gameContent);
                        gameContent = "";
                    }
                });
            },
            error: function (data) {
                //$.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };
    $scope.getGameData("family");

    $(".gameMenu").click(function (e) {
      var selCat = $(this).attr("id");
      $scope.getGameData(selCat);
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
});

