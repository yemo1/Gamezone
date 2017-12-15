
var formTitle = _Title;

gamezoneApp.controller('gamezoneCtrlr', function ($scope, $http) {

    //Make Games Page Menu Active
    $("#menuUL li").removeClass("current");
    $("#gameMenu").addClass("current");

    $scope.basicObj = {};
    $scope.Game = {};
    $scope.CategoryGameList = [];

    var displayedItemCnt = 4;

    //Get ApplicationUser Data from DB
    $scope.getGameData = function (selectedCat) {
        //Show Loading Gif
        $("#isotopeContainer").append($("#pacMan"));

        $.ajax({
            type: "GET",
            url: apiURL + "/api/Game?gameCategory=" + selectedCat + "&gameCount=2000",
            async: true,
            success: function (data) {
                //Empty Div
                $("#isotopeContainer").empty();

                $.each(data.Data, function (i, rec) {
                    if (rec.title != "What's My Icon?") {
                        gameContent = "<div class='col-xs-12 col-sm-8 col-md-4 isotopeSelector block " + selectedCat + "'>";
                        gameContent = gameContent + "<div class='service-wrap hovereffect panel clearfix animate' data-animate='bounceIn' data-duration='1.0s' data-delay='0.2s'>";
                        gameContent = gameContent + "<a href='" + rec.url + "' class='game-link'>";
                        gameContent = gameContent + "<p class='game-category hiddenPara'>" + selectedCat + "</p>";
                        gameContent = gameContent + "<div class='longDescription hiddenPara'>" + rec.long_description + "</div>";
                        gameContent = gameContent + "<img src='" + rec.banner_medium + "' alt='" + rec.title + "' class='img-responsive' width='100%' height='186.45' max-width='294.98' max-height='187.7'/>";
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
                $.notify("Error Encountered: " + data.statusText, 'error');
            }
        });
    };


    //Populate Page with Games
    $scope.getGameData("family");


    //CLick handler of Menu Items
    $(".gameMenu").click(function (e) {
        var selCat = $(this).attr("id");
        $scope.getGameData(selCat);
    });

    //Game CLick Event Handler
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
        sessionStorage.setItem("selectedGame", JSON.stringify(selectedGame));
        window.location = "/games/gameplay";
    });


});

function GoPlay(e) {
    //alert("YES");

}