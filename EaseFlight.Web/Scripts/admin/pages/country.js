﻿$(document).ready(function () {
    $('.select2').select2({
        theme: 'bootstrap4'
    })
    //Init datatable
    $("#countryTable").DataTable();

    ////Add create button
    var parent = $('.dataTables_filter').parent();
    parent.append('<button title="Create country" onclick="openAddModal()" type="button" class="btn btn-block btn-success btn-sm btn-create"><i class="fas fa-plus"></i></button>');

    $.contextMenu({
        selector: '.tr-country',
        callback: function (key, options) {

            if (key == 'edit') { //Edit option
                editPlane($(this));
            } else if (key == 'delete')
            { //Delete option
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

    $('#countryModal .modal-title').text('Add New Country');
    $('#countryModal').modal('show');
}

function saveCountry() {
    var form = $('form[name="countryForm"]');
    if ($('.countryName').val() == '') {
        $('.countryName').addClass('is-invalid');
        $('.countryName').removeClass('is-valid');
        return;
    } else {
        $('.countryName').addClass('is-valid');
        $('.countryName').removeClass('is-invalid');
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

    var countryid = $(parent).find('.country-id').text();
    var country = $(parent).find('.country-name').text();
    var region = $(parent).find('.country-region').text();

    $('form[name="countryForm"]').trigger('reset');
    $('#countryModal .modal-title').text('Edit Country');

    $('.country-form input[name="country"]').val(country);
    $('.country-form .select2').val(region);
    $('.country-form .select2').trigger('change');
    $('.country-form input#idcountry').val(countryid);
    $('form[name="countryForm"]').attr('action', '/Admin/Country/UpdateCountry');
    $('#countryModal').modal("show");

}