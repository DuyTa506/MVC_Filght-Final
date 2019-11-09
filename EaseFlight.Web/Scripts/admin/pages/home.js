$(document).ready(function () {
    $('.select2').select2({
        theme: 'bootstrap4'
    })
})
function saveCountry() {
    var form = document.getElementsByName('addform');
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

function deleteCountry(url) {
    if (confirm("Are You Sure?")) 
        window.location.href = url;
}

function editCountry() {
    var country = $(event.target).parents('tr').find('.country-name').text();
    var region = $(event.target).parents('tr').find('.region-name').text();
    var countryid = $(event.target).parents('tr').find('.country-id').text();

    $('.country-form input[name="country"]').val(country);
    $('.country-form .select2').val(region);
    $('.country-form .select2').trigger('change');
    $('.country-form input#idcountry').val(countryid);
    $('.country-form').attr('action', '/Admin/Home/UpdateCountry');

    $('#addnew').modal("show");

}

function saveAirport() {
    var form = $('form[name="addformairport"]');
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

    var idcountry = $(".select2").val();

    $('input[name="countryid"]').val(idcountry);

    $(form).submit();
}

function editAirport() {
    var name = $(event.target).parents('tr').find('.airport-name').text();
    var city = $(event.target).parents('tr').find('.airport-city').text();
    var country = $(event.target).parents('tr').find('.airport-namecountry').attr('data-value');
    var airportid = $(event.target).parents('tr').find('.airport-id').text();

    $('form[name="addformairport"] input[name="name"]').val(name);
    $('form[name="addformairport"] input[name="city"]').val(city);
    $('form[name="addformairport"] select[name="country"]').val(country);
    $('form[name="addformairport"] select[name="country"]').trigger('change');
    $('form[name="addformairport"] input[name="airportid"]').val(airportid);
    $('form[name="addformairport"]').attr('action', '/Admin/Airport/UpdateAirport');

    $('#addnewairport').modal("show");

}

function deleteAirport(url) {
    if (confirm("Are You Sure?"))
        window.location.href = url;
}
