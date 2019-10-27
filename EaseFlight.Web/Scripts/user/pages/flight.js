var pageDepart = 10, pageReturn = 10, loadMoreDepart = true;

$(document).ready(function () {
    $('#main-nav').addClass('navbar-theme-abs navbar-theme-transparent navbar-theme-border');

    //Add events One Way/Round Trip option
    addEventTripSearch();

    //Add result flight search
    getSearchValue();
    getFlightSearch();
})

function returnTab() {
    $('.div-return').removeClass('col-md-4 disabled-tab cursor-pointer');
    $('.div-return').addClass('col-md-8');
    $('.div-oneway').removeClass('col-md-8');
    $('.div-oneway').addClass('col-md-4 disabled-tab cursor-pointer');
    $('.div-return .sort-result').removeClass('hide');
    $('.div-oneway .sort-result').addClass('hide');
    $('.div-return .load-more').removeClass('hide');
    $('.div-oneway .load-more').addClass('hide');

    $('.div-return').attr("style", "");
    var interval = setInterval(function () {
        $('.div-oneway').css({ "height": $('.div-return').height(), "overflow": "hidden" });
    }, 1);

    setTimeout(function () {
        clearInterval(interval);
    }, 300);
}

function onewayTab() {
    if ($('.div-return').hasClass('hide')) return;

    $('.div-return').addClass('col-md-4 disabled-tab cursor-pointer');
    $('.div-return').removeClass('col-md-8');
    $('.div-oneway').addClass('col-md-8');
    $('.div-oneway').removeClass('col-md-4 disabled-tab cursor-pointer');
    $('.div-return .sort-result').addClass('hide');
    $('.div-oneway .sort-result').removeClass('hide');
    $('.div-return .load-more').addClass('hide');
    $('.div-oneway .load-more').removeClass('hide');

    $('.div-oneway').attr("style", "");
    var interval = setInterval(function () {
        $('.div-return').css({ "height": $('.div-oneway').height(), "overflow": "hidden" });
    }, 1);

    setTimeout(function () {
        clearInterval(interval);
    }, 300);
}

function getSearchValue() {
    var formData = new FormData();
    var parameters = window.location.search.slice(1).split('&');
    if (parameters.length != 9) return;

    for (var i = 0; i < parameters.length; ++i) {
        var param = parameters[i].split('=');
        formData.append(param[0], param[1]);
    }
        
    $.ajax({
        url: '/Flight/GetSearchValue',
        type: 'post',
        data: formData,
        cache: false,
        processData: false,
        contentType: false,
        success: function (response) {
            var data = JSON.parse(response);
            var seatclass = $('.seat-dropdown li.' + data.seat);
            var passenger = $('.passenger-number');

            //Seat departure and airrival
            setPlaceValue($('.ul-departure').find('li#' + data.departure));
            setPlaceValue($('.ul-arrival').find('li#' + data.arrival));

            //Set seat class
            $('.seat-selected').addClass('hide');
            seatclass.find('i').removeClass('hide');
            $('.span-seat').text(seatclass.text());
            $('.span-seat').attr('data-seat', seatclass.attr('onclick').split('(')[1].slice(0, 1));

            //Set passenger
            $(passenger[0]).text(data.adult);
            $(passenger[1]).text(data.child);
            $(passenger[2]).text(data.infant);
            $('.passenger-type').val(data.adult + ' Adult, ' + data.child + ' Child, ' + data.infant + ' Infant');

            //Set date
            $('.datePickerStart').data("DateTimePicker").date(new Date(data.date));
           
            //Set round trip
            if (data.roundTrip) {
                $('#flight-option-1').parent('label').removeClass('active');
                $('#flight-option-1').removeAttr('checked');
                $('#flight-option-2').attr('checked');
                $('#flight-option-2').parent('label').addClass('active');
                $('.place').removeClass('col-md-4-5');
                $('.place').addClass('col-md-3-5');
                $('.time-place').removeClass('col-md-6-5');
                $('.time-place').addClass('col-md-7-5');
                $('.checkin').removeClass('col-md-4');
                $('.checkin').addClass('col-md-3');
                $('.checkout').removeClass('hide');
                $('.seatclass').removeClass('col-md-4');
                $('.seatclass').addClass('col-md-3');
                $('.passenger').removeClass('col-md-4');
                $('.passenger').addClass('col-md-3');
                $('.datePickerEnd').data("DateTimePicker").date(new Date(data.returnDate));
            }
        }
    });
}

function setPlaceValue(el){
    var selText = el.text().trim().split('\n');
    var city = selText[0];
    var airportCode = selText[selText.length - 1].split('-')[0].trim();
    var place = el.parents('.theme-search-area-section-inner').find('.place-result').attr('id');
    var placeResult = city + ' (' + airportCode + ')';

    el.parents('.theme-search-area-section-inner').find('.place-result').val(placeResult);

    if (place == 'departure') {
        placeDeparture = placeResult;
        idAirpotDeparture = el.attr('id');
    } else {
        placeArrival = placeResult;
        idAirportArrival = el.attr('id');
        $('.flight-to').text('Flights to ' + city);
    }
}

function getFlightSearch() {
    var formData = new FormData();
    var parameters = window.location.search.slice(1).split('&');
    var percent = 1;
    var loading = setInterval(function () {
        if (percent <= 100) {
            $('.progress-bar').css("width", percent + '%');
            $('.progress-value').text(percent++ + '%');
        }
    }, 20);

    if (parameters.length != 9)
        setTimeout(function () {
            $('.loadingbar').addClass('hide');
            clearInterval(loading);
            return;
        }, 1000);

    for (var i = 0; i < parameters.length; ++i) {
        var param = parameters[i].split('=');

        //Check if param not value, except "return" param
        if (i != 3 && param.length == 1)
            setTimeout(function () {
                $('.loadingbar').addClass('hide');
                return;
            }, 1000);

        formData.append(param[0], param[1]);
    }

    formData.append("pageDepart", pageDepart);
    formData.append("pageReturn", pageReturn);

    $.ajax({
        url: '/Flight/Find',
        type: 'post',
        data: formData,
        cache: false,
        processData: false,
        contentType: false,
        success: function (response) {
            setTimeout(function () {
                $('.loadingbar').addClass('hide');
                var data = JSON.parse(response);

                if (data.type == "error")
                    $('.resultDiv').html('<h3>Nothing</h3>');
                else $('.resultDiv').html(data.result);

                if ($('#flightcount').val()) {
                    var city = $('.flight-to').text().split('to')[1];
                    $('.flight-to').text($('#flightcount').val() + ' Flights to ' + city);
                }

                var pageSize = parseInt($('.load-depart').attr("data-page"));
                if (pageDepart >= pageSize) $('.load-depart').remove();
                var pageSize = parseInt($('.load-return').attr("data-page"));
                if (pageReturn >= pageSize) $('.load-return').remove();

                if (loadMoreDepart)
                    onewayTab();
                else returnTab();
            }, 1000);
        }
    });
}

function loadMore(event) {
    if (event == 0) {
        $('.load-depart').html('<div class="lds-ellipsis"><div></div><div></div><div></div><div></div></div>');
        loadMoreDepart = true;
        pageDepart += 10;
    }
    else {
        $('.load-return').html('<div class="lds-ellipsis"><div></div><div></div><div></div><div></div></div>');
        loadMoreDepart = false;
        pageReturn += 10;
    }

    getFlightSearch();
}