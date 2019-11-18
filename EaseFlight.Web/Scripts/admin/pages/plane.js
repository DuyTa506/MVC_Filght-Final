$(document).ready(function () {
    $('.plane-menu').addClass('menu-active');

    //Init datatable
    $("#planeTable").DataTable();

    ////Add create button
    var parent = $('.dataTables_filter').parent();
    parent.append('<button title="Create aircraft" onclick="openAddModal()" type="button" class="btn btn-block btn-success btn-sm btn-create"><i class="fas fa-plus"></i></button>');

    $.contextMenu({
        selector: '.tr-plane',
        callback: function (key, options) {

            if (key == 'status') { //Change status option
                openStatusModal($(this));
            } else if (key == 'edit') { //Edit option
                editPlane($(this));
            } else if (key == 'delete') { //Delete option
                $('.confirm-button').attr('onclick', 'deletePlane("' + $(this).find('.plane-id').text() + '")');
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

})
function openStatusModal(parent) {
    var status = $(parent).find('.plane-status').text();
    var selector = '.btn-' + status.toLowerCase();

    $('.btn-status').removeClass('hide');
    $(selector).addClass('hide');
    $('#status-modal').find('input').val($(parent).find('.plane-id').text());
    $('#status-modal').modal('show');
}
function changeReady() {
    var planeId = $('#status-modal').find('input').val();
    window.location.href = '/Admin/Plane/ChangeStatus?id=' + planeId + '&status=Ready';
}

function changeOnline() {
    var planeId = $('#status-modal').find('input').val();
    window.location.href = '/Admin/Plane/ChangeStatus?id=' + planeId + '&status=Online';
}

function changeRepair() {
    var planeId = $('#status-modal').find('input').val();
    window.location.href = '/Admin/Plane/ChangeStatus?id=' + planeId + '&status=Repair';
}

function openAddModal() {
    $('form[name="planeForm"]').attr('action', '/Admin/Plane/AddNewPlane');
    $('form[name="planeForm"]').trigger('reset');
    $('form[name="planeForm"] span.msg-invalid').removeClass('msg-invalid').addClass('msg-valid');
    $('form[name="planeForm"] input').removeClass('is-invalid');
    $('form[name="planeForm"] select').removeClass('is-invalid');

    $('#planeModal .modal-title').text('Add New Aircraft');
    $('#planeModal').modal('show');
}

function savePlane() {
    var form = $('form[name="planeForm"]');
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

function editPlane(parent) {
    $('form[name="planeForm"] span.msg-invalid').removeClass('msg-invalid').addClass('msg-valid');
    $('form[name="planeForm"] input').removeClass('is-invalid');
    $('form[name="planeForm"] select').removeClass('is-invalid');
    $('form[name="planeForm"]').attr('action', '/Admin/Plane/UpdatePlane');
    $('form[name="planeForm"]').trigger('reset');
    $('#planeModal .modal-title').text('Edit Aircraft');
    var planeairportid = [];
    var planeseatclass = [];
    var seatcapacity = [];
    var seatprice = [];
    var planeid = $(parent).find('.plane-id').text();
    var planeairline = $(parent).find('.plane-airline').text();
    var planetypeid = $(parent).find('.plane-type').attr("data-id");

    $(parent).find('.plane-airport li').each(function () {
        planeairportid.push($(this).attr("data-id"));
    })

    $(parent).find('.plane-seatclass li').each(function () {
        planeseatclass.push($(this).attr("data-id"));
        seatcapacity.push($(this).attr("data-capacity"));
        seatprice.push($(this).attr("data-price"));
    })

    //Set Plane ID
    $('form[name="planeForm"]').find('input[name="planeid"]').val(planeid);
    $('form[name="planeForm"]').find('.plane-type').val($(parent).find('.plane-id').text());
    //
    $('form[name="planeForm"]').find('.airplane').val(planeairline);
    //
    $('form[name="planeForm"]').find('input[name="firstclass"]').val(typeof seatcapacity[0] == "undefined" ? "0" : seatcapacity[0]);
    $('form[name="planeForm"]').find('input[name="business"]').val(typeof seatcapacity[1] == "undefined" ? "0" : seatcapacity[1]);
    $('form[name="planeForm"]').find('input[name="economy"]').val(typeof seatcapacity[2] == "undefined" ? "0" : seatcapacity[2]);
    //
    $('form[name="planeForm"]').find('input[name="firstclassprice"]').val(typeof seatprice[0] == "undefined" ? "0" : seatprice[0]);
    $('form[name="planeForm"]').find('input[name="businessprice"]').val(typeof seatprice[1] == "undefined" ? "0" : seatprice[1]);
    $('form[name="planeForm"]').find('input[name="economyprice"]').val(typeof seatprice[2] == "undefined" ? "0" : seatprice[2]);
    //
    $('form[name="planeForm"]').find('select[name="planetype"]').val(planetypeid);
    $('select[name="planetype"]').trigger('change');
    //
    $('form[name="planeForm"]').find('select[name="airport"]').val(planeairportid);
    $('select[name="airport"]').trigger('change');

    $('#planeModal').modal("show");
}

function deletePlane(id) {
    $.ajax({
        url: '/Admin/Plane/DelelePlane',
        type: 'post',
        data: {
            planeid: id
        },
        success: function () {
            window.location.reload();
        }
    })
}

