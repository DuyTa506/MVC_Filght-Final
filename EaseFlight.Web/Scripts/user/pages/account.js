﻿$(document).ready(function () {
    var strongRegex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
})
function register() {
    var formData = $('#registerForm').serialize();
    var formDataArr = $('#registerForm').serializeArray();
    if (formDataArr[3].value != formDataArr[4].value) {
        $('.error').addClass('alert alert-danger').html('Confirm password not correct!');
        return;
    }
    var inputs = $('#registerForm input');
    for (var i = 0; i < inputs.length - 1; ++i)
        if (!checkInputText(inputs.get(i))) return;
    
    checkUsernameExist(formData, function callback(data) {
        var msg = data.msg;
        if (msg == "Username")
            $('.error').addClass('alert alert-danger').html('Username has exist!');
        else if (msg == "Email")
            $('.error').addClass('alert alert-danger').html('Email has exist!');
        else {
            $.ajax({
                url: '/Account/Register',
                data: $('#registerForm').serialize(),
                type: 'post',
                success: function () {
                    showLoginForm();
                    $('.error').addClass('alert alert-success').html('Register successfully, please login!')
                }
            });
        }
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

function checkInputText(textInput) {
    textInput = textInput ? textInput : event.target;
    //Check empty
    if (textInput && textInput.value == "") {
        textInput.focus();
        $(textInput).addClass("has-error");
        $('.error').addClass('alert alert-danger').html("Please input " + textInput.placeholder);
        return false;
    }
    $(textInput).removeClass("has-error");
    $('.error').removeClass('alert alert-danger').html('');
    return true;
}

function forgotPassword() {
    var formData = $('#forgotForm').serialize();
    $.ajax({
        url: '/Account/ForgotPassword',
        type: 'post',
        data: formData,
        success: function (response) {
            var message = JSON.parse(response).msg;
            $('.msg-error').html(message);
        }
    });
}

//function resetPassword() {
//    var formData = $('#forgotForm').serializeArray();

//    if (formData[0].value != formData[1].value) {
//        $('.msg-error').text('Confirm password not correct!');
//        return;
//    }

//    $.ajax({
//        url: '/Account/Login',
//        type: 'post',
//        data: $('#forgotForm').serialize(),
//        success: function (response) {
//            var message = JSON.parse(response).msg;
//            if (message != "success")
//                $('.msg-error').text(message);
//            else window.location.reload();
//        }
//    });
//}

function login() {
    var username = document.getElementById('loginForm').querySelector('input[name="username"]');
    var password = document.getElementById('loginForm').querySelector('input[name="password"]');
    var url = document.getElementById('redirectUrl').value;
    if (!checkInputText(username)) return;
    if (!checkInputText(password)) return;

    var formData = $('#loginForm').serialize();
    $.ajax({
        url: '/Account/Login',
        type: 'post',
        data: formData,
        success: function (response) {
            var message = JSON.parse(response).msg;
            if (message != "success")
                $('.error').addClass('alert alert-danger').html(message);
            else {
                if (url == '')
                    window.location.reload();
                else window.location.href = url;
            }
        }
    });
}

function getForgetPasswordTab() {
    $('.li-forgot').removeClass('hide');
    $('.li-forgot').addClass('active');
    $('#forgot').addClass('active in');
    $('.li-signin').addClass('hide');
    $('#signin').addClass('hide');
}

function getSigninTab() {
    $('.li-forgot').addClass('hide');
    $('.li-forgot').removeClass('active');
    $('#forgot').removeClass('active in');
    $('.li-signin').removeClass('hide');
    $('#signin').removeClass('hide');
    $('.li-signin').addClass('active');
    $('#signin').addClass('active in');
}

//Login with Google
var hostname = 'https://' + window.location.hostname + (window.location.port ? ':' : '') + window.location.port;
var OAUTHURL = 'https://accounts.google.com/o/oauth2/auth?';
var VALIDURL = 'https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=';
var SCOPE = 'https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email';
var CLIENTID = '296844354744-6p6ttffmeajbu5igimu3fsrkb561j9gm.apps.googleusercontent.com';
var REDIRECT = hostname;
var LOGOUT = hostname + '/Account/Logout';
var TYPE = 'token';
var _url = OAUTHURL + 'scope=' + SCOPE + '&client_id=' + CLIENTID + '&redirect_uri=' + REDIRECT + '&response_type=' + TYPE;
var acToken;
var tokenType;
var expiresIn;
var user;
var loggedIn = false;
function loginGoogle() {
    var win = window.open(_url, "windowname1", 'width=800, height=600');
    var pollTimer = window.setInterval(function () {
        try {
            if (win.document.URL.indexOf(REDIRECT) != -1) {
                window.clearInterval(pollTimer);
                var url = win.document.URL;
                acToken = gup(url, 'access_token');
                tokenType = gup(url, 'token_type');
                expiresIn = gup(url, 'expires_in');
                win.close();
                validateToken(acToken);
            }
        }
        catch (e) {

        }
    }, 500);
}
function gup(url, name) {
    namename = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\#&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(url);
    if (results == null)
        return "";
    else
        return results[1];
}

function validateToken(token) {
    getUserInfo();
    $.ajax({
        url: VALIDURL + token,
        data: null,
        success: function () {

        }
    });
}

function getUserInfo() {
    $.ajax({
        url: 'https://www.googleapis.com/oauth2/v1/userinfo?access_token=' + acToken,
        data: null,
        success: function (user) {
            $.ajax({
                url: '/Account/ThirdPartyLogin',
                type: 'post',
                data: {
                    id: user.id,
                    name: user.name,
                    email: user.email,
                    picture: user.picture
                },
                success: function () {
                    var url = document.getElementById('redirectUrl').value;
                    if (url == '')
                        window.location.reload();
                    else window.location.href = url;
                }
            });
        }
    });
}
//End

//Login with Facebook
window.fbAsyncInit = function () {
    FB.init({
        appId: '980500005622744',
        cookie: true,
        xfbml: true,
        version: 'v4.0'
    });
};

// Load the JavaScript SDK asynchronously
(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = "//connect.facebook.net/en_US/sdk.js";
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));

// Facebook login with JavaScript SDK
function loginFacebook() {
    FB.login(function (response) {
        if (response.authResponse) {
            getFbUserData();
        }
    }, { scope: 'email' });
}

// Fetch the user profile data from facebook
function getFbUserData() {
    FB.api('/me?fields=id,name,email,picture',
        function (user) {
            $.ajax({
                url: '/Account/ThirdPartyLogin',
                type: 'post',
                data: {
                    id: user.id,
                    name: user.name,
                    email: user.email,
                    picture: 'https://graph.facebook.com/' + user.id + '/picture?type=large'
                },
                success: function () {
                    var url = document.getElementById('redirectUrl').value;
                    if (url == '')
                        window.location.reload();
                    else window.location.href = url;
                }
            });
        });
}
//End