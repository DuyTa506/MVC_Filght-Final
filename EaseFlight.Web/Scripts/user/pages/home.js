var idAirpotDeparture, idAirportArrival, placeResult = '';

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

    $('.place-dropdown .country').on('click', function (event) {
        event.stopPropagation();
    });

    $('.place-result').focusin(function () {
        $(this).select();
    });
    $('.place-result').keyup(function () {
        $(this).removeAttr('readonly');
    });
    $('.place-result').focusout(function () {
        $(this).attr('readonly');
        $(this).val(placeResult);
    });
});

//Select Departure and Arrial dropdown
function placeSelect() {
    var selText = $(event.target).closest('li').text().trim().split('\n');
    var city = selText[0];
    var airportCode = selText[1].split('-')[0].trim();
    var id = $(event.target).closest('li').attr('id');
    var place = $(event.target).parents('.theme-search-area-section-inner').find('.place-result').attr('id')

    place == 'departure' ? idAirpotDeparture = id : idAirportArrival = id;
    placeResult = city + ' (' + airportCode + ')';
    $(event.target).parents('.theme-search-area-section-inner').find('.place-result').val(placeResult);
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

function oninputPlace() {
    var word = event.target.value.toLowerCase();

    $(event.target).parent().find('.airport').each(function () {
        var span = $(this).find('span').not('.hightline');
        var airport = span.text();
        var parent = $(this).parent();

        if (airport.toLowerCase().indexOf(word.toLowerCase()) == -1) {
            if (span.hasClass('city-title')) {
                span.addClass('not');
            } else {
                if (parent.find('.city-title').hasClass('not'))
                    parent.addClass('hide');
            }
            span.html(airport);

        } else {
            if (span.hasClass('city-title')) 
                span.removeClass('not');

            var hightline = airport.replace(new RegExp(word, "gi"), "<span class='hightline'>$&</span>");
            span.html(hightline);
            parent.removeClass('hide');
        }
    });

    $('.place-dropdown .sub-ul li:not(.hide):last').addClass('border-bottom-none');
}