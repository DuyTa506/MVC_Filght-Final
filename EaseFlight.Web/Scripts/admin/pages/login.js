$(document).ready(function () {
    $('input[name="username"]').focus();

    //Enter Key
    $(document).on('keypress', function (e) {
        if (e.which == 13) {
            login();
        }
    });
})

function login() {
    var form = $('form[name="login-form"]');
    var checked = true, index = 0;

    if ($('input[name="username"]').val().trim() == '') {
        if (index++ == 0)
            $('input[name="username"]').focus();
        $('.msg-username').removeClass('msg-valid').addClass('msg-invalid');
        $('input[name="username"]').addClass('is-invalid').removeClass('is-valid');
        checked = false;
    } else {
        $('.msg-username').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="username"]').removeClass('is-invalid').addClass('is-valid');
    }

    if ($('input[name="password"]').val().trim() == '') {
        if (index++ == 0)
            $('input[name="password"]').focus();

        $('.msg-pass').removeClass('msg-valid').addClass('msg-invalid');
        $('input[name="password"]').addClass('is-invalid').removeClass('is-valid');
        checked = false;
    } else {
        $('.msg-pass').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="password"]').removeClass('is-invalid').addClass('is-valid');
    }

    if (!checked) return;

    form.submit();
}