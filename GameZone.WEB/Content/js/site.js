$(document).ready(function () {

    $(window).scroll(function () {
        if ($(window).scrollTop() >= $('.page-section:first-of-type').height()) {
            // page-section:first
            $('header').addClass('sticky')
        }
        else {
            $('header').removeClass('sticky');
        }
    });

    // making game-filter fixed when it scrolls
    $(window).on('scroll', function (e) {
        //var filter_pos = $('.game-filter').offset().top;
        if ($(window).scrollTop() >= $('.page-section:first-of-type').height()) {
            // page-section:first
            $('.game-filter').addClass('filter-fixed');
        }
        else {
            $('.game-filter').removeClass('filter-fixed');
        }
    });

    $('body').on('click', '#free-play', function () {
        $(this).hide().remove();
    });

    // menu filtering to scroll to game area on mobile screens
    $('body').on('click', '.top-filter li a', function () {
        $('html, body').animate({
            scrollTop: ($('#game-area').offset().top)
        }, 1000);
    });

    $('body').on('click', '#login-link', function (e) {
        e.preventDefault();
        $('#register-form').addClass('slideOutLeft');
        $('#login-header').removeClass('hide').addClass('fadeIn');
        $('#register-header').addClass('hide');
        setTimeout(function () {
            $('#login-form').addClass('slideInRight').removeClass('hide');
            $('#login-header').removeClass('fadeIn');
            $('#register-form').addClass('hide').removeClass('slideOutLeft');
        }, 250);
    });

    $('body').on('click', '#register-link', function (e) {
        e.preventDefault();
        $('#login-form').addClass('slideOutLeft');
        $('#login-header').addClass('hide ');
        $('#register-header').removeClass('hide').addClass('fadeIn');
        setTimeout(function () {
            $('#register-form').addClass('slideInRight').removeClass('hide');
            $('#register-header').removeClass('fadeIn');
            $('#login-form').addClass('hide').removeClass('slideOutLeft');
        }, 250);
    });

    $('body').on('click', '#register-target', function (e) {
        $('#register-form').removeClass('hide');
        $('#login-header').addClass('hide');
        $('#register-header').removeClass('hide');
        $('#login-form').addClass('hide');
    });

    $('body').on('click', '#login-target', function (e) {
        $('#register-form').addClass('hide');
        $('#login-header').removeClass('hide');
        $('#register-header').addClass('hide');
        $('#login-form').removeClass('hide');
    });

    //$(window).on('load',function(){
    //  $('#exampleModal').modal('show');
    //});

    $("#slideshow > div:gt(0)").hide();

    setInterval(function () {
        $('#slideshow > div:first')
          .fadeOut(1000)
          .next()
          .fadeIn(1000)
          .end()
          .appendTo('#slideshow');
    }, 3000);

    $('body').find('.lazy .img-responsive').lazyload({});

    $('body').on('click', '#free-play', function () {
        $(this).hide().remove();
        $('body').find('.game-container').addClass('play');
    });

    $('.animate').scrolla({
        mobile: true,
        once: false
    });


});