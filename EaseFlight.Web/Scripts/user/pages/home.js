var idAirpotDeparture, idAirportArrival;

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
    var id = $(event.target).closest('li').attr('id');
    var place = $(event.target).parents('.theme-search-area-section-inner').find('.place-result').attr('id')

    place == 'departure'? idAirpotDeparture = id : idAirportArrival = id;
    $(event.target).parents('.theme-search-area-section-inner').find('.place-result').html(city + ' (' + airportCode + ')');
}

function findFlight() {
    var formData = new FormData();
    var option = $('.theme-search-area-options-list').find('label.active').find('input').attr('id');

    formData.append('idAirportDeparture', idAirpotDeparture);
    formData.append('idAirportArrival', idAirportArrival);
    formData.append('departureDate', $('#departureDate').val());
    formData.append('returnDate', $('#returnDate').val());
    formData.append('idSeatClass', $('.seatclass-select').val());
    if (option == 'flight-option-1')
        formData.append('roundTrip', false);
    else formData.append('roundTrip', true);

    $.ajax({
        url: '/Flight/Find',
        type: 'post',
        data: formData,
        cache: false,
        processData: false,
        contentType: false,
        success: function () {

        }
    });
}