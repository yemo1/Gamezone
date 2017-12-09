
var formTitle = _Title;

gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {

    //Make Games Page Menu Active
    $("#topMenu li").removeClass("current");
    $("#gameMenu").addClass("current");

    $scope.basicObj = {};
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

    //Get ApplicationUser Data from DB
    $scope.getGameData = function (selectedCat) {
        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=2000",
            async: false,
            success: function (data) {
                var gameContent = "";

                //Empty Div
                $("#isotopeContainer").empty();

                $.each(data.Data, function (i, rec) {
                    if (rec.title != "What's My Icon?") {
                        gameContent = "<div class='col-xs-12 col-sm-8 col-md-4 isotopeSelector block " + selectedCat + "'>";
                        gameContent = gameContent + "<div class='service-wrap hovereffect panel clearfix animate' data-animate='bounceIn' data-duration='1.0s' data-delay='0.2s'>";
                        gameContent = gameContent + "<a href='" + rec.url + "' class='game-link'>";
                        gameContent = gameContent + "<p class='game-category hiddenPara'>" + selectedCat + "</p>";
                        gameContent = gameContent + "<div class='longDescription hiddenPara'>" + rec.long_description + "</div>";
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
        sessionStorage.setItem("selectedGame", JSON.stringify(selectedGame));
        e.preventDefault();
        window.location = "/games/gameplay";
    });
});

