var passengerData = {}, missing = false, info = null;

$(document).ready(function () {
    passportDatePicker();
    getInformation();
});

function passportDatePicker() {
    $('.passport-expiry').datetimepicker({
        format: 'DD/MM/YYYY'
    });
}

function setYearOption(type) {
    var currentYear = new Date().getFullYear();
    $('.year-form').empty();

    if (type == 'Adult') {
        var yearAdult = currentYear - 12;

        while (yearAdult >= (currentYear - 100)) //Min: 12, Max: 100
            $('.year-form').append('<option>' + yearAdult-- + '</option>');

    } else if (type == 'Child') {
        var yearChild = currentYear - 2;

        while (yearChild >= (currentYear - 11)) //Min: 2, Max: 11
            $('.year-form').append('<option>' + yearChild-- + '</option>');

    } else if (type == 'Infant') {
        var yearInfant = currentYear;

        while (yearInfant >= (currentYear - 2)) //Min: 0, Max: 2
            $('.year-form').append('<option>' + yearInfant-- + '</option>');
    } 
}

function getYearValue(el) {
    return $(el).find(":selected").text();
}

function onchangeMonth() {
    var parent = $(event.target).parents('.passenger-form');

    switch ($(event.target).val()) {
        case '02':
            parent.find('.febmonth').addClass('hide');
            if (leapYear(parseInt(parent.find('.year-form').find(":selected").text())))
                parent.find('.leapyear').removeClass('hide');
            break;
        case '04': case '06': case '09': case '11':
            parent.find('.febmonth').removeClass('hide');
            parent.find('.daymonth').addClass('hide');
            break;
        default:
            parent.find('.febmonth').removeClass('hide');
            parent.find('.daymonth').removeClass('hide');
    }
}

function onchangeYear() {
    var parent = $(event.target).parents('.passenger-form');
    if (leapYear(parseInt($(event.target).val())) && parent.find('.month-form').val() == '2')
        parent.find('.leapyear').removeClass('hide');
    else if (!leapYear(parseInt($(event.target).val())) && parent.find('.month-form').val() == '2')
        parent.find('.leapyear').addClass('hide');
}

function submitBooking() {
    var checked = true;
    $('a[data-complete="false"]').each(function () {
        $(this).parents('.account-cart').addClass('invalid');
        checked = false;
    });

    if (!checked) {
        ToastError('Please enter all passengers information');
        return;
    }

    if (missing) { //Account not full information
        ToastError('Please update your account information');
        window.scrollTo({ top: 0, behavior: 'smooth' });
        return;
    }

    $.ajax({
        url: '/Ticket/SavePassenger',
        type: 'post',
        data: {
            json : JSON.stringify(passengerData)
        },
        success: function () {
            window.location.href = '/Ticket/PaymentPaypal';
        }
    });
}

