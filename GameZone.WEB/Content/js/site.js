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
        console.log("fia!!");
        $(this).hide().remove();
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