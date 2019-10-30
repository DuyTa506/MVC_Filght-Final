$(document).ready(function () {
    $(window).bind("beforeunload", function (e) {
        return "You have some unsaved changes";
    });

    $('.passport-expiry').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    setYearOption();
});

function setYearOption() {
    var currentYear = new Date().getFullYear();

    //Set adult
    var yearAdult = currentYear - 12;
    while (yearAdult >= (currentYear - 100)) //Min: 12, Max: 100
        $('.year-adult').append('<option>' + yearAdult-- + '</option>');

    //Set child
    var yearChild = currentYear - 2;
    while (yearChild >= (currentYear - 11)) //Min: 2, Max: 11
        $('.year-child').append('<option>' + yearChild-- + '</option>');

    //Set Infant
    var yearInfant = currentYear;
    while (yearInfant >= (currentYear - 2)) //Min: 0, Max: 2
        $('.year-infant').append('<option>' + yearInfant-- + '</option>');
}

function getYearValue(el) {
    return $(el).find(":selected").text();
}

function onchangeMonth() {
    var parent = $(event.target).parents('.theme-payment-page-sections-item');

    switch ($(event.target).val()) {
        case '2':
            parent.find('.febmonth').addClass('hide');
            if (leapYear(parseInt(parent.find('.year-form').find(":selected").text())))
                parent.find('.leapyear').removeClass('hide');
            break;
        case '4': case '6': case '9': case '11':
            parent.find('.febmonth').removeClass('hide');
            parent.find('.daymonth').addClass('hide');
            break;
        default:
            parent.find('.febmonth').removeClass('hide');
            parent.find('.daymonth').removeClass('hide');
    }
}

function onchangeYear() {
    var parent = $(event.target).parents('.theme-payment-page-sections-item');
    if (leapYear(parseInt($(event.target).val())) && parent.find('.monthform').val() == '2')
        parent.find('.leapyear').removeClass('hide');
    else if (!leapYear(parseInt($(event.target).val())) && parent.find('.monthform').val() == '2')
        parent.find('.leapyear').addClass('hide');
}

function submitBooking() {
    $.ajax({
        url: '/Ticket/SavePassenger',
        type: 'post',
        success: function () {

        }
    });

    window.location.href = '/Ticket/PaymentPaypal';
}