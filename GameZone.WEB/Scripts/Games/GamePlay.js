
var formTitle = _Title;

gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {
    //Make Games Page Menu Active
    $("#topMenu li").removeClass("current");
    $("#gameMenu").addClass("current");

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
    $scope.Game = {};

    //Get ApplicationUser Data from DB
    $scope.getOtherCategoryGames = function (selectedCat) {
        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=10",
            async: false,
            success: function (data) {
                var gameContent = "";
                //Empty Div
                $("#otherCategoryGames").empty();

                $.each(data.Data, function (i, rec) {
                    if (rec.title != "What's My Icon?") {
                        gameContent = "<div class='sp-slide'>";
                        gameContent = gameContent + "<a href='" + rec.url + "' class='OtherCatGame-link'>";
                        gameContent = gameContent + "<img class='sp-image' src='" + rec.banner_medium + "'data-src='" + rec.banner_medium + "'data-retina='" + rec.banner_medium + "'/>";
                        gameContent = gameContent + "</a>";
                        gameContent = gameContent + "<p class='sp-caption'>" + rec.title;
                        gameContent = gameContent + "<br>";
                        gameContent = gameContent + "<span class='text-center small text-muted'>" + selectedCat + "</span>";
                        gameContent = gameContent + "</p></div>";
                        $("#otherCategoryGames").append(gameContent);
                        gameContent = "";
                    }
                });
            },
            error: function (data) {
                //$.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };    

    $scope.InstantiateGame = function () {
        var gameData = sessionStorage.getItem("selectedGame");
        gameData = JSON.parse(gameData);
        gameData.Category = gameData.Category.toUpperCase();
        $scope.Game = gameData;
        $("#gamePlay").attr("src", gameData.URL);
        $("#longDesc").html($.parseHTML(gameData.LongDescription));
        //Load Other Games in same category
        $scope.getOtherCategoryGames(gameData.Category.toLowerCase());
    };
    $scope.InstantiateGame();

    //CLick handler of Menu Items
    $(".gameMenu").click(function (e) {
        var selCat = $(this).attr("id");
        $scope.getGameData(selCat);
    });

    //Game CLick Event Handler
    $("a.game-link").click(function (e) {
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
        sessionStorage.setItem("selectedGame", selectedGame);
        e.preventDefault();
        window.location = "games/gameplay";
    });


});

