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

    $(window).on('load',function(){
      $('#exampleModal').modal('show');
    });

    $('.animate').scrolla({
      mobile: true,
      once: false
	});


});        