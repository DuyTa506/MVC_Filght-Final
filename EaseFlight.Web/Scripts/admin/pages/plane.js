//$(document).ready(function () {

//    $('input[type="number"]').change(function () {
//        var firstclass = parseInt($('input[name="firstclass"]').val());
//        var business = parseInt($('input[name="business"]').val());
//        var economy = parseInt($('input[name="economy"]').val());
//        var capacity = parseInt($('select[name="planetype"] option:selected').attr('data-capacity'));

//    })

//})
$(document).ready(function () {
    //Init datatable
    $("#planeTable").DataTable({
        "order": [[0, "desc"]]
    });

    ////Add create button
    var parent = $('.dataTables_filter').parent();
    parent.append('<button title="Create plane" onclick="openAddModal()" type="button" class="btn btn-block btn-success btn-sm btn-create"><i class="fas fa-plus"></i></button>');
    
    $.contextMenu({
        selector: '.tr-plane',
        callback: function (key, options) {

            if (key == 'status') { //Change status option
                openStatusModal($(this));
            } else if (key == 'edit') { //Edit option
                editPlane($(this));
            } else if (key == 'delete') { //Delete option
                $('.confirm-button').attr('onclick', 'deletePlane("' + $(this).find('.planeId').text() + '")');
                $('#confirm-modal').modal('show');
            }
        },
        items: {
            "status": { name: "Change status", icon: "paste" },
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
    $('form[name="planeForm"]').attr('action', '/Admin/Plane/AddNewPlane');
    $('form[name="planeForm"]').trigger('reset');

    $('#planeModal').modal('show');
}

function savePlane() {
    var form = $('form[name="addform"]');
    var checked = true, index = 0;

    if ($('input[name="airline"]').val().trim() == '') {
        if (index++ == 0)
            $('input[name="airline"]').focus();
        $('.msg-airline').removeClass('msg-valid').addClass('msg-invalid');
        $('input[name="airline"]').addClass('is-invalid').removeClass('is-valid');
        checked = false;
    } else {
        $('.msg-airline').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="airline"]').removeClass('is-invalid').addClass('is-valid');
    }
    var firstclass = parseInt($('input[name="firstclass"]').val());
    var business = parseInt($('input[name="business"]').val());
    var economy = parseInt($('input[name="economy"]').val());
    var capacity = parseInt($('select[name="planetype"] option:selected').attr('data-capacity'));

    if ((firstclass + business + economy) != capacity) {
        $('.msg-class').text('Total Seat is ' + capacity);
        $('.msg-class').removeClass('msg-valid').addClass('msg-invalid');
        $('.seatcapacity').addClass('is-invalid').removeClass('is-valid');
        checked = false;
    } else {
        $('.msg-class').removeClass('msg-invalid').addClass('msg-valid');
        $('.seatcapacity').removeClass('is-invalid').addClass('is-valid');
    }

    if (firstclass > 0) {
        if ($('input[name="firstclassprice"]').val() == 0) {
            $('.msg-price').removeClass('msg-valid').addClass('msg-invalid');
            $('input[name="firstclassprice"]').addClass('is-invalid').removeClass('is-valid');
            checked = false;
        } else {
            $('.msg-price').removeClass('msg-invalid').addClass('msg-valid');
            $('input[name="firstclassprice"]').removeClass('is-invalid').addClass('is-valid');
        }
    }
    if (business > 0) {
        if ($('input[name="businessprice"]').val() == 0) {
            $('.msg-price').removeClass('msg-valid').addClass('msg-invalid');
            $('input[name="businessprice"]').addClass('is-invalid').removeClass('is-valid');
            checked = false;
        } else {
            $('.msg-price').removeClass('msg-invalid').addClass('msg-valid');
            $('input[name="businessprice"]').removeClass('is-invalid').addClass('is-valid');
        }
    }
    if (economy > 0) {
        if ($('input[name="economyprice"]').val() == 0) {
            $('.msg-price').removeClass('msg-valid').addClass('msg-invalid');
            $('input[name="economyprice"]').addClass('is-invalid').removeClass('is-valid');
            checked = false;
        } else {
            $('.msg-price').removeClass('msg-invalid').addClass('msg-valid');
            $('input[name="economyprice"]').removeClass('is-invalid').addClass('is-valid');
        }
    }

    if (!checked) {
        return;
    }


    $(form).submit();
}

function editPlane() {
    var planeid = $(event.target).parents('tr').find('.plane-id').text();
    var planetype = $(event.target).parents('tr').find('.plane-type').text();
    var planeseatmap = $(event.target).parents('tr').find('plane-seatmap-name').text();
    var planeairline = $(event.target).parents('tr').find('plane-airline').text();
    var planeairport = $(event.target).parents('tr').find('plane-airport').text();
    var planeseatclass = $(event.target).parents('tr').find('plane-seatclass').text();

    $('form[name="addform"] input[name="airline"]').val(planetype);
    $('form[name="addform"]').attr('action', '/Admin/Plane/UpdatePlane');

    $('#addnew').modal("show");

}

function editPlane(parent) {
    //Set plane ID
    $('form[name="planeForm"]').find('.planeId').val($(parent).find('.planeId').text());

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

