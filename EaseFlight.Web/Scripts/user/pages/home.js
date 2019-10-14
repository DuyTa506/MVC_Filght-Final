$(document).ready(function () {
    //Remove css in Layout
    $('#mainFooter').remove();
    $('#main-nav').addClass('navbar-theme-abs navbar-theme-transparent navbar-theme-border');
    $('.theme-copyright').remove();

    //Add events One Way/Round Trip option
    addEventTripSearch();

    //Check reset password success
    var href = window.location.href.split('#');
    if (href[1] == "login") {
        openLoginModal();
        $('.error').addClass('alert alert-success').html('Reset password successfully, please login!')
    }
});

//Select Departure and Arrial dropdown
function placeSelect() {
    var selText = $(event.target).closest('li').text().trim().split('\n');
    var city = selText[0];
    var airportCode = selText[1].split('-')[0].trim();
    $(event.target).parents('.theme-search-area-section-inner').find('.place-result').html(city + ' (' + airportCode + ')');
}