function openPassengerModal() {
    $('.passenger-form').trigger("reset");
    $('.passenger-form input').removeClass('invalid');
    $('.passenger-form select').removeClass('invalid');
    $('.passenger-form span.invalid-msg').removeClass('invalid-msg').addClass('valid-msg');
    $('.passenger-form .update-account').addClass('hide');
    $('.save-button').attr('onclick', 'savePassenger()');

    var passengerType = $(event.target).parents('.account-cart').find('.theme-account-card-number').attr('data-type');
    var title = $(event.target).parents('.account-cart').find('.theme-account-card-number').text() + ' Information';

    setYearOption(passengerType);
    $('#editPassenger .modal-title').text(title);

    if ($(event.target).parents('li').find('a').attr('data-complete') == 'true') {
        var key = Object.keys(passengerData);

        for (var i = 0; i < key.length; ++i) {
            if ($(event.target).parents('li').find('a').attr('data-passenger') == key[i]) {
                //Set information passenger for modal
                $('select[name="Gender"]').val(passengerData[key[i]][1].value);
                $('.passenger-form input[name="FirstName"]').val(passengerData[key[i]][2].value);
                $('.passenger-form input[name="LastName"]').val(passengerData[key[i]][3].value);
                $('.passenger-form input[name="IDCardOrPassport"]').val(passengerData[key[i]][7].value);
                $('.passenger-form input[name="DateIssueOrExpiry"]').val(passengerData[key[i]][8].value);
                $('select[name="Nationality"]').val(passengerData[key[i]][9].value);
                $('select[name="Nationality"]').trigger("change");
                $('select[name="City"]').val(passengerData[key[i]][10].value);
                $('select[name="City"]').trigger("change");

                var dob = passengerData[key[i]][6].value.split('/');
                $('.passenger-form .day-form').val(dob[0]);
                $('.passenger-form .month-form').val(dob[1]);
                $('.passenger-form .year-form').val(dob[2]);
            }
        }
    } else {
        $('.passenger-form .day-form').val(' ');
        $('.passenger-form .month-form').val(' ');
        $('.passenger-form .year-form').val(' ');
    }
    
    $('#editPassenger').modal('show');
    setTimeout(function () {
        $('.passenger-form input[name="FirstName"]').focus();
    }, 500);
}

function savePassenger(update, edit) {
    var checked = true;
    $('input[name="Phone"]').parent().find('span').text('Phone is required');
    $('input[name="FirstName"]').parent().find('span').text('First Name is required');
    $('input[name="LastName"]').parent().find('span').text('Last Name is required');

    if (!validatePassenger(update)) return;

    //Check Phone format
    if (!isNumeric($('input[name="Phone"]').val()) && update == 'update') {
        $('input[name="Phone"]').addClass("invalid");
        $('input[name="Phone"]').parent().find('span').removeClass('valid-msg');
        $('input[name="Phone"]').parent().find('span').addClass('invalid-msg');
        $('input[name="Phone"]').parent().find('span').text('Invalid Phone');
        checked = false;
    } else {
        $('input[name="Phone"]').removeClass("invalid");
        $('input[name="Phone"]').parent().find('span').addClass('valid-msg');
        $('input[name="Phone"]').parent().find('span').removeClass('invalid-msg');
    }

    //Check Lastname format
    if (!validateName($('input[name="LastName"]').val())) {
        $('input[name="LastName"]').addClass("invalid");
        $('input[name="LastName"]').parent().find('span').removeClass('valid-msg');
        $('input[name="LastName"]').parent().find('span').addClass('invalid-msg');
        $('input[name="LastName"]').parent().find('span').text('Invalid Last Name');
        checked = false;
    } else {
        $('input[name="LastName"]').removeClass("invalid");
        $('input[name="LastName"]').parent().find('span').addClass('valid-msg');
        $('input[name="LastName"]').parent().find('span').removeClass('invalid-msg');
    }

    //Check First Name format
    if (!validateName($('input[name="FirstName"]').val())) {
        $('input[name="FirstName"]').addClass("invalid");
        $('input[name="FirstName"]').parent().find('span').removeClass('valid-msg');
        $('input[name="FirstName"]').parent().find('span').addClass('invalid-msg');
        $('input[name="FirstName"]').parent().find('span').text('Invalid First Name');
        checked = false;
    } else {
        $('input[name="FirstName"]').removeClass("invalid");
        $('input[name="FirstName"]').parent().find('span').addClass('valid-msg');
        $('input[name="FirstName"]').parent().find('span').removeClass('invalid-msg');
    }

    if (!checked) return;

    var passenger = $('#editPassenger .modal-title').text().split('Information')[0].replace(' ', '').trim();
    $('.dob').val($('.day-form').val() + '/' + $('.month-form').val() + '/' + $('.year-form').val());
    $('.passenger-id').val(passenger);
    var dataForm = $('.passenger-form').serializeArray();
    passengerData[dataForm[0].value] = dataForm;

    if (update == 'update') {
        $.ajax({
            url: '/Account/Update',
            type: 'post',
            data: dataForm,
            success: function () {
                $('.missing-info').addClass('hide');
                ToastSuccess('Update account information successfully');
                $('#editPassenger').modal('hide');
                $('.account-name span').text(dataForm[3].value);
                missing = false;
                getInformation();
                if(edit == 'edit')
                    $('a[data-passenger="Adult1"]').parents('.account-cart').find('.theme-account-card-name p')
                        .text(dataForm[2].value + ' ' + dataForm[3].value);
            }
        });
        return;
    }

    $('a[data-passenger="' + passenger + '"]').attr('data-complete', 'true');
    $('a[data-passenger="' + passenger + '"]').parents('.account-cart').find('.theme-account-card-name p')
        .text(dataForm[2].value + ' ' + dataForm[3].value);
    $('a[data-passenger="' + passenger + '"]').parents('.account-cart').removeClass('invalid');
    $('#editPassenger').modal('hide');
    ToastSuccess('Save passenger information successfully');
}

