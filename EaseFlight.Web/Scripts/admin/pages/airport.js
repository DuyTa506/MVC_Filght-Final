$(document).ready(function () {
    $('.airport-menu').addClass('menu-active');

    //Init datatable
    $("#airportTable").DataTable();

    //Add create button
    var parent = $('.dataTables_filter').parent();
    parent.append('<button title="Create airport" onclick="openAddModal()" type="button" class="btn btn-block btn-success btn-sm btn-create"><i class="fas fa-plus"></i></button>');

    $.contextMenu({
        selector: '.tr-airport',
        callback: function (key, options) {

            if (key == 'edit') { //Edit option
                editAirport($(this));
            } else if (key == 'delete') { //Delete option
                $('.confirm-button').attr('onclick', 'deleteAirport("' + $(this).find('.airport-id').text() + '")');
                $('#confirm-modal').modal('show');
            }
        },
        items: {
            "edit": { name: "Edit", icon: "edit" },
            "delete": { name: "Delete", icon: "delete" }
        }
    });
})

function openAddModal() {
    $('form[name="airportForm"]').attr('action', '/Admin/Airport/AddNewAirport');
    $('form[name="airportForm"]').trigger('reset');
    $('form[name="airportForm"] span.msg-invalid').removeClass('msg-invalid').addClass('msg-valid');
    $('form[name="airportForm"] input').removeClass('is-invalid');
    $('form[name="airportForm"] select').removeClass('is-invalid');

    $('#airportModal .modal-title').text('Add New Airport');
    $('#airportModal').modal('show');
}

function saveAirport() {
    var form = $('form[name="airportForm"]');
    var checked = true, index = 0;

    if ($('input[name="name"]').val().trim() == '') {
        if (index++ == 0)
            $('input[name="name"]').focus();
        $('.msg-name').removeClass('msg-valid').addClass('msg-invalid');
        $('input[name="name"]').addClass('is-invalid').removeClass('is-valid');
        checked = false;
    } else {
        $('.msg-name').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="name"]').removeClass('is-invalid').addClass('is-valid');
    }

    if ($('input[name="city"]').val().trim() == '') {
        if (index++ == 0)
            $('input[name="city"]').focus();

        $('.msg-city').removeClass('msg-valid').addClass('msg-invalid');
        $('input[name="city"]').addClass('is-invalid').removeClass('is-valid');
        checked = false;
    } else {
        $('.msg-city').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="city"]').removeClass('is-invalid').addClass('is-valid');
    }

    if (!checked) return;

    var idcountry = $('form[name="airportForm"]').find('select[name="country"]').val();

    $('form[name="airportForm"]').find('input[name="countryid"]').val(idcountry);

    $(form).submit();
}

function editAirport(parent) {
    $('form[name="airportForm"] span.msg-invalid').removeClass('msg-invalid').addClass('msg-valid');
    $('form[name="airportForm"] input').removeClass('is-invalid');
    $('form[name="airportForm"] select').removeClass('is-invalid');
    var name = $(parent).find('.airport-name').text();
    var city = $(parent).find('.airport-city').text();
    var countryid = $(parent).find('.airport-country').attr('data-value');
    var airportid = $(parent).find('.airport-id').text();

    $('form[name="airportForm"]').find('input[name = "name"]').val(name);

    $('form[name="airportForm"]').find('input[name="city"]').val(city);

    $('form[name="airportForm"]').find('select[name="countryid"]').val(countryid);
    $('form[name="airportForm"]').find('select[name="countryid"]').trigger('change');

    $('form[name="airportForm"]').find('input[name="airportid"]').val(airportid);

    $('form[name="airportForm"]').attr('action', '/Admin/Airport/UpdateAirport');

    $('#airportModal').modal("show");
}

function deleteAirport(id) {
    $.ajax({
        url: '/Admin/Airport/DeleteAirport',
        type: 'post',
        data: {
            airportid: id
        },
        success: function () {
            window.location.reload();
        }
    })
}