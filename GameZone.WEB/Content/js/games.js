$( document ).ready(function( $ ) {
    $( '#example2' ).sliderPro({
        width: 300,
        height: 300,
        visibleSize: '100%',
        forceSize: 'fullWidth',
        autoSlideSize: true
    });

    // instantiate fancybox when a link is clicked
    // $( '#example2 .sp-image' ).parent( 'a' ).on( 'click', function( event ) {
    //     event.preventDefault();

    //     // check if the clicked link is also used in swiping the slider
    //     // by checking if the link has the 'sp-swiping' class attached.
    //     // if the slider is not being swiped, open the lightbox programmatically,
    //     // at the correct index
    //     if ( $( '#example2' ).hasClass( 'sp-swiping' ) === false ) {
    //         $.fancybox.open( $( '#example2 .sp-image' ).parent( 'a' ), { index: $( this ).parents( '.sp-slide' ).index() } );
    //     }
    // });
});