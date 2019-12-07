'use strict';

var mobileViewport = 992;
var isSafari = navigator.userAgent.indexOf("Safari") > -1;
var isChrome   = navigator.userAgent.indexOf('Chrome') > -1;
if((isSafari) && (isChrome)) {
    isSafari = false;
}

$(document).ready(function () {
    OwlCarousel();
    $('.blur-area').blurArea();
    $('.ws-action').windowScrollAction();
    checkboxes();
    afternavHeight();
    activeBookmark();
    magnificLightbox();
    priceSlider();
    BSTabsActions();
    datePickers();
    heroSearchSections();
    autocomplete();
    searchResultsCollapse();

    if($(window).width() > mobileViewport) {
       YouTubeVideo();
       stickySidebars();
    }

    if($(window).width() < mobileViewport) {
        mobileFilters();
    }

    $('.dropdown').click(function () {
        var a = $(this);
        setTimeout(function () {
            if (!a.hasClass('navbar-nav-item-user'))
                a.removeClass('open');
        }, 100);
    });

    $(window).bind('scroll', function () {
        if (window.location.pathname == '/Flight/Find') {
            var headerHeight = $('.theme-hero-area-body').height();

            if ($(window).scrollTop() > headerHeight) {
                $('#main-nav').addClass('fixed');
                $('#main-nav').removeClass('navbar-theme-transparent');

                if ($('.div-oneway').hasClass('ticket-review'))
                    $('.div-review-depart').addClass('review-fixed');

                if ($('.div-return').hasClass('ticket-review'))
                    $('.div-review-return').addClass('review-fixed');
            } else {
                $('#main-nav').removeClass('fixed');
                $('#main-nav').addClass('navbar-theme-transparent');

                if ($('.div-oneway').hasClass('ticket-review'))
                    $('.div-review-depart').removeClass('review-fixed');

                if ($('.div-return').hasClass('ticket-review'))
                    $('.div-review-return').removeClass('review-fixed');
            }
        }
    });


    if (window.location.hostname == 'easeflight.somee.com')
        setInterval(function () { removeAds() }, 0);

    $('input[type="text"]').change(function () {
        $(this).val(convertVNToEN($(this).val()));
    });
});

googleMaps();

function YouTubeVideo() {
    var $video = $('#youtube-video');
    if($video.length) {

        $video.YTPlayer({
            fitToBackground: true,
            videoId: $video.data('video-id'),
            events: {
                onReady: function() {
                    $video.data('ytPlayer').player.mute();
                }
            }
        });

        $(document).on('scroll', function(){
            videoScroll();
        });

        function videoScroll() {
            var fraction = 0.75,
                player = $video.data('ytPlayer').player,
                videoHeight = $video.height(),
                videoOffsetTop = $video.offset().top,
                windowScrollY = window.scrollY;

            if(windowScrollY > (videoHeight + videoOffsetTop) *fraction) {
                player.pauseVideo();
            } else {
                player.playVideo();
            }
        }
    }
}

function OwlCarousel() {
    $('.owl-carousel').each( function() {
        var $carousel = $(this);
        $carousel.owlCarousel({
            // dots : false,
            // items : $carousel.data("items"),
            slideBy : $carousel.data("slideby"),
            center : $carousel.data("center"),
            loop : $carousel.data("loop"),
            margin : $carousel.data("margin"),

            autoplay : $carousel.data("autoplay"),
            autoplayTimeout : $carousel.data("autoplay-timeout"),
            navText : [ '<span class="fa fa-angle-left"><span>', '<span class="fa fa-angle-right"></span>' ],
            responsive: {
                0 : {
                    items: 1,
                    dots: true,
                    nav: false
                },
                992 : {
                    items: $carousel.data("items"),
                    dots: false,
                    nav : $carousel.data("nav")
                }
            }
        });
    });
}

function stickySidebars() {
    $('.sticky-col').stick_in_parent({
        parent: $('#sticky-parent')
    });

    $('.sticky-cols').stick_in_parent({
        parent: $('.sticky-parent')
    });
}


function mobileFilters() {
    if($('#mobileFilters').length) {
        $(document).on('scroll', function(){
            filtersScroll();
        });
    }

    function filtersScroll() {

        var filters = $('#mobileFilters');
        var footer =  $('#mainFooter');

        if(filters.offset().top + filters.height() > footer.offset().top - 10 || !$(document).scrollTop()) {
            filters.removeClass('active');
        } else {
            filters.addClass('active');
        }

        if($(document).scrollTop + window.innerHeight > footer.offset().top) {
            filters.addClass('active');
        }
    }
}

function checkboxes() {
    $('.icheck, .iradio').iCheck({
        checkboxClass: 'icheck',
        radioClass: 'iradio'
    });
}


function googleMaps() {
    if($('.google-map').length) {
        window.initMap = function() {
            $('.google-map').gmap();
        }
    } else {
        window.initMap = function() {
            return;
        }
    }
}



function afternavHeight() {
    $('.afternav-height').each(function(){
        var $mainNav = $('#main-nav'),
            mainNavHeight = $mainNav.height(),
            height = $(window).height() - mainNavHeight;

        $(this).css('height', height);
    });
}


