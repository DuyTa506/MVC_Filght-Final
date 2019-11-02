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
    window.location.href = url;
}