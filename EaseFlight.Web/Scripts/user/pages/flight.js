$(document).ready(function () {
    $('#main-nav').addClass('navbar-theme-abs navbar-theme-transparent navbar-theme-border');

    //Add events One Way/Round Trip option
    addEventTripSearch();
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