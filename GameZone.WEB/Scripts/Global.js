var gamezoneApp = angular.module('gamezoneApp', []);
//var apiURL = "http://localhost:62101";
var apiURL = "http://localhost:17683";

//Validate Form Controls
function isFormValid(containerId) {
    var isInValid = 0;
    $('#' + containerId + ' input').each(function () {
        try {
            var id = $(this).attr('id'); if (id.indexOf('txt') >= 0) isInValid += (_isControlValid(id)) ? 0 : 1;
            //document.writeln("id: " + id + " >isvalid: " + _isControlValid(id));
        } catch (e) { }
    });
    $('#' + containerId + ' textarea').each(function () {
        try {
            var id = $(this).attr('id'); if (id.indexOf('txt') >= 0) isInValid += (_isControlValid(id)) ? 0 : 1;
            //document.writeln("id: " + id + " >isvalid: " + _isControlValid(id));
        } catch (e) { }
    });
    $('#' + containerId + ' select').each(function () {
        try { var id = $(this).attr('id'); if (id.indexOf('sel') >= 0) isInValid += (_isControlValid(id)) ? 0 : 1; } catch (e) { }
    });
    //alert(containerID + " => nFailed: " + isInValid);
    if (isInValid > 0) alertWarning('fill in the compulsory fields...');
    return isInValid == 0 ? true : false;
}

//Get Angular App Variable
function getNgScope() {
    var appElement = document.querySelector('[ng-app=gamezoneApp]');
    return angular.element(appElement).scope();
}

//Partially Harsh Parameter Value
function partialHarsh(val) {
    var valArray = val.split("").reverse(), retVal = "";
    //Arabicize String (Read String from back)
    for (var i = 0; i < valArray.length; i++) {
        retVal += valArray[i] + "%0$";
    }
    return retVal;
};