
var formTitle = _Title;

function InstantiateGame() {        
        var gameData = sessionStorage.getItem("selectedGame");
        gameData = JSON.parse(gameData);        
        $("#gamePlay").attr("src", gameData.URL);
};
InstantiateGame();
$("#gamePlay").fullScreen(true);