function validatePassenger(update) {
    var index = 0;
    var checked = true;

    if (update == 'update') {
        $('.passenger-form input[type="text"]').not('input[name="IDCardOrPassport"], input[name="DateIssueOrExpiry"]').each(function () {
            if ($(this).val().trim() == "") {
                if (index++ == 0)
                    $(this).focus();

                $(this).addClass("invalid");
                $(this).parent().find('span').removeClass('valid-msg');
                $(this).parent().find('span').addClass('invalid-msg');
                checked = false;
            } else {
                $(this).removeClass("invalid");
                $(this).parent().find('span').addClass('valid-msg');
                $(this).parent().find('span').removeClass('invalid-msg');
            }
        });
    } else {
        $('.passenger-form input[type="text"]').not('input[name="IDCardOrPassport"], input[name="DateIssueOrExpiry"], input[name="Address"], input[name="Phone"]').each(function () {
            if ($(this).val().trim() == "") {
                if (index++ == 0)
                    $(this).focus();

                $(this).addClass("invalid");
                $(this).parent().find('span').removeClass('valid-msg');
                $(this).parent().find('span').addClass('invalid-msg');
                checked = false;
            } else {
                $(this).removeClass("invalid");
                $(this).parent().find('span').addClass('valid-msg');
                $(this).parent().find('span').removeClass('invalid-msg');
            }
        });
    }

    $('.passenger-form select').not('select[name="Nationality"], select[name="City"]').each(function () {
        if ($(this).val() == '-1' || $(this).val() == ' ' || $(this).val() == null) {
            $(this).addClass("invalid");
            $(this).parents('.item').find('span').removeClass('valid-msg');
            $(this).parents('.item').find('span').addClass('invalid-msg');
            checked = false;
        } else {
            $(this).removeClass("invalid");
            $(this).parents('.item').find('span').addClass('valid-msg');
            $(this).parents('.item').find('span').removeClass('invalid-msg');
        }
    });

    if ($('.day-form').hasClass('invalid')) {
        $('.day-form').parents('.item').find('span').removeClass('valid-msg');
        $('.day-form').parents('.item').find('span').addClass('invalid-msg');
        checked = false;
    }
    if ($('.month-form').hasClass('invalid')) {
        $('.month-form').parents('.item').find('span').removeClass('valid-msg');
        $('.month-form').parents('.item').find('span').addClass('invalid-msg');
        checked = false;
    }
    if ($('.year-form').hasClass('invalid')) {
        $('.year-form').parents('.item').find('span').removeClass('valid-msg');
        $('.year-form').parents('.item').find('span').addClass('invalid-msg');
        checked = false;
    }

    return checked;
}

function getInformation() {
    $.ajax({
        url: '/Account/GetInformation',
        type: 'post',
        success: function (response) {
            var data = JSON.parse(response);

            if (data.msg == 'success') {
                info = JSON.parse(data.info);

                if (info.Address == null || info.Address == '')
                    missing = true;
                else if (info.Birthday == null || info.Birthday == '')
                    missing = true;
                else if (info.FirstName == null || info.FirstName == '')
                    missing = true;
                else if (info.Gender == null || info.Gender == '')
                    missing = true;
                else if (info.Phone == null || info.Phone == '')
                    missing = true;

                if (missing) $('.missing-info').removeClass('hide');
            }
        }
    });
}

function openUpdateAccountModal(edit) {
    $('.passenger-form').trigger("reset");
    $('.passenger-form input').removeClass('invalid');
    $('.passenger-form select').removeClass('invalid');
    $('.passenger-form span.invalid-msg').removeClass('invalid-msg').addClass('valid-msg');
    $('.passenger-form .update-account').removeClass('hide');

    if(edit == 'edit')
        $('.save-button').attr('onclick', 'savePassenger("update","edit")');
    else $('.save-button').attr('onclick', 'savePassenger("update")');

    setYearOption('Adult');
    $('#editPassenger .modal-title').text('Update Account Information');

    //Set value form
    $('.passenger-form input[name="FirstName"]').val(info.FirstName);
    $('.passenger-form input[name="LastName"]').val(info.LastName);
    $('.passenger-form select[name="Gender"]').val(info.Gender);
    $('.passenger-form input[name="IDCardOrPassport"]').val(info.IDCardOrPassport);
    $('.passenger-form input[name="DateIssueOrExpiry"]').val(info.Expire);
    $('.passenger-form input[name="Phone"]').val(info.Phone);
    $('.passenger-form input[name="Address"]').val(info.Address);

    if (info.PlaceIssue != null && info.PlaceIssue != '') {
        var country = info.PlaceIssue.split(',');
        $('select[name="Nationality"]').val(country[0].trim());
        $('select[name="Nationality"]').trigger("change");
        $('select[name="City"]').val(country[1].trim());
        $('select[name="City"]').trigger("change");
    }

    if (info.Birthday != null && info.Birthday != '') {
        var dob = info.Birthday.split('/');
        $('.passenger-form .day-form').val(dob[0]);
        $('.passenger-form .month-form').val(dob[1]);
        $('.passenger-form .year-form').val(dob[2]);
    } else {
        $('.passenger-form .day-form').val(' ');
        $('.passenger-form .month-form').val(' ');
        $('.passenger-form .year-form').val(' ');
    }

    $('#editPassenger').modal('show');
    setTimeout(function () {
        $('.passenger-form input[name="FirstName"]').focus();
    }, 500);
}

function itsYou() {
    if (missing) {
        ToastError('Please update your account information');
        window.scrollTo({ top: 0, behavior: 'smooth' });
        return;
    }

    $('a[data-passenger="Adult1"]').attr('data-complete', 'true');
    $('a[data-passenger="Adult1"]').parents('.account-cart').find('.theme-account-card-name p')
        .text(info.FirstName + ' ' + info.LastName);
    $('a[data-passenger="Adult1"]').parents('.account-cart').removeClass('invalid');
    $('a[data-passenger="Adult1"]').attr('onclick', 'openUpdateAccountModal("edit")');
    $('.it-you').addClass('hide');
    $('.not-you').removeClass('hide');
    ToastSuccess("Get your account information successfully");

    //Remove Adult1
    delete passengerData["Adult1"];
}

function notYou() {
    $('a[data-passenger="Adult1"]').attr('data-complete', 'false');
    $('a[data-passenger="Adult1"]').parents('.account-cart').find('.theme-account-card-name p')
        .text('Passenger Name');
    $('a[data-passenger="Adult1"]').attr('onclick', 'openPassengerModal()');
    $('.it-you').removeClass('hide');
    $('.not-you').addClass('hide');
    ToastSuccess("Remove your account information successfully");
}