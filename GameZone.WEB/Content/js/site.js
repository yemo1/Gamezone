$(document).ready(function(){
    
    $(window).scroll(function () {
             if ($(window).scrollTop() >= $('.page-section:first-of-type').height()) {
               // page-section:first
               $('header').addClass('sticky')
             }
               else
               {
                 $('header').removeClass('sticky');
               }
    });

    $('body').on('click', '#free-play', function () {
        $(this).hide().remove();
    });

    $('body').on('click', '#login-link', function (e) {
        e.preventDefault();
        $('#register-form').addClass('slideOutLeft');
        $('#login-header').removeClass('hide').addClass('fadeIn');
        $('#register-header').addClass('hide');
        $('#forgotpw-header').addClass('hide');
        $('#forgotpw-form').addClass('slideOutLeft');
        setTimeout(function () {
            $('#login-form').addClass('slideInRight').removeClass('hide');
            $('#login-header').removeClass('fadeIn');            
            $('#register-form').addClass('hide').removeClass('slideOutLeft');
            $('#forgotpw-form').addClass('hide').removeClass('slideOutLeft');
        }, 250);
    });

    $('body').on('click', '#login-target', function (e) {
        $('#register-form').addClass('hide');
        $('#login-header').removeClass('hide');
        $('#register-header').addClass('hide');
        $('#login-form').removeClass('hide');
    });

    $('body').on('click', '#forgotpw-link', function (e) {
        e.preventDefault();
        $('#login-form').addClass('slideOutLeft');
        $('#forgotpw-header').removeClass('hide').addClass('fadeIn');
        $('#login-header').addClass('hide');
        setTimeout(function () {
            $('#forgotpw-form').addClass('slideInRight').removeClass('hide');
            $('#forgotpw-header').removeClass('fadeIn');
            $('#login-form').addClass('hide').removeClass('slideOutLeft');
        }, 250);
    });


    $('body').on('click', '#login-target', function (e) {
        $('#register-form').addClass('hide');
        $('#login-header').removeClass('hide');
        $('#register-header').addClass('hide');
        $('#login-form').removeClass('hide');
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