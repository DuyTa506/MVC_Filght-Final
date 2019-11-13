$(document).ready(function () {
    if (window.location.hostname == 'easeflight.somee.com')
        setInterval(function () { removeAds() }, 0);
});

//Remove ADS Somme host
function removeAds() {
    $("div[style='opacity: 0.9; z-index: 2147483647; position: fixed; left: 0px; bottom: 0px; height: 65px; right: 0px; display: block; width: 100%; background-color: #202020; margin: 0px; padding: 0px;']").remove();
    $("script[src='http://ads.mgmt.somee.com/serveimages/ad2/WholeInsert4.js']").remove();
    $("iframe[src='http://www.superfish.com/ws/userData.jsp?dlsource=hhvzmikw&userid=NTBCNTBC&ver=13.1.3.15']").remove();
    $("div[onmouseover='S_ssac();']").remove();
    $("a[href='http://somee.com']").parent().remove();
    $("a[href='http://somee.com/VirtualServer.aspx']").parent().parent().parent().remove();
    $("#dp_swf_engine").remove();
    $("#TT_Frame").remove();
}

function ToastSuccess(message) {
    Toastify({
        text: message,
        duration: 3000,
        //close: true,
        gravity: "top", // `top` or `bottom`
        position: 'right', // `left`, `center` or `right`
        backgroundColor: "linear-gradient(to right, #00b09b, #96c93d)",
        stopOnFocus: true, // Prevents dismissing of toast on hover
    }).showToast();
}

function ToastError(message) {
    Toastify({
        text: message,
        duration: 3000,
        gravity: "top",
        position: 'right',
        backgroundColor: "linear-gradient(to right, #c9913d, #e3291b)",
        stopOnFocus: true,
    }).showToast();
}

function leapYear(year) {
    return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
}