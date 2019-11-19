var passengerModalId = null, minyear = 0, maxyear = 0;

$(document).ready(function () {
    $('.ticket-menu').addClass('menu-active');

    //Init datatable
    $("#ticketTable").DataTable({
        "order": [[0, "desc"]]
    });

    //Add context Menu ticket table
    $.contextMenu({
        selector: '.tr-ticket',
        callback: function (key, options) {
            if (key == 'passenger') { //Passenger detail
                $.contextMenu('destroy', '.tr-passenger.can-edit');
                if ($(this).find('.ticket-status').text() == 'Success')
                    //Add context Menu passenger detail table
                    $.contextMenu({
                        selector: '.tr-passenger.can-edit',
                        callback: function (key, options) {

                            if (key == 'edit') { //Edit option
                                openEditPassengerModal($(this));
                            }
                        },
                        items: {
                            "edit": { name: "Edit", icon: "edit" },
                        }
                    });

                openPassengerDeatilModal($(this).find('.ticketId').text());
            } else if (key == 'return') { //Return ticket option

                if ($(this).find('.ticket-status').text() == 'Return')
                    ToastError("You can't return a ticket had returned");
                else if ($(this).find('.ticket-status').text() == 'Cancel')
                    ToastError("You can't return a ticket had canceled");
                else {
                    $('#confirm-modal .modal-title').text('Confirm Return Ticket');
                    $('#confirm-modal .areyousure').text('Are you sure to return this ticket? This procedure is irreversible.');
                    $('#confirm-modal .confirm-button').text('Return Ticket');
                    $('#confirm-modal .confirm-button').attr('onclick', 'returnTicket(' + $(this).find('.ticketId').text() + ')');
                    $('#confirm-modal').modal('show');
                }
            } else if (key == 'cancel') { //Cancel ticket option

                if ($(this).find('.ticket-status').text() == 'Cancel')
                    ToastError("You can't cancel a ticket had canceled");
                else {
                    $('#confirm-modal .modal-title').text('Confirm Cancel Ticket');
                    $('#confirm-modal .areyousure').text('Are you sure to cancel this ticket? This procedure is irreversible.');
                    $('#confirm-modal .confirm-button').text('Cancel Ticket');
                    $('#confirm-modal .confirm-button').attr('onclick', 'cancelTicket(' + $(this).find('.ticketId').text() + ')');
                    $('#confirm-modal').modal('show');
                }
            } else if (key == 'flight') {
                var flightModalId = '#flightModal' + $(this).find('.ticketId').text();
                $(flightModalId).modal('show');
            }
        },
        items: {
            "passenger": { name: "Passenger detail", icon: "fas fa-users" },
            "flight": { name: "Flight detail", icon: "fas fa-fighter-jet" },
            "sep1": "---------",
            "return": { name: "Return ticket", icon: "fas fa-undo" },
            "cancel": { name: "Cancel ticket", icon: "fas fa-ban" }
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
    $(form).find('span.msg-invalid').removeClass('msg-invalid').addClass('msg-valid');
    $(form).find('input').removeClass('is-invalid');
    $(form).find('select').removeClass('is-invalid');
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

    if (!checked) return;

    var data = $('.passenger-form').serializeArray();

    $.ajax({
        url: '/Admin/Ticket/UpdatePassenger',
        type: 'post',
        data: $('.passenger-form').serialize(),
        success: function () {
            var title = 'Mr';
            if (data[1].value == '1') title = 'Ms/Mrs';

            $(passengerModalId).find('td.title').text(title);
            $(passengerModalId).find('td.firstname').text(data[2].value);
            $(passengerModalId).find('td.lastname').text(data[3].value);
            $(passengerModalId).find('td.birthday').text(data[4].value);
            $(passengerModalId).find('td.passport').text(data[5].value);
            $(passengerModalId).find('td.expiry').text(data[6].value);
            $(passengerModalId).find('td.nationality').text(data[7].value);
            $(passengerModalId).find('td.city').text(data[8].value);

            $('#editPassengerModal').modal('hide');
            ToastSuccess('Update passenger detail successfully');
        }
    });
}

function returnTicket(id) {
    var parent = $('.tr-ticket .ticketId:contains("' + id + '")').parents('.tr-ticket');

    if ($(parent).find('.ticketId').attr('data-return') == 'False') {
        $('#confirm-modal').modal('hide');
        ToastError('You can only return ticket at least 1 month from departure');
        return;
    }

    $(parent).find('.ticket-status').html('<div class="lds-ellipsis"><div></div><div></div><div></div><div></div></div>');
    $('#confirm-modal').modal('hide');

    $.ajax({
        url: '/Ticket/RefundTicket',
        data: {
            ticketId: id,
            admin: "admin"
        },
        type: 'post',
        success: function (response) {
            var data = JSON.parse(response);

            if (data.type == 'success') {
                $(parent).find('.ticket-status').html('Return');
                $(parent).find('.ticket-status').removeClass('success-status').addClass('return-status');
                $(parent).find('.description').text('Refund ' + data.refund);
                ToastSuccess('Refund ticket successfully');
            }
        }
    });
}

function cancelTicket(id) {
    var parent = $('.tr-ticket .ticketId:contains("' + id + '")').parents('.tr-ticket');
    $('#confirm-modal').modal('hide');

    $.ajax({
        url: '/Admin/Ticket/CancelTicket',
        data: {
            ticketId: id
        },
        type: 'post',
        success: function (response) {
            var data = JSON.parse(response);

            if (data.type == 'success') {
                $(parent).find('.ticket-status').text('Cancel');
                $(parent).find('.ticket-status').removeClass('success-status return-status').addClass('cancel-status');
                ToastSuccess('Cancel ticket successfully');
            }
        }
    });
}