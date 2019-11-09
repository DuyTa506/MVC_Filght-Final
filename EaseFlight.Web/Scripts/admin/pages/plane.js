//$(document).ready(function () {

//    $('input[type="number"]').change(function () {
//        var firstclass = parseInt($('input[name="firstclass"]').val());
//        var business = parseInt($('input[name="business"]').val());
//        var economy = parseInt($('input[name="economy"]').val());
//        var capacity = parseInt($('select[name="planetype"] option:selected').attr('data-capacity'));

//    })

//})

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
    var planeid = $('.plane-name').val();
    var planetype = $('.plane-type');
    var planetype = $();
    var planetype = $();
    var planetype = $();
    var planetype = $();
    var planetype = $();
}