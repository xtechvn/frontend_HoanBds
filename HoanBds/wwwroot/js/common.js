 const swiperBanner = new Swiper('.swiper-banner', {
    loop: true,
    pagination: {
        el: '.swiper-banner .swiper-pagination',
    },
    navigation: {
        nextEl: '.swiper-banner .swiper-button-next',
        prevEl: '.swiper-banner .swiper-button-prev',
    },

});
const swiperService = new Swiper('.list-service-home', {
    pagination: false,
    slidesPerView: 1,
    spaceBetween: 20,
    loop: false,
    breakpoints: {
        540: {
            slidesPerView: 1,
        },
        768: {
            slidesPerView: 2,
        },
        992: {
            slidesPerView: 3,
            spaceBetween: 30,
        }
    },
    navigation: {
        nextEl: '.list-service-home .swiper-button-next',
        prevEl: '.list-service-home .swiper-button-prev',
    },

});

// gallery
var swiperSmallThumb = new Swiper(".thumb-small", {
    spaceBetween: 15,
    slidesPerView: 4,
    freeMode: true,
    watchSlidesProgress: true,
    navigation: {
        nextEl: '.thumb-big .swiper-button-next',
        prevEl: '.thumb-big .swiper-button-prev',
    },
});
var swiperBigThumb = new Swiper(".thumb-big", {
    spaceBetween: 15,
    navigation: {
        nextEl: '.thumb-big .swiper-button-next',
        prevEl: '.thumb-big .swiper-button-prev',
    },

    thumbs: {
        swiper: swiperSmallThumb,
    },
});

// custom select

$('select').each(function () {
    var $this = $(this), numberOfOptions = $(this).children('option').length;

    $this.addClass('select-hidden');
    $this.wrap('<div class="select"></div>');
    $this.after('<div class="select-styled"></div>');

    var $styledSelect = $this.next('div.select-styled');
    $styledSelect.text($this.children('option').eq(0).text());

    var $list = $('<ul />', {
        'class': 'select-options'
    }).insertAfter($styledSelect);

    for (var i = 0; i < numberOfOptions; i++) {
        $('<li />', {
            text: $this.children('option').eq(i).text(),
            rel: $this.children('option').eq(i).val()
        }).appendTo($list);
        if ($this.children('option').eq(i).is(':selected')) {
            $('li[rel="' + $this.children('option').eq(i).val() + '"]').addClass('is-selected')
        }
    }

    var $listItems = $list.children('li');

    $styledSelect.click(function (e) {
        e.stopPropagation();
        $('div.select-styled.active').not(this).each(function () {
            $(this).removeClass('active').next('ul.select-options').hide();
        });
        $(this).toggleClass('active').next('ul.select-options').toggle();
    });

    $listItems.click(function (e) {
        e.stopPropagation();
        $styledSelect.text($(this).text()).removeClass('active');
        $this.val($(this).attr('rel'));
        $list.find('li.is-selected').removeClass('is-selected');
        $list.find('li[rel="' + $(this).attr('rel') + '"]').addClass('is-selected');
        $list.hide();
    });

    $(document).click(function () {
        $styledSelect.removeClass('active');
        $list.hide();
    });
});


// /////////////////////////////////////////////
/* scroll menu top*/
var resizeTimer = false,
    resizeWindow = $(window).prop("innerWidth");
var parentHeight = $('.mainHeader--height').outerHeight();
var $header = $('.main-header');
var offset_sticky_header = $header.outerHeight() + 100;
var offset_sticky_down = 0;
$('.mainHeader--height').css('min-height', parentHeight);
$(window).on("resize", function () {
    if (resizeTimer) {
        clearTimeout(resizeTimer)
    }
    resizeTimer = setTimeout(function () {
        var newWidth = $(window).prop("innerWidth");
        if (resizeWindow != newWidth) {
            $header.removeClass('hSticky-up').removeClass('hSticky-down').removeClass('hSticky');
            $('.mainHeader--height').css('min-height', '');
            parentHeight = $('.mainHeader--height').outerHeight();
            $('.mainHeader--height').css('min-height', parentHeight);
            resizeWindow = newWidth
        }
    }, 200);
});
setTimeout(function () {
    $header.removeClass('hSticky-up').removeClass('hSticky-down').removeClass('hSticky');
    $('.mainHeader--height').css('min-height', '');
    parentHeight = $('.mainHeader--height').outerHeight();
    $('.mainHeader--height').css('min-height', parentHeight);
    jQuery(window).scroll(function () {
        /* scroll header */
        if (jQuery(window).scrollTop() > offset_sticky_header && jQuery(window).scrollTop() > offset_sticky_down) {
            if (jQuery(window).width() > 991) {
                $('body').removeClass('locked-scroll');
                $('.header-action-icon').removeClass('show-action');
            }
            $header.addClass('hSticky');
            if (jQuery(window).scrollTop() > offset_sticky_header + 150) {
                $header.removeClass('hSticky-up').addClass('hSticky-down');
                $('body').removeClass('bSticky-scroll');
            }
        }
        else {
            if (jQuery(window).scrollTop() > offset_sticky_header + 150 && (jQuery(window).scrollTop() - 150) + jQuery(window).height() < $(document).height()) {
                $header.addClass('hSticky-up');
                $('body').addClass('bSticky-scroll');
            }
        }
        if (jQuery(window).scrollTop() <= offset_sticky_down && jQuery(window).scrollTop() <= offset_sticky_header) {
            $header.removeClass('hSticky-up').removeClass('hSticky-down').removeClass('hSticky');
            $('body').removeClass('bSticky-scroll');
        }
        offset_sticky_down = jQuery(window).scrollTop();
    });
}, 300);
/* on click action icon header*/
$('.header-action-toggle').click(function (e) {
    e.preventDefault();
    if ($(this).parents('.header-action-icon').hasClass('show-action')) {
        $('body').removeClass('locked-scroll');
        $(this).parents('.header-action-icon').removeClass('show-action');
    }
    else {
        $('.header-action-icon').removeClass('show-action');
        $('body').addClass('locked-scroll');
        $(this).parents('.header-action-icon').addClass('show-action');
    }
});
// Menu mobile
$('.list-root li a').click(function(e){
    if ($(this).find('i').length){
        e.preventDefault();
        var menu_child_id = $(this).parent().data('menu-root');
        $('.list-root').addClass('mm-subopened');
        $('#' + menu_child_id).addClass('mm-opened');
    } 
})
$('.list-child li:first-child a').click(function(){
    $(this).parents('.list-child').removeClass('mm-opened');
    $('.list-root').removeClass('mm-subopened');
})
$('.list-child li.level-2 a').click(function(e){
    if ($(this).find('i').length){
        e.preventDefault();
        var menu_sub_id = $(this).parent().data('menu-root');
        $('li.level-2').addClass('mm-subopened');
        $('#' + menu_sub_id).addClass('mm-sub');
    } 
})
$('.sub-child li:first-child a').click(function(){
    $(this).parents('.sub-child').removeClass('mm-sub');
    $('.list-child').removeClass('mm-subopened');
})
$(document).on("click",".sub-child li.level-3 a",function(e){
    if ($(this).find('i').length){
        e.preventDefault();
        var menu_subnav_id = $(this).parent().data('menu-root');
        $('li.level-3').addClass('mm-open-3');
        $('#' +  menu_subnav_id).addClass('mm-sub-3');
    } 
});
$(document).on("click",".sub-child-3 li:first-child a",function(e){
    $(this).parents('.sub-child-3').removeClass('mm-sub-3');
    $('.sub-child').removeClass('mm-open-3');
});

function setAnimation ( _elem, _InOut ) {
    var animationEndEvent = 'webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend';
    _elem.each ( function () {
        var $elem = $(this);
        var $animationType = 'animated ' + $elem.data( 'animation-' + _InOut );

        $elem.addClass($animationType).one(animationEndEvent, function () {
            $elem.removeClass($animationType); 
        });
    });
}

// scroll top
function scrollTop() {
    if ($(window).scrollTop() > 500) {
        $(".backToTopBtn").addClass("active");
    } else {
        $(".backToTopBtn").removeClass("active");
    }
}
$(function () {
    scrollTop();
    $(window).on("scroll", scrollTop);

    $(".backToTopBtn").click(function () {
        $("html, body").animate({ scrollTop: 0 }, 1);
        return false;
    });
});
/* show desc product short*/
var expandable_content_height = $('.expandable-toggle .description-productdetail').height();
if(expandable_content_height > 130){
    $('.expandable-toggle .description-productdetail').css({
        "height": "130px",
        "overflow": "hidden"
    });
}else{
    $('.expandable-content_toggle').addClass('hidden');
}
$('body').on('click', '.js_expandable_content', function (e) {
    if (!$('.expandable-toggle').hasClass('opened')) {		
        $('.expandable-content_toggle').removeClass('btn-closemore').addClass('btn-viewmore').find('.expandable-content_toggle-text').html('Xem thêm');
        var curHeight = $('.expandable-toggle .description-productdetail').height();
        expandable_content_height = 135;
        $('.expandable-toggle .description-productdetail').height(curHeight).animate({
            height: expandable_content_height
        }, 300,  function () {
            $(this).parent().addClass('opened');
        });
        if ($(window).width() < 992) {
            $('html, body').animate({
                scrollTop: $(".product-content-desc").offset().top - 40
            }, 250);
        }
    } 
    else {
        $('.expandable-toggle .description-productdetail').css('height', 'auto');
        $('.expandable-toggle').removeClass('opened');
        $('.expandable-content_toggle').removeClass('btn-viewmore').addClass('btn-closemore').find('.expandable-content_toggle-text').html('Rút gọn');
    }
}); 