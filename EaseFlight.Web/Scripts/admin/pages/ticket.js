var passengerModalId = null, minyear = 0, maxyear = 0;

$(document).ready(function () {
    //Init datatable
    $("#ticketTable").DataTable({
        "order": [[0, "desc"]]
    });

    //Add context Menu ticket table
    $.contextMenu({
        selector: '.tr-ticket',
        callback: function (key, options) {

            if (key == 'passenger') { //Passenger detail
                openPassengerDeatilModal($(this).find('.ticketId').text());
            }else if(key == 'delete') { //Delete option
            
            }
        },
        items: {
            "passenger": { name: "Passenger detail", icon: "fas fa-users" },
            "flight": { name: "Flight detail", icon: "fas fa-fighter-jet" },
            "sep1": "---------",
            "return": { name: "Return ticket", icon: "fas fa-undo" },
            "cancel": { name: "Cancel ticket", icon: "fas fa-ban" },
            "delete": { name: "Delete", icon: "delete" }
        }
    }); 

    //Add context Menu passenger detail table
    $.contextMenu({
        selector: '.tr-passenger',
        callback: function (key, options) {

            if (key == 'edit') { //Edit option
                openEditPassengerModal($(this));
            }
        },
        items: {
            "edit": { name: "Edit", icon: "edit" },
        }
    });

    //On hide modal edit passenger
    $('#editPassengerModal').on('hidden.bs.modal', function () {
        $(passengerModalId).modal('show');
    });
})

function openPassengerDeatilModal(id) {
    passengerModalId = '#passengerDetailModal' + id;

    $(passengerModalId).modal('show');
}

function openEditPassengerModal(parent) {
    var form = $('.passenger-form');
    var type = $(parent).find('.type').text();
    var title = 0;
    var dob = $(parent).find('.birthday').text().split('/');

    if ($(parent).find('.title').text() == 'Ms/Mrs')
        title = 1;

    form.trigger('reset');
    form.find('select[name="title"]').val(title);
    form.find('input[name="firstname"]').val($(parent).find('.firstname').text());
    form.find('input[name="lastname"]').val($(parent).find('.lastname').text());
    form.find('input[name="passport"]').val($(parent).find('.passport').text());
    form.find('select[name="nationality"]').val($(parent).find('.nationality').text());
    form.find('input[name="passengertype"]').val(type)
    form.find('input[name="passengerId"]').val($(parent).find('.id').text())
    
    $('select[name="title"]').trigger('change');
    $('select[name="nationality"]').trigger('change');
   
    setTimeout(function () {
        form.find('select[name="city"]').val($(parent).find('.city').text());
        form.find('.day-form').val(dob[0]);
        form.find('.month-form').val(dob[1]);
        form.find('.year-form').val(dob[2]);
        $('select[name="city"]').trigger('change');
        $('.day-form').trigger('change');
        $('.month-form').trigger('change');
        $('.year-form').trigger('change');
    }, 100);

    //Create Datetimepicker expire passport
    var expiredate = $(parent).find('.expiry').text();
    if (expiredate == '') expiredate = moment().format("DD/MM/YYYY");
    $('input[name="expiry"]').daterangepicker({
        timePicker: false,
        singleDatePicker: true,
        showDropdowns: true,
        startDate: expiredate,
        locale: {
            format: 'DD/MM/YYYY'
        }
    });

    //Create Datetimepicker birtday passport
    setYearOption(type);
    var mindate = '01/01/' + minyear;
    var maxdate = '31/12/' + maxyear;
    var defaultdate = $(parent).find('.birthday').text();
    $('input[name="birthday"]').daterangepicker({
        timePicker: false,
        singleDatePicker: true,
        showDropdowns: true,
        minYear: minyear,
        maxYear: maxyear,
        minDate: mindate,
        maxDate: maxdate,
        startDate: defaultdate,
        locale: {
            format: 'DD/MM/YYYY'
        }
    });
    
    $(passengerModalId).modal('hide');
    $('#editPassengerModal').modal('show');
}

function setYearOption(type) {
    var currentYear = new Date().getFullYear();

    if (type == 'Adult') {
        maxyear = currentYear - 12;
        minyear = currentYear - 100;
    } else if (type == 'Child') {
        maxyear = currentYear - 2;
        minyear = currentYear - 11;
    } else if (type == 'Infant') {
        maxyear = currentYear;
        minyear = currentYear - 2;
    }
}

function savePassenger() {
    var checked = true;

    if ($('input[name="firstname"]').val().trim() == '') {
        $('.msg-firstname').removeClass('msg-valid').addClass('msg-invalid');
        $('input[name="firstname"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-firstname').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="firstname"]').removeClass('is-invalid');
    }

    if ($('input[name="lastname"]').val().trim() == '') {
        $('.msg-lastname').removeClass('msg-valid').addClass('msg-invalid');
        $('input[name="lastname"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-lastname').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="lastname"]').removeClass('is-invalid');
    }

    if ($('input[name="birthday"]').val().trim() == '') {
        $('.msg-birthday').removeClass('msg-valid').addClass('msg-invalid');
        $('input[name="birthday"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-birthday').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="birthday"]').removeClass('is-invalid');
    }

    $.ajax({
        url: '/Admin/Ticket/UpdatePassenger',
        type: 'post',
        data: $('.passenger-form').serialize(),
        success: function (response) {

        }
    });

    if (!checked) return;
}