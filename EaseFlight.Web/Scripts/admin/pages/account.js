var username = '', email = '';

$(document).ready(function () {
    $('.account-menu').addClass('menu-active');

    //Init datatable
    $("#accountTable").DataTable();

    ////Add create button
    var parent = $('.dataTables_filter').parent();
    parent.append('<button title="Create account" onclick="openAddModal()" type="button" class="btn btn-block btn-success btn-sm btn-create"><i class="fas fa-plus"></i></button>');

    //Add context Menu
    $.contextMenu({
        selector: '.tr-account',
        callback: function (key, options) {

            if (key == 'disable') { //Disable account option
                if ($(this).find('.account-status').text() == 'Disable')
                    ToastError("You can't disable to account has disabled");
                else {
                    disableAccount($(this));
                }
            } else if (key == 'active') { //Active account option
                if ($(this).find('.account-status').text() == 'Active')
                    ToastError("You can't active to account has actived");
                else {
                    activeAccount($(this));
                }
            } else if (key == 'edit') { //Edit option
                openEditModal($(this));
            } else if (key == 'delete') { //Delete option
                $('.confirm-button').attr('onclick', 'deleteAccount("' + $(this).find('.accountId').text() + '")');
                $('#confirm-modal').modal('show');
            }
        },
        items: {
            "active": { name: "Active account", icon: "paste" },
            "disable": {
                name: "Disable account", icon: function() {
                    return 'context-menu-icon context-menu-icon-quit';
                }
            },
            "sep1": "---------",
            "edit": { name: "Edit", icon: "edit" },
            "delete": { name: "Delete", icon: "delete" }
        }
    });

    $('input[name="birthday"]').daterangepicker({
        timePicker: false,
        singleDatePicker: true,
        showDropdowns: true,
        locale: {
            format: 'DD/MM/YYYY'
        }
    });

    $('#accountModal').on('shown.bs.modal', function () {
        $('input[name="firstname"]').focus();
        $('.account-form .msg-span').removeClass('msg-invalid').addClass('msg-valid');
        $('.account-form input').removeClass('is-invalid');
    });
    $('#accountModal').on('hidden.bs.modal', function () {
        username = ''; email = '';
    });
})

function openAddModal() {
    $('.account-form').trigger('reset');
    $('input[name="username"]').prop("disabled", false);
    $('#accountModal .modal-title').text('Add Account');
    $('.account-form').attr('action', '/Admin/Account/AddAccount');
    $('.pwdefault-label').text('Password Default');
    $('#accountModal').modal('show');

    setTimeout(function () {
        $('input[name="password"]').val('P@ssword123');
    }, 200);
}

function saveAccount() {
    $('.msg-firstname').text('First Name is required');
    $('.msg-lastname').text('Last Name is required');
    $('.msg-phone').text('Phone is required');
    $('.msg-email').text('Email is required');
    $('.msg-username').text('Username is required');

    var checked = true, index = 0;

    $('input[type="text"]').not('input[name="password"]').each(function () {
        var spanmsg = '.msg-' + $(this).attr('name');

        if ($(this).val().trim() == '') {
            if (index++ == 0)
                $(this).focus();

            $(spanmsg).removeClass('msg-valid').addClass('msg-invalid');
            $(this).addClass('is-invalid');
            checked = false;
        } else {
            $(spanmsg).removeClass('msg-invalid').addClass('msg-valid');
            $(this).removeClass('is-invalid');
        }
    });

    if (!checked) return;
    var checked = true, index = 0;

    //Validate first name
    if ($('input[name="username"]').val().length < 6 || $('input[name="username"]').val().length > 20) {
        if (index++ == 0)
            $('input[name="username"]').focus();

        $('.msg-username').removeClass('msg-valid').addClass('msg-invalid');
        $('.msg-username').text('Length of Username minimum is 6 characters and maximum is 20 characters');
        $('input[name="username"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-username').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="username"]').removeClass('is-invalid');
    }

    //Validate first name
    if (!validateName($('input[name="firstname"]').val())) {
        if (index++ == 0)
            $('input[name="firstname"]').focus();

        $('.msg-firstname').removeClass('msg-valid').addClass('msg-invalid');
        $('.msg-firstname').text('Invalid First Name');
        $('input[name="firstname"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-firstname').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="firstname"]').removeClass('is-invalid');
    }

    //Validate last name
    if (!validateName($('input[name="lastname"]').val())) {
        if (index++ == 0)
            $('input[name="lastname"]').focus();

        $('.msg-lastname').removeClass('msg-valid').addClass('msg-invalid');
        $('.msg-lastname').text('Invalid Last Name');
        $('input[name="lastname"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-lastname').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="lastname"]').removeClass('is-invalid');
    }

    //Validate email
    if (!validateEmail($('input[name="email"]').val())) {
        if (index++ == 0)
            $('input[name="email"]').focus();

        $('.msg-email').removeClass('msg-valid').addClass('msg-invalid');
        $('.msg-email').text('Invalid Email');
        $('input[name="email"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-email').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="email"]').removeClass('is-invalid');
    }

    //Validate phone
    if (!isNumeric($('input[name="phone"]').val())) {
        if (index++ == 0)
            $('input[name="phone"]').focus();

        $('.msg-phone').removeClass('msg-valid').addClass('msg-invalid');
        $('.msg-phone').text('Invalid Phone number');
        $('input[name="phone"]').addClass('is-invalid');
        checked = false;
    } else {
        $('.msg-phone').removeClass('msg-invalid').addClass('msg-valid');
        $('input[name="phone"]').removeClass('is-invalid');
    }

    if (!checked) return;

    //Check username or email exist
    var currentUsername = $('input[name="username"]').val();
    var currentEmail = $('input[name="email"]').val();
  
    var formdata = '';
    if (currentEmail.trim() != email.trim() && currentUsername.trim() != username)
        formdata = 'username=' + currentUsername + '&email=' + currentEmail;
    else if (currentEmail.trim() != email.trim())
        formdata = 'username=' + currentEmail + '&email=' + currentEmail;
    else if (currentUsername.trim() != username)
        formdata = 'username=' + currentUsername + '&email=' + currentUsername;

    checkUsernameExist(formdata, function callback(data) {
        var msg = data.msg;
        var checkexist = true;

        if (msg == "Username") {
            $('.msg-username').removeClass('msg-valid').addClass('msg-invalid');
            $('.msg-username').text('Username has exist');
            $('input[name="username"]').addClass('is-invalid').focus();
            checkexist = false;
        } else {
            $('.msg-username').removeClass('msg-invalid').addClass('msg-valid');
            $('input[name="username"]').removeClass('is-invalid');
        }

        if (msg == "Email") {
            $('.msg-email').removeClass('msg-valid').addClass('msg-invalid');
            $('.msg-email').text('Email has exist');
            $('input[name="email"]').addClass('is-invalid').focus();
            checkexist = false;
        } else {
            $('.msg-email').removeClass('msg-invalid').addClass('msg-valid');
            $('input[name="email"]').removeClass('is-invalid');
        }

        if (!checkexist) return;

        $('.account-form').submit();
    });
}

function checkUsernameExist(formData, callback) {
    $.ajax({
        url: '/Account/CheckUsernameExist',
        type: 'post',
        data: formData,
        success: function (response) {
            callback(JSON.parse(response));
        }
    });
}

function openEditModal(parent) {
    $('input[name="username"]').prop("disabled", false);
    $('#accountModal .modal-title').text('Edit Account');
    $('.account-form').attr('action', '/Admin/Account/UpdateAccount');
    $('.pwdefault-label').text('Password');
    $('input[name="password"]').val('*************');
    $('#accountModal').modal('show');

    //Set value modal
    var title = ($(parent).find('.title').text().trim() == 'Mr') ? "0" : "1";

    $('input[name="firstname"]').val($(parent).find('.firstname').text());
    $('input[name="lastname"]').val($(parent).find('.lastname').text());
    $('input[name="username"]').val($(parent).find('.username').text());
    $('input[name="address"]').val($(parent).find('.address').text());
    $('input[name="phone"]').val($(parent).find('.phone').text());
    $('input[name="email"]').val($(parent).find('.email').text());
    $('input[name="accountId"]').val($(parent).find('.accountId').text());
    $('select[name="title"]').val(title);
    $('select[name="title"]').trigger('change');

    if ($(parent).find('.username').text() == 'Third Login')
        $('input[name="username"]').attr('disabled', 'true');

    var birthday = $(parent).find('.birthday').text();

    if (birthday.trim() == '')
        birthday = moment().format("DD/MM/YYYY");

    $('input[name="birthday"]').daterangepicker({
        timePicker: false,
        singleDatePicker: true,
        showDropdowns: true,
        startDate: birthday,
        locale: {
            format: 'DD/MM/YYYY'
        }
    });

    username = $('input[name="username"]').val();
    email = $('input[name="email"]').val();
}

function disableAccount(parent) {
    var id = $(parent).find('.accountId').text();

    $.ajax({
        url: '/Admin/Account/DisableAccount',
        data: {
            accountId: id
        },
        type: 'post',
        success: function () {
            ToastSuccess('Disable account successfully');
            $(parent).find('.account-status').text('Disable');
            $(parent).find('.account-status').removeClass('account-active').addClass('account-disable');
        }
    });
}

function activeAccount(parent) {
    var id = $(parent).find('.accountId').text();

    $.ajax({
        url: '/Admin/Account/ActiveAccount',
        data: {
            accountId: id
        },
        type: 'post',
        success: function () {
            ToastSuccess('Active account successfully');
            $(parent).find('.account-status').text('Active');
            $(parent).find('.account-status').removeClass('account-disable').addClass('account-active');
        }
    });
}

function deleteAccount(id) {
    $.ajax({
        url: '/Admin/Account/DeleteAccount',
        data: {
            accountId: id
        },
        type: 'post',
        success: function() {
            window.location.reload();
        }
    });
}