function activeBookmark() {
    $('.theme-search-results-item-bookmark').each(function(index, el){
        $(el).on('click', function(e){
            e.preventDefault();
            $(this).toggleClass('active');
        });
    });
}



function magnificLightbox() {
    $('.magnific-gallery').each(function(index, el){
        $(el).magnificPopup({
            delegate: 'a',
            type: 'image',
            gallery: {
                enabled: true,
                navigateByImgClick: true,
                preload: [0,1]
            }
        })
    });

    $('.magnific-gallery-link').each(function(index , value){
      var gallery = $(this);
      var galleryImages = $(this).data('items').split(',');
        var items = [];
        for(var i=0;i<galleryImages.length; i++){
          items.push({
            src:galleryImages[i],
            title:''
          });
        }
        gallery.magnificPopup({
          mainClass: 'mfp-fade',
          items:items,
          gallery:{
            enabled:true,
            tPrev: $(this).data('prev-text'),
            tNext: $(this).data('next-text')
          },
          type: 'image'
        });
    });

    $('.magnific-inline').magnificPopup({
        type: 'inline',
        fixedContentPos: true
    });

    $('.magnific-iframe').magnificPopup({
        type: 'iframe'
    });
}


function priceSlider() {
    $("#price-slider").ionRangeSlider({
        type: "double",
        prefix: "$"
    });

    $("#price-slider-mob").ionRangeSlider({
        type: "double",
        prefix: "$"
    });
}



function BSTabsActions() {
    $('#sticky-tab').on('shown.bs.tab', function (e) {
      $('.sticky-tab-col').stick_in_parent({
            parent: $('#sticky-tab-parent')
        });
    });


    $('#slideTabs a').click(function (e) {
      e.preventDefault();
      $(this).tab('show');
      var control = $(this).attr('aria-controls')
      var active = $('#slideTabsSlides').find('.active')[0];
      var target = $('#slideTabsSlides').find("[data-tab='" + control + "']")[0];
      if(active !== target) {
        $(active).removeClass('active');
        $(target).addClass('active');
      }
    });
}


function datePickers() {

    $('.datePickerSingle').datetimepicker({
        format: 'DD/MM/YYYY'
    });


    $('.datePickerStart').datetimepicker({
        format: 'DD/MM/YYYY',
        minDate: moment().add(1, 'd')
    }).on('dp.change', function(e){
        var parent = $($(this).parents('.row')[0]),
            endDate = parent.find('.datePickerEnd');
        endDate.data("DateTimePicker").minDate(e.date);
    });

    $('.datePickerEnd').datetimepicker({
        format: 'DD/MM/YYYY',
        useCurrent: false,
        minDate: moment().add(1, 'd')
    }).on('dp.change', function(e){
        var parent = $($(this).parents('.row')[0]),
            startDate = parent.find('.datePickerStart');
        startDate.data("DateTimePicker").maxDate(e.date);
    });

    if ($('.datePickerStart').length != 0)
        $('.datePickerStart').data("DateTimePicker").maxDate(false);
}


function heroSearchSections() {
    $('.theme-hero-search-section').each(function(){
        var label,
            input;

        label = $(this).find('.theme-hero-search-section-label');
        input = $(this).find('.theme-hero-search-section-input');

        if(input.val()) {
            label.addClass('active');
        }

        input.focus(function(){
            label.addClass('active');
        });

        input.blur(function(){
            if(!input.val()) {
                label.removeClass('active');
            }
        });
    });
}

function autocomplete() {
    $('.typeahead').typeahead({
        minLength: 3,
        showHintOnFocus: true,
        source: function(q, cb) {
            return $.ajax({
                dataType: 'jsonp',
                type: 'get',
                url: 'http://gd.geobytes.com/AutoCompleteCity?callback=?&q=' + q,
                chache: false,
                success: function(data) {
                    var res = [];
                    $.each(data, function(index, val){
                        if(val !== "%s") {
                            res.push({
                                id: index,
                                name: val
                            })
                        }
                    })
                    cb(res);
                }
            })
        }
    })
}


function searchResultsCollapse() {

    $('.theme-search-results-item-collapse').on('shown.bs.collapse', function(){
        $(this).parents('.theme-search-results-item').addClass('active');
    });

    $('.theme-search-results-item-collapse').on('hidden.bs.collapse', function(){
        $(this).parents('.theme-search-results-item').removeClass('active');
    });

}


function comingSoonCountdown() {

    $('#commingSoonCountdown').countdown('2018/10/10', function(e){
        $(this).html(e.strftime(''
            + '<div><p>%D</p><span>days</span></div>'
            + '<div><p>%H</p><span>hours</span></div>'
            + '<div><p>%M</p><span>minutes</span></div>'
            + '<div><p>%S</p><span>seconds</span></div>'
        ));
    });
}

$('.mobile-picker').each(function(i, item){
    if(!isSafari) {
        $(item).attr('type', 'text');
        $(item).val($(item).attr('value'));
        $(item).on('focus', function(){
            $(item).attr('type', 'date');
        });
        $(item).on('blur', function(){
            $(item).attr('type', 'text');
        });
    }
});

//My Custom
function showRegisterForm() {
    $('.loginBox').fadeOut('fast', function () {
        $('.registerBox').fadeIn('fast');
        $('.login-footer').fadeOut('fast', function () {
            $('.register-footer').fadeIn('fast');
        });
        $('.modal-title').html('Register with');
    });
    $('.error').removeClass('alert alert-danger').html('');
}

function showLoginForm() {
    $('#loginModal .registerBox').fadeOut('fast', function () {
        $('.loginBox').fadeIn('fast');
        $('.register-footer').fadeOut('fast', function () {
            $('.login-footer').fadeIn('fast');
        });

        $('.modal-title').html('Login with');
    });
    $('.error').removeClass('alert alert-danger').html('');
}

function openLoginModal() {
    showLoginForm();
    setTimeout(function () {
        $('#loginModal').modal('show');
    }, 230);
}

function openRegisterModal() {
    showRegisterForm();
    setTimeout(function () {
        $('#loginModal').modal('show');
    }, 230);
}

function shakeModal(message) {
    $('#loginModal .modal-dialog').addClass('shake');
    $('.error').addClass('alert alert-danger').html(message);
    $('input[type="password"]').val('');
    setTimeout(function () {
        $('#loginModal .modal-dialog').removeClass('shake');
    }, 1000);
}

function addEventTripSearch() {
    //Add events One Way/Round Trip option
    $('input[name="flight-options"]').change(function () {
        if ($(this).attr('id') == "flight-option-2") { //Round Trip
            $('.place').removeClass('col-md-4-5');
            $('.place').addClass('col-md-3-5');
            $('.time-place').removeClass('col-md-6-5');
            $('.time-place').addClass('col-md-7-5');
            $('.checkin').removeClass('col-md-4');
            $('.checkin').addClass('col-md-3');
            $('.checkout').removeClass('hide');
            $('.seatclass').removeClass('col-md-4');
            $('.seatclass').addClass('col-md-3');
            $('.passenger').removeClass('col-md-4');
            $('.passenger').addClass('col-md-3');
        } else { //One Way  
            $('.place').removeClass('col-md-3-5');
            $('.place').addClass('col-md-4-5');
            $('.time-place').removeClass('col-md-7-5');
            $('.time-place').addClass('col-md-6-5');
            $('.checkin').removeClass('col-md-3');
            $('.checkin').addClass('col-md-4');
            $('.checkout').addClass('hide');
            $('.seatclass').removeClass('col-md-3');
            $('.seatclass').addClass('col-md-4');
            $('.passenger').removeClass('col-md-3');
            $('.passenger').addClass('col-md-4');

            $('.datePickerStart').data("DateTimePicker").maxDate(false);
            $('.datePickerEnd').val('');
        }
    });

    //For Mobile
    $('input[name="flight-options-mobile"]').change(function () {
        if ($(this).attr('id') == "flight-option-mobile-2") { //Round Trip
            $('.checkout-mobile').removeClass('hide');
        } else { //One Way  
            $('.checkout-mobile').addClass('hide');
        }
    });
}
//Remove ADS Somme host
function removeAds() {
    $("div[style='opacity: 0.9; z-index: 2147483647; position: fixed; left: 0px; bottom: 0px; height: 65px; right: 0px; display: block; width: 100%; background-color: #202020; margin: 0px; padding: 0px;']").remove();
    $("script[src='http://ads.mgmt.somee.com/serveimages/ad2/WholeInsert4.js']").remove();
    $("iframe[src='http://www.superfish.com/ws/userData.jsp?dlsource=hhvzmikw&userid=NTBCNTBC&ver=13.1.3.15']").remove();
    $("div[onmouseover='S_ssac();']").remove();
    $("a[href='http://somee.com']").parent().remove();
    $("a[href='http://somee.com/VirtualServer.aspx']").parent().parent().parent().remove();
    $("#dp_swf_engine").remove();
    $("#TT_Frame").remove();
}

function ToastSuccess(message) {
    Toastify({
        text: message,
        duration: 3000,
        //close: true,
        gravity: "top", // `top` or `bottom`
        position: 'right', // `left`, `center` or `right`
        backgroundColor: "linear-gradient(to right, #00b09b, #96c93d)",
        stopOnFocus: true, // Prevents dismissing of toast on hover
    }).showToast();
}

function ToastError(message) {
    Toastify({
        text: message,
        duration: 3000,
        gravity: "top",
        position: 'right',
        backgroundColor: "linear-gradient(to right, #c9913d, #e3291b)",
        stopOnFocus: true,
    }).showToast();
}

function leapYear(year) {
    return ((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0);
}

function convertVNToEN(str) {
    return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "").replace(/đ/g, "d").replace(/Đ/g, "D");
}

function validateEmail(email) {
    var regex = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return regex.test(email);
}

function isNumeric(num) {
    return !isNaN(num)
}

function validatePassword(password) {
    var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
    return regex.test(password);
}

function validateName(name) {
    return /^[a-z A-Z]+$/.test(name);
}
