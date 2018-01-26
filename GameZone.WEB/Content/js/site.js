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
        $('#login-form').removeClass('hide').addClass('slideInRight');
        $('#login-header').removeClass('hide').addClass('fadeIn');
        $('#register-header').addClass('hide');       
        setTimeout(function () {
            $('#login-header').removeClass('fadeIn');            
            $('#register-form').addClass('hide').removeClass('slideOutLeft');
        }, 500);
    });

    $('body').on('click', '#register-link', function (e) {
        e.preventDefault();
        $('#login-form').addClass('slideOutLeft');
        $('#register-form').removeClass('hide').addClass('slideInRight');
        $('#login-header').addClass('hide ');
        $('#register-header').removeClass('hide').addClass('fadeIn');
        setTimeout(function () {
            $('#register-header').removeClass('fadeIn');
            $('#login-form').addClass('hide').removeClass('slideOutLeft');
        }, 500);
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