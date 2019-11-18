$(document).ready(function () {
    $('.flight-menu').addClass('menu-active');

    //Init datatable
    $("#flightTable").DataTable({
        "order": [[0, "desc"]]
    });

    ////Add create button
    var parent = $('.dataTables_filter').parent();
    parent.append('<button title="Create flight" onclick="openAddModal()" type="button" class="btn btn-block btn-success btn-sm btn-create"><i class="fas fa-plus"></i></button>');

    //Add context Menu
    $.contextMenu({
        selector: '.tr-flight',
        callback: function (key, options) {

            if (key == 'status') { //Change status option
                openStatusModal($(this));
            } else if (key == 'edit') { //Edit option
                editFlight($(this));
            } else if (key == 'delete') { //Delete option
                $('.confirm-button').attr('onclick', 'deleteFlight("' + $(this).find('.flightId').text() + '")');
                $('#confirm-modal').modal('show');
            }
        },
        items: {
            "status": { name: "Change status", icon: "paste"},
            "sep1": "---------",
            "edit": { name: "Edit", icon: "edit" },
            "delete": { name: "Delete", icon: "delete" }
        }
    });  

    //Select plane onchange
    $('select[name="plane"]').change(function () {
        planeSelectOnchange($(this).val());
    });

    planeSelectOnchange($('select[name="plane"]').val());

    //Create Datetimepicker
    $('input[name="flightDate"]').daterangepicker({
        timePicker: true,
        timePickerIncrement: 5,
        minDate: moment(),
        locale: {
            format: 'DD/MM/YYYY hh:mm A'
        }
    })
})

function openAddModal() {
    $('form[name="flightForm"]').attr('action', '/Admin/Flight/AddFlight');
    $('form[name="flightForm"]').trigger('reset');
    $('form[name="flightForm"] span.msg-invalid').removeClass('msg-invalid').addClass('msg-valid');
    $('form[name="flightForm"] input').removeClass('is-invalid');
    $('form[name="flightForm"] select').removeClass('is-invalid');
    $('#flightModal .modal-title').text('Add New Flight');

    $('#flightModal').modal('show');
}

function planeSelectOnchange(planeId) {
    $.ajax({
        url: '/Admin/Flight/GetAirport',
        type: 'post',
        data: {
            planeId: planeId
        },
        success: function (response) {
            var data = JSON.parse(response).airport;
            var depart = $('select[name="depart"]');
            var arrival = $('select[name="arrival"]');

            depart.empty(); arrival.empty();

            for (var i = 0; i < data.length; ++i) {
                var airport = data[i].split(':');
                depart.append('<option value="' + airport[0] + '">' + airport[1] + '</option>');
                arrival.append('<option value="' + airport[0] + '">' + airport[1] + '</option>');
            }

            depart.trigger('change');
            arrival.trigger('change');
        }
    });
}

function saveFlight() {
    var checked = true;
    var form = $('form[name="flightForm"]');

    if ($('select[name="depart"]').val() == $('select[name="arrival"]').val()) {
        $('.msg-place').removeClass('msg-valid').addClass('msg-invalid');
        $('select[name="depart"]').addClass('is-invalid');
        $('select[name="arrival"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-place').removeClass('msg-invalid').addClass('msg-valid');
        $('select[name="depart"]').removeClass('is-invalid');
        $('select[name="arrival"]').removeClass('is-invalid');
    }

    if ($('input[name="flightDate"]').val().trim() == '') {
        $('.msg-date').removeClass('msg-valid').addClass('msg-invalid');
        $('input[name="flightDate"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-date').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="flightDate"]').removeClass('is-invalid');
    }

    if (!checked) return;

    $(form).submit();
}

function editFlight(parent) {
    $('form[name="flightForm"] span.msg-invalid').removeClass('msg-invalid').addClass('msg-valid');
    $('form[name="flightForm"] input').removeClass('is-invalid');
    $('form[name="flightForm"] select').removeClass('is-invalid');
    $('#flightModal .modal-title').text('Edit Flight')
    //Set flight ID
    $('form[name="flightForm"]').find('.flightId').val($(parent).find('.flightId').text());

    //Set Plane
    $('select[name="plane"]').val($(parent).find('.plane').attr('data-value'));
    $('select[name="plane"]').trigger('change');

    //Set Depart and Arrival
    setTimeout(function () {
        $('select[name="depart"]').val($(parent).find('.depart').attr('data-value'));
        $('select[name="depart"]').trigger('change');
        $('select[name="arrival"]').val($(parent).find('.arrival').attr('data-value'));
        $('select[name="arrival"]').trigger('change');
    }, 200);

    //Set DepartDate and ArrivalDate
    var departDate = $(parent).find('.departDate').text();
    var arrivalDate = $(parent).find('.arrivalDate').text();

    $('input[name="flightDate"]').val(departDate + ' - ' + arrivalDate);
    $('input[name="flightDate"]').data('daterangepicker').setStartDate(departDate);
    $('input[name="flightDate"]').data('daterangepicker').setEndDate(arrivalDate);

    //Set Price
    setTimeout(function () {
        $('input[name="price"]').val(parseFloat($(parent).find('.price').text().split('$')[1]));
    }, 200);

    $('form[name="flightForm"]').attr('action', '/Admin/Flight/UpdateFlight');
    
    $('#flightModal').modal('show');
}

function deleteFlight(id) {
    window.location.href = '/Admin/Flight/DeleteFlight?id=' + id;
}

function openStatusModal(parent) {
    var status = $(parent).find('.td-status').text();
    var selector = '.btn-' + status.toLowerCase();

    $('.btn-status').removeClass('hide');
    $(selector).addClass('hide');
    $('#status-modal').find('input').val($(parent).find('.flightId').text());
    $('#status-modal').modal('show');
}

function changeReady() {
    var flightId = $('#status-modal').find('input').val();
    window.location.href = '/Admin/Flight/ChangeStatus?id=' + flightId + '&status=Ready'; 
}

function changeOnline() {
    var flightId = $('#status-modal').find('input').val();
    window.location.href = '/Admin/Flight/ChangeStatus?id=' + flightId + '&status=Online'; 
}

function changeDelay() {
    var flightId = $('#status-modal').find('input').val();
    window.location.href = '/Admin/Flight/ChangeStatus?id=' + flightId + '&status=Delay'; 
}