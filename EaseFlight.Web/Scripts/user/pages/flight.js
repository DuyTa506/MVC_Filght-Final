var pageDepart = 10, pageReturn = 10, loadMoreDepart = true, flightDepart = null, flightReturn = null, viewDepart = false, viewReturn = false, isViewDepart = false,
    firstTimeDepart = 0, firstTimeReturn = 0, ticketDepart = null, ticketReturn = null, priceDepart = 0, priceReturn = 0, detailDepart = null, detailReturn = null,
    flightIdDepart = '', flightIdReturn = '';
var formDataBook = new FormData();

$(document).ready(function () {
    $('#main-nav').addClass('navbar-theme-abs navbar-theme-transparent navbar-theme-border');

    //Add events One Way/Round Trip option
    addEventTripSearch();

    //Add result flight search
    getSearchValue();
    getFlightSearch();

    //Confirm modal on hide
    $('#confirmbook').on('hidden.bs.modal', function () {
        $('.confirmbook-modal .theme-search-results-item-flight-sections').empty();
        $('.confirmbook-modal .theme-search-results-item-flight-detail-items').empty();

        if (isViewDepart)
            flightDepart = null;
        else flightReturn = null;
    });

    $('#loginModal').on('hidden.bs.modal', function () {
        $('#confirmbook').css('opacity', '1');
    });
})

function returnTab() {
    if (($('.return-ticket.hide').length > 0 && $('.departure-ticket.hide').length > 0) || firstTimeReturn < 1) {
        $('.div-return').removeClass('col-md-4 disabled-tab cursor-pointer');
        $('.div-return').addClass('col-md-8');
        $('.div-oneway').removeClass('col-md-8');
        $('.div-oneway').addClass('col-md-4 disabled-tab cursor-pointer');
        $('.div-return .sort-result').removeClass('hide');
        $('.div-oneway .sort-result').addClass('hide');
        $('.div-return .load-more').removeClass('hide');
        $('.div-oneway .load-more').addClass('hide');
        $('.div-return').attr("style", "");
        $('.div-return').removeClass('ticket-review');
        $('.div-review-return').removeClass('review-fixed');

        if ($('.return-ticket.hide').length == 0 || $('.departure-ticket.hide').length == 0) 
            ++firstTimeReturn;
    }
    
    if (!viewReturn) {
        var interval = setInterval(function () {
            $('.div-oneway').css({ "height": $('.div-return').height(), "overflow": "hidden" });
        }, 1);

        setTimeout(function () {
            clearInterval(interval);
        }, 300);
    }
}

function onewayTab() {
    if ($('.div-return').hasClass('hide')) return;

    if (($('.return-ticket.hide').length > 0 && $('.departure-ticket.hide').length > 0 ) || firstTimeDepart < 1) {
        $('.div-return').addClass('col-md-4 disabled-tab cursor-pointer');
        $('.div-return').removeClass('col-md-8');
        $('.div-oneway').addClass('col-md-8');
        $('.div-oneway').removeClass('col-md-4 disabled-tab cursor-pointer');
        $('.div-return .sort-result').addClass('hide');
        $('.div-oneway .sort-result').removeClass('hide');
        $('.div-return .load-more').addClass('hide');
        $('.div-oneway .load-more').removeClass('hide');
        $('.div-oneway').attr("style", "");
        $('.div-oneway').removeClass('ticket-review');
        $('.div-review-depart').removeClass('review-fixed');

        if ($('.departure-ticket.hide').length == 0 || $('.return-ticket.hide').length == 0)
            ++firstTimeDepart;
    }

    if (!viewDepart) {
        var interval = setInterval(function () {
            $('.div-return').css({ "height": $('.div-oneway').height(), "overflow": "hidden" });
        }, 1);

        setTimeout(function () {
            clearInterval(interval);
        }, 300);
    }
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
                var isTicketDepart = $('.div-review-depart').hasClass('review-fixed');
                var isTicketReturn = $('.div-review-return').hasClass('review-fixed');

                if (data.type == "error")
                    $('.resultDiv').html('<p>There are no flights for your search!</p>');
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

                if (viewDepart) reviewDepart();
                if (viewReturn) reviewReturn();

                if (ticketDepart != null)
                    $('.departure-ticket').html(ticketDepart);

                if (ticketReturn != null)
                    $('.return-ticket').html(ticketReturn);

                if (isTicketDepart) $('.div-review-depart').addClass('review-fixed');
                if (isTicketReturn) $('.div-review-return').addClass('review-fixed');
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
function chooseFlight(flights, price) {
    var divOneway = $(event.target).parents('.div-oneway');
    var parent = $(event.target).parents('.theme-search-results-item-preview');
    var img = parent.find('img').clone();

    if ($('.div-return.hide').length > 0) {
        getBooking(flights, price, false);
        return;
    }

    if (divOneway.length == 0) { //Return
        flightReturn = parent.find('.theme-search-results-item-flight-section').clone();
        detailReturn = parent.parent().find('.theme-search-results-item-flight-details').clone();
        priceReturn = parseInt(price);
        flightIdReturn = flights;

        if ($('.result-oneway.hide').length == 0) {
            //Set airline name and photo
            var airline = parent.find('.theme-search-results-item-flight-section-airline-title').text().split('by')[1];
            $('.return-ticket .airline-ticket').text(airline);
            $('.return-ticket .airline-ticket-photo').empty();
            $('.return-ticket .airline-ticket-photo').append($(img));
            $('.return-ticket .airline-ticket-photo img').removeClass('theme-search-results-item-flight-section-airline-logo');
            $('.return-ticket .airline-ticket-photo img').removeClass('top25percent');
            $('.return-ticket .airline-ticket-photo img').removeClass('top75percent');
            $('.return-ticket .airline-ticket-photo img').addClass('_3JCi8');

            //Set Time depart and arrival
            $('.return-ticket .time-depart').html($(parent.find('.time-from').clone()));
            $('.return-ticket .time-arrival').html($(parent.find('.time-to').clone()));

            //Set price
            $('.return-ticket .price-ticket').text(parent.find('.theme-search-results-item-price-tag').text());

            //Set time
            $('.return-ticket .time-flight-ticket').text(parent.find('.time-flight').text() + ', ' + parent.parent().find('.theme-search-results-item-flight-details-info-stops').text());

            ticketReturn = $('.return-ticket').html();

            viewReturn = true;
            reviewReturn();
            window.scrollTo({ top: 52, behavior: 'smooth' });
        } else {
            isViewDepart = false;
            $('.confirmbook-modal .theme-search-results-item-flight-sections').append(flightDepart);
            $('.confirmbook-modal .theme-search-results-item-flight-sections').append(flightReturn);
            $('.confirmbook-modal .theme-search-results-item-flight-detail-items').append($(detailDepart));
            $('.confirmbook-modal .theme-search-results-item-flight-detail-items').append($(detailReturn));
            $('.confirmbook-modal .theme-search-results-item-price-tag').text('$' + (priceDepart + priceReturn) + '/pax (' + $('.span-seat').text() + ')');
            $('#confirmbook').modal('show');
        }
    } else { //Oneway
        flightDepart = parent.find('.theme-search-results-item-flight-section').clone();
        detailDepart = parent.parent().find('.theme-search-results-item-flight-details').clone();
        priceDepart = parseInt(price);
        flightIdDepart = flights;

        if ($('.result-return.hide').length == 0) {
            //Set airline name and photo
            var airline = parent.find('.theme-search-results-item-flight-section-airline-title').text().split('by')[1];
            $('.departure-ticket .airline-ticket').text(airline);
            $('.departure-ticket .airline-ticket-photo').empty();
            $('.departure-ticket .airline-ticket-photo').append($(img));
            $('.departure-ticket .airline-ticket-photo img').removeClass('theme-search-results-item-flight-section-airline-logo');
            $('.departure-ticket .airline-ticket-photo img').removeClass('top25percent');
            $('.departure-ticket .airline-ticket-photo img').removeClass('top75percent');
            $('.departure-ticket .airline-ticket-photo img').addClass('_3JCi8');
                
            //Set Time depart and arrival
            $('.departure-ticket .time-depart').html($(parent.find('.time-from').clone()));
            $('.departure-ticket .time-arrival').html($(parent.find('.time-to').clone()));

            //Set price
            $('.departure-ticket .price-ticket').text(parent.find('.theme-search-results-item-price-tag').text());

            //Set time
            $('.departure-ticket .time-flight-ticket').text(parent.find('.time-flight').text() + ', ' + parent.parent().find('.theme-search-results-item-flight-details-info-stops').text());

            ticketDepart = $('.departure-ticket').html();

            viewDepart = true;
            reviewDepart();
            window.scrollTo({ top: 52, behavior: 'smooth' });
        } else {
            isViewDepart = true;
            $('.confirmbook-modal .theme-search-results-item-flight-sections').append($(flightDepart));
            $('.confirmbook-modal .theme-search-results-item-flight-sections').append($(flightReturn));
            $('.confirmbook-modal .theme-search-results-item-flight-detail-items').append($(detailDepart));
            $('.confirmbook-modal .theme-search-results-item-flight-detail-items').append($(detailReturn));
            $('.confirmbook-modal .theme-search-results-item-price-tag').text('$' + (priceDepart + priceReturn) + '/pax (' + $('.span-seat').text() + ')');
            $('#confirmbook').modal('show');
        }
    }
}

function reviewDepart() {
    $('.div-oneway div.theme-search-results-sort, .div-oneway div.theme-search-results-sort-select, div.result-oneway, .load-depart').addClass('hide');
    $('.div-oneway .resutl-title').addClass('_mb-0');
    $('.departure-ticket').removeClass('hide');

    setTimeout(function () {
        returnTab();
        $('.div-oneway').removeClass('disabled-tab cursor-pointer');
        $('.div-oneway').addClass('ticket-review');
    }, 50);
}

function reviewReturn() {
    $('.div-return div.theme-search-results-sort, .div-return div.theme-search-results-sort-select, div.result-return, .load-return').addClass('hide');
    $('.div-return .resutl-title').addClass('_mb-0');
    $('.return-ticket').removeClass('hide');

    setTimeout(function () {
        onewayTab();
        $('.div-return').removeClass('disabled-tab cursor-pointer');
        $('.div-return').addClass('ticket-review');
    }, 50);
}

function changeFlight() {
    if ($(event.target).parents('div.col-md-4').hasClass('div-oneway')) { //Change departure
        $('.div-oneway div.theme-search-results-sort, .div-oneway div.theme-search-results-sort-select, div.result-oneway, .load-depart').removeClass('hide');
        $('.div-oneway .resutl-title').removeClass('_mb-0');
        $('.departure-ticket').addClass('hide');
    } else {// Change return
        $('.div-return div.theme-search-results-sort, .div-return div.theme-search-results-sort-select, div.result-return, .load-return').removeClass('hide');
        $('.div-return .resutl-title').removeClass('_mb-0');
        $('.return-ticket').addClass('hide');
    }

    firstTimeDepart = 0; viewDepart = false; firstTimeReturn = 0; viewReturn = false;
}

function getBooking(flights, price, rountrip) {
    
    var parameters = window.location.search.slice(1).split('&');
    var flightDepart = flightIdDepart;
    var flightReturn = flightIdReturn;

    for (var i = 0; i < parameters.length; ++i) {
        var param = parameters[i].split('=');
        formDataBook.append(param[0], param[1]);
    }

    if (rountrip) //Round trip
        price = priceDepart + priceReturn;
    else //One way
        flightDepart = flights;

    formDataBook.append('price', price);
    formDataBook.append('flightDepart', flightDepart);
    formDataBook.append('flightReturn', flightReturn);

    $.ajax({
        url: '/Account/AnyUserLogged',
        type: 'post',
        success: function (response) {
            var data = JSON.parse(response);

            if (data.msg == 'true')
                goBooking()
            else {
                $('#confirmbook').css('opacity', '0.8');
                openLoginModal();
                $('#redirectUrl').val('bookflag');
                setTimeout(function () {
                    ToastError('You must sign in first')
                }, 400);
            }
        }
    });
}

function goBooking() {
    $.ajax({
        url: '/Flight/Booking',
        type: 'post',
        data: formDataBook,
        cache: false,
        processData: false,
        contentType: false,
        success: function (response) {
            window.location.href = '/Flight/Booking';
        }
    });
}