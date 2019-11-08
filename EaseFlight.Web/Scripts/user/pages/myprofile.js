$(document).ready(function () {
    //Set datetimepicker for birthday
    $('.dob').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    //Set datetimepicker for passport
    $('.passport-expiry').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    //Set Country and City value
    $('select[name="Nationality"]').val(country);
    $('select[name="Nationality"]').trigger("change");
    $('select[name="City"]').val(city);
    $('select[name="City"]').trigger("change");

    //Onchange photo
    $('.upload-photo').change(function () {
        var formData = new FormData();
        var file = document.getElementsByClassName('upload-photo')[0].files[0];

        formData.append("file", file);

        $.ajax({
            type: 'post',
            url: '/Account/ChangePhoto',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                var data = JSON.parse(response);

                if (data.type = 'success') {
                    var lastname = $('.dropdown-toggle.account-name').find('span').text();

                    $('.theme-account-avatar-img').attr('src', data.path);
                    $('.dropdown-toggle.account-name').html('<img class="user-avatar" src="' + data.path + '" width="20" height="20" /><span>' + lastname + '</span>')
                }
            }
        });
    });
});

function updateAccount() {
    var parent = $(event.target).parents('.theme-account-preferences-item-change').find('.theme-account-preferences-item-change-form');
    var form = $(parent).find('form');
    var checked = true; var index = 0;

    $(form).find('input[name="RePassword"]').next('span').text('Re-type new password is required');
    $(form).find('input[name="CurrentPassword"]').next('span').text('Current password is required');
    $(form).find('input[name="NewPassword"]').next('span').text('New password is required');
    $(form).find('input[name="Email"]').next('span').text('Email is required');
    $(form).find('input[name="Phone"]').next('span').text('Phone is required');
    $(form).find('input[name="FirstName"]').next('span').text('First Name is required');
    $(form).find('input[name="LastName"]').next('span').text('Last Name is required');

    //Validate input text
    $(parent).find('input[type="text"], input[type="password"]').each(function () {
        if ($(this).val().trim() == '') {
            if (index++ == 0)
                $(this).focus();

            $(this).addClass("invalid");
            $(this).next('span').removeClass('valid-msg');
            $(this).next('span').addClass('invalid-msg');
            checked = false;
        } else {
            $(this).removeClass("invalid");
            $(this).next('span').addClass('valid-msg');
            $(this).next('span').removeClass('invalid-msg');
        }
    });

    //Validate select
    $(parent).find('select').each(function () {
        if ($(this).val() == '-1' || $(this).val().trim() == '' || $(this).val() == null) {
            $(this).addClass("invalid");
            $(this).next('span').removeClass('valid-msg');
            $(this).next('span').addClass('invalid-msg');
            checked = false;
        } else {
            $(this).removeClass("invalid");
            $(this).next('span').addClass('valid-msg');
            $(this).next('span').removeClass('invalid-msg');
        }
    });

    if (!checked) return;

    var indexName = 0, checkedName = true;
    //Validate first name
    if ($(form).find('input[name="FirstName"]').length > 0 && !validateName($(form).find('input[name="FirstName"]').val())) {
        validateInvalid($(form).find('input[name="FirstName"]'), 'Invalid First Name', indexName++);
        checkedName = false;
    } else validateValid($(form).find('input[name="FirstName"]'), 'First Name is required');

    //Validate last name
    if ($(form).find('input[name="LastName"]').length > 0 && !validateName($(form).find('input[name="LastName"]').val())) {
        validateInvalid($(form).find('input[name="LastName"]'), 'Invalid Last Name', indexName++);
        checkedName = false;
    } else validateValid($(form).find('input[name="LastName"]'), 'Last Name is required');

    if (!checkedName) return;

    //Validate email
    if ($(form).find('input[name="Email"]').length > 0 && !validateEmail($(form).find('input[name="Email"]').val())) {
        validateInvalid($(form).find('input[name="Email"]'), 'Invalid email address');
        return;
    } else validateValid($(form).find('input[name="Email"]'), 'Email is required');

    //Validate phone
    if ($(form).find('input[name="Phone"]').length > 0 && !isNumeric($(form).find('input[name="Phone"]').val())) {
        validateInvalid($(form).find('input[name="Phone"]'), 'Invalid phone number');
        return;
    } else validateValid($(form).find('input[name="Phone"]'), 'Phone is required');
      
    if ($(form).hasClass('password-form')) { //Update password
        var indexPass = 0, checkedPass = true;

        //Check cofirm password
        if ($(form).find('input[name="NewPassword"]').val() != $(form).find('input[name="RePassword"]').val()) {
            validateInvalid($(form).find('input[name="RePassword"]'), 'Re-type password not match', indexPass++);
            checkedPass = false;
        } else validateValid($(form).find('input[name="RePassword"]'), 'Re-type new password is required');

        //Check strong password
        if ($(form).find('input[name="NewPassword"]').length > 0 && !validatePassword($(form).find('input[name="NewPassword"]').val())) {
            validateInvalid($(form).find('input[name="NewPassword"]'), 'Password must contain at least one uppercase letter, one lowercase letter, one numeric character and one special character', indexPass++);
            checkedPass = false;
        } else validateValid($(form).find('input[name="NewPassword"]'), 'New Password is required');

        if (!checkedPass) return;
    
        $.ajax({
            url: '/Account/ChangePassword',
            type: 'post',
            data: form.serialize(),
            success: function (response) {
                var data = JSON.parse(response);

                if (data.type == 'error')
                    validateInvalid($(form).find('input[name="CurrentPassword"]'), 'Your current password not correct, please check again');
                else {
                    $(form).parents('.theme-account-preferences-item-change').find('a').last().trigger("click");
                    ToastSuccess("Update Password successfully");
                }
            }
        });

    } else { //Update information account
        $.ajax({
            url: '/Account/Update',
            type: 'post',
            data: form.serialize(),
            success: function () {
                var title = $(parent).parents('.theme-account-preferences-item').find('.theme-account-preferences-item-title').text();
                var value = $(form).find('input[type="text"]').first().val();
                
                $(form).parents('.theme-account-preferences-item-change').find('a').last().trigger("click");

                //Set value Full name
                if ($(parent).parents('.theme-account-preferences-item').hasClass('fullname-div'))
                    value = ($(form).find('select').val()== '0'?'Mr. ':'Ms/Mrs. ') + $(form).find('input[name="FirstName"]').val() + " " + $(form).find('input[name="LastName"]').val();
                
                $(parent).parents('.theme-account-preferences-item').find('.theme-account-preferences-item-value').text(value);
                ToastSuccess("Update " + title + " successfully");

                //Set default value
                $(form).find('input').each(function () {
                    $(this).attr('value', $(this).val());
                });

                //Set name in navbar
                if ($(form).find('input[name="LastName"]').length > 0)
                    $('.account-name span').text($(form).find('input[name="LastName"]').val());
            }
        });
    }
}

function resetForm() {
    var form = $(event.target).parents('.theme-account-preferences-item').find('form');

    $(form).find('input').removeClass('invalid').each(function () {
        $(this).val($(this).attr('value'));
    });
    $(form).find('span').removeClass('invalid-msg').addClass('valid-msg');
    $(form).find('select').removeClass('invalid');
}

function validateValid(el, message) {
    $(el).removeClass("invalid");
    $(el).next('span').addClass('valid-msg');
    $(el).next('span').removeClass('invalid-msg');
    $(el).next('span').text(message);
}

function validateInvalid(el, message, index) {
    $(el).addClass("invalid");
    $(el).next('span').removeClass('valid-msg');
    $(el).next('span').addClass('invalid-msg');
    $(el).next('span').text(message);
    index ? (index == 0 ? $(el).focus() : "") : $(el).focus();
}