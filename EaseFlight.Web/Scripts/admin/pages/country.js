$(document).ready(function () {
    $('.country-menu').addClass('menu-active');

    //Init datatable
    $("#countryTable").DataTable();
    
    //Add create button
    var parent = $('.dataTables_filter').parent();
    parent.append('<button title="Create country" onclick="openAddModal()" type="button" class="btn btn-block btn-success btn-sm btn-create"><i class="fas fa-plus"></i></button>');

    $.contextMenu({
        selector: '.tr-country',
        callback: function (key, options) {

            if (key == 'edit') { //Edit option
                editCountry($(this));
            } else if (key == 'delete') { //Delete option
                $('.confirm-button').attr('onclick', 'deleteCountry("' + $(this).find('.country-id').text() + '")');
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
    $('form[name="countryForm"]').attr('action', '/Admin/Country/AddNewCountry');
    $('form[name="countryForm"]').trigger('reset');
    $('form[name="countryForm"] span.msg-invalid').removeClass('msg-invalid').addClass('msg-valid');
    $('form[name="countryForm"] input').removeClass('is-invalid');

    $('#countryModal .modal-title').text('Add New Country');
    $('#countryModal').modal('show');
}

function saveCountry() {
    var form = $('form[name="countryForm"]');
    if ($('.countryName').val() == '') {
        $('.countryName').addClass('is-invalid');
        $('.countryName').removeClass('is-valid');
        $('.msg-country').removeClass('msg-valid').addClass('msg-invalid');
        return;
    } else {
        $('.countryName').addClass('is-valid');
        $('.countryName').removeClass('is-invalid');
        $('.msg-country').removeClass('msg-invalid').addClass('msg-valid');
    }
    $(form).submit();
}

function deleteCountry(id) {
    $.ajax({
        url: '/Admin/Country/DeleteCountry',
        type: 'post',
        data: {
            countryid: id
        },
        success: function () {
            window.location.reload();
        }
    })
}


function editCountry(parent) {
    $('form[name="countryForm"] span.msg-invalid').removeClass('msg-invalid').addClass('msg-valid');
    $('form[name="countryForm"] input').removeClass('is-invalid');
    var countryid = $(parent).find('.country-id').text();
    var country = $(parent).find('.country-name').text();
    var region = $(parent).find('.country-region').text();

    $('form[name="countryForm"]').find('input[name="countryid"]').val(countryid);
    $('form[name="countryForm"]').trigger('reset');
    $('#countryModal .modal-title').text('Edit Country');

    $('form[name="countryForm"]').find('input[name="country"').val(country);
    $('form[name="countryForm"]').find('select[name="region"]').val(region);
    $('select[name="region"]').trigger('change');

    $('form[name="countryForm"]').attr('action', '/Admin/Country/UpdateCountry');
    $('#countryModal').modal("show");

}
