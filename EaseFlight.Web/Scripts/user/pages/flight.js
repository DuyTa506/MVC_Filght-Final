$(document).ready(function () {
    $('#main-nav').addClass('navbar-theme-abs navbar-theme-transparent navbar-theme-border');

    //Add events One Way/Round Trip option
    addEventTripSearch();

    //Add result flight search
    getFlightSearch();
})

function returnTab() {
    $('.div-return').removeClass('col-md-4 disabled-tab cursor-pointer');
    $('.div-return').addClass('col-md-8');
    $('.div-oneway').removeClass('col-md-8');
    $('.div-oneway').addClass('col-md-4 disabled-tab cursor-pointer');
    $('.div-return .sort-result').removeClass('hide');
    $('.div-oneway .sort-result').addClass('hide');
    $('.div-return .load-more').removeClass('hide');
    $('.div-oneway .load-more').addClass('hide');
}

function onewayTab() {
    $('.div-return').addClass('col-md-4 disabled-tab cursor-pointer');
    $('.div-return').removeClass('col-md-8');
    $('.div-oneway').addClass('col-md-8');
    $('.div-oneway').removeClass('col-md-4 disabled-tab cursor-pointer');
    $('.div-return .sort-result').addClass('hide');
    $('.div-oneway .sort-result').removeClass('hide');
    $('.div-return .load-more').addClass('hide');
    $('.div-oneway .load-more').removeClass('hide');
}

function getFlightSearch() {
    var formData = new FormData();
    var parameters = window.location.search.slice(1).split('&');

    if (parameters.length != 9) return;

    for (var i = 0; i < parameters.length; ++i) {
        var param = parameters[i].split('=');

        if (i != 3 && param.length == 1) return; //Check if param not value, except "return" param

        formData.append(param[0], param[1]);
    }

    $.ajax({
        url: '/Flight/Find',
        type: 'post',
        data: formData,
        cache: false,
        processData: false,
        contentType: false,
        success: function (response) {
            setTimeout(function () {
                $('.loadingbar').addClass('hide');
                var data = JSON.parse(response);
                if (data.type == "error")
                    $('.resultDiv').html('<h3>Error</h3>');
                else $('.resultDiv').html(data.result);
            }, 1000);
        }
    });
}