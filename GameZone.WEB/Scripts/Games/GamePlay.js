
var formTitle = _Title;

gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {
    //Make Games Page Menu Active
    $("#menuUL li").removeClass("current");
    $("#gameMenu").addClass("current");

    $scope.basicObj = {};
    $scope.Game = {};

    //Get ApplicationUser Data from DB
    $scope.getOtherCategoryGames = function (selectedCat) {
        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=5",
            async: false,
            success: function (data) {
                var gameContent = "";
                //Empty Div
                $("#otherCategoryGames").empty();

                $.each(data.Data, function (i, rec) {
                    if (rec.title != "What's My Icon?") {
                        gameContent = "<div class='sp-slide'>";
                        gameContent = gameContent + "<a href='" + rec.url + "' class='OtherCatGame-link'>";
                        gameContent = gameContent + "<p class='game-category hiddenPara'>" + selectedCat + "," + rec.title + "</p>";
                        gameContent = gameContent + "<div class='longDescription hiddenPara'>" + rec.long_description + "</div>";
                        gameContent = gameContent + "<img class='sp-image' src='" + rec.banner_medium + "'data-src='" + rec.banner_medium + "'data-retina='" + rec.banner_medium + "'/>";
                        gameContent = gameContent + "</a>";
                        gameContent = gameContent + "<p class='sp-caption'><label id='game-title'>" + rec.title + "</label>";
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
        //alert(gameData.Title);
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
    $(document).on("click", "a.OtherCatGame-link", function (e) {
    //$("a.OtherCatGame-link").click(function (e) {
        var selGameURL = $(this).attr("href");
        var selGameLongDesc = $(this).find('div.longDescription').html();
        var selGameCat = $(this).find('p.game-category').text();
        var selGameTitle = selGameCat.split(',')[1];
        
        var selectedGame = {
            "URL": selGameURL,
            "Category": selGameCat.split(',')[0],
            "Title": selGameTitle,
            "LongDescription": selGameLongDesc
        };
        sessionStorage.setItem("selectedGame", JSON.stringify(selectedGame));
        e.preventDefault();
        e.preventDefault();
        window.location = "/games/gameplay";
    });


});

