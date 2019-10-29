$(document).ready(function () {
    $(window).bind("beforeunload", function (e) {
        return "You have some unsaved changes";
    })
});