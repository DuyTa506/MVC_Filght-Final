var idAirpotDeparture = -1, idAirportArrival = -1, placeDeparture = '', placeArrival = '', arrival = false, clickPassenger = false;

$(document).ready(function () {
    //Remove css in Layout
    if (window.location.pathname != "/Flight/Find")
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

    //Add event for Departure and Arrival Dropdown select
    $('.place-dropdown .country').on('click', function (event) {
        event.stopPropagation();
        var id = $(event.target).attr('data-target');
        $(id).collapse('toggle');
        $(event.target).find('.caret-dropdown').toggleClass('active');
    });

    $('.place-result').focusin(function () {
        $(this).removeAttr('readonly');
        $(this).select();
        $('.dropdown-menu.place-dropdown').find('.dropdown-item').removeClass('hide');
        $('span.hightline').addClass('color-none');
        $('.ul-departure').find('li#' + idAirportArrival).addClass('hide');
        $('.ul-arrival').find('li#' + idAirpotDeparture).addClass('hide');
        $('.country').removeClass('hide');
        $('.no-result').addClass('hide');
    });

    $('.place-result').focusout(function () {
        $(this).attr('readonly');
        var el = $(this);
        var prev = '';

        if (el.attr('id') == 'arrival')
            prev = placeArrival
        else prev = placeDeparture;

        setTimeout(function () {
            if (el.attr('id') == 'arrival' && prev == placeArrival)
                el.val(placeArrival);
            else if (el.attr('id') == 'departure' && prev == placeDeparture)
                el.val(placeDeparture);
        }, 500);
    });

    $('html').click(function () {
        if (!$(event.target).hasClass('passenger-type') && !$(event.target).hasClass('div-passenger-type')
            && event.target.closest(".div-passenger-type") == null) {
            $('.div-passenger-type').addClass('hide');
            clickPassenger = false;
        }
    });

    $('li.collapse').collapse('show');
    $('.ul-departure li.country').last().addClass('border-bottom-none');
    $('.ul-arrival li.country').last().addClass('border-bottom-none');

    //Reset departdate for search flight
    $('.datePickerStart._desk-h').attr('value', '');
    $('.datePickerStart._desk-h').val('');
    $('.datePickerStart._mob-h').attr('value', '');
    $('.datePickerStart._mob-h').val('');
});

//Select Departure and Arrial dropdown
function placeSelect(idAir) {
    var selText = $(event.target).closest('li').text().trim().split('\n');
    var city = selText[0];
    var airportCode = selText[selText.length - 1].split('-')[0].trim();
    var place = $(event.target).parents('.theme-search-area-section-inner').find('.place-result').attr('id');

    var placeResult = city + ' (' + airportCode + ')';
    $(event.target).parents('.theme-search-area-section-inner').find('.place-result').val(placeResult);

    if (place == 'departure') {
        placeDeparture = placeResult;
        idAirpotDeparture = idAir;
    } else {
        placeArrival = placeResult;
        idAirportArrival = idAir;
    }
}

function findFlight() {
    var option = $('.theme-search-area-options-list').find('label.active').find('input').attr('id');
    var inputValue = $('.passenger-type').val().split(', ');
    var departureDate = "", returnDate = "";
    var roundtrip = false;

    if ($('.datePickerStart._mob-h').val() == "") {
        departureDate = $('.datePickerStart._desk-h').val();
    } else departureDate = $('.datePickerStart._mob-h').val();

    if ($('.datePickerEnd._mob-h').val() == "") {
        returnDate = $('.datePickerEnd._desk-h').val();
    } else returnDate = $('.datePickerEnd._mob-h').val();

    if (idAirportArrival == -1 || idAirpotDeparture == -1 || departureDate == ""
        || $('.span-seat').attr('data-seat') == "") {
        ToastError("Please enter full search information!");
        return;
    }

    if (option == 'flight-option-2') { //Round Trip
        if (returnDate == "") {
            ToastError("Please enter full search information!");
            return;
        }
        roundtrip = true;
    }
    
    //Redirect to Find page
    window.location.href = '/Flight/Find?departure=' + idAirpotDeparture + '&arrival=' + idAirportArrival + '&date=' + departureDate
        + '&return=' + returnDate + '&seat=' + $('.span-seat').attr('data-seat') + '&adult=' + inputValue[0].split(' ')[0]
        + '&child=' + inputValue[1].split(' ')[0] + '&infant=' + inputValue[2].split(' ')[0] + '&roundtrip=' + roundtrip;
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
    $('.ul-departure').find('li#' + idAirportArrival).addClass('hide');
    $('.ul-arrival').find('li#' + idAirpotDeparture).addClass('hide');

    $(event.target).parent().find('.country').each(function () {
        var country = $(this).attr('id');
        var lifor = $(this).parent().find('li[lifor="' + country + '"]');
        var liforHide = $(this).parent().find('li[lifor="' + country + '"].hide');

        if (liforHide.length == lifor.length)
            $(this).addClass('hide');
        else $(this).removeClass('hide');
    });

    if ($(event.target).parent().find('.country').length == $(event.target).parent().find('.country.hide').length)
        $(event.target).parent().find('.no-result').removeClass('hide').text("No results found for " + '"' + word + '"');
    else $(event.target).parent().find('.no-result').addClass('hide');

    $(event.target).parent().find('li.collapse').collapse('show');
}

function showPassenger() {
    if (clickPassenger)
        $('.div-passenger-type').addClass('hide');
    else $('.div-passenger-type').removeClass('hide');

    clickPassenger = !clickPassenger;
}

function addPassenger() {
    var parent = $(event.target).parents('.passenger-element');
    var passengerType = parent.find('.passenger-name').text();
    var value = parseInt(parent.find('.passenger-number').text()) + 1;
    var inputValue = $('.passenger-type').val().split(', ');

    //Set value
    if (passengerType == 'Adult')
        inputValue[0] = value + ' Adult';
    else if (passengerType == 'Child')
        inputValue[1] = value + ' Child';
    else inputValue[2] = value + ' Infant';

    if (!checkPassenger(parseInt(inputValue[0].split(' ')[0]), parseInt(inputValue[1].split(' ')[0])
        , parseInt(inputValue[2].split(' ')[0])))
        return;

    parent.find('.passenger-number').text(value)
    $('.passenger-type').val(inputValue.join(', '));
}

function minusPassenger() {
    var parent = $(event.target).parents('.passenger-element');
    var passengerType = parent.find('.passenger-name').text();
    var value = parseInt(parent.find('.passenger-number').text()) - 1;
    var inputValue = $('.passenger-type').val().split(', ');

    if (value == -1 && passengerType != 'Adult') return;
    if (value == 0 && passengerType == 'Adult') return;

    

    //Set value
    if (passengerType == 'Adult') 
        inputValue[0] = value + ' Adult';
    else if (passengerType == 'Child')
        inputValue[1] = value + ' Child';
    else inputValue[2] = value + ' Infant';

    if (!checkPassenger(parseInt(inputValue[0].split(' ')[0]), parseInt(inputValue[1].split(' ')[0])
        , parseInt(inputValue[2].split(' ')[0])))
        return;

    parent.find('.passenger-number').text(value)
    $('.passenger-type').val(inputValue.join(', '));
}

function checkPassenger(adult, child, infant) {
    if ((adult + child + infant) > 10) {
        ToastError("The number of passengers is not greater than 10");
        return false;
    }

    if (adult < (child + infant)) {
        ToastError("1 child or infant must be go with 1 adult");
        return false;
    }

    return true;
}

function seatSelect(id) {
    $('.seat-selected').addClass('hide');
    $(event.target).find('i').removeClass('hide');
    $('.span-seat').text($(event.target).text());
    $('.span-seat').attr('data-seat', id);
}