$(document).ready(function () {
    $('input[name="username"]').focus();
})

function login() {
    var form = $('form[name="login-form"]');
    var checked = true, index = 0;

    if ($('input[name="username"]').val().trim() == '') {
        if (index++ == 0)
            $('input[name="username"]').focus();
        $('.msg-username').removeClass('msg-valid').addClass('msg-invalid');
        checked = false;
    } else {
        $('.msg-username').removeClass('msg-invalid').addClass('msg-valid');
    }

    if ($('input[name="password"]').val().trim() == '') {
        if (index++ == 0)
            $('input[name="password"]').focus();

        $('.msg-pass').removeClass('msg-valid').addClass('msg-invalid');
        checked = false;
    } else {
        $('.msg-pass').removeClass('msg-invalid').addClass('msg-valid');
    }

    if (!checked) return;

    form.submit();
}