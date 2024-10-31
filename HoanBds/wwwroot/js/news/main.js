$(document).ready(function () {

    var category_id = parseInt($(".category_id").data("categoryid"));

    const query_string = window.location.search;
    // Khởi tạo URLSearchParams để xử lý query string
    const url_params = new URLSearchParams(query_string);
    // Load tin trên trang chủ NEWS
    news.bin_news_home(category_id, page = url_params.get('page') == null ? 1 : url_params.get('page'));   
    news.MostViewd_news();
})

$(document.body).on('click', '.expand-news', function (e) {
    var total_item = $(".total_items").val();
    var total_page = Math.ceil(total_item / 10);
    const query_string = window.location.search;
    // Khởi tạo URLSearchParams để xử lý query string
    const url_params = new URLSearchParams(query_string);
    // Lấy giá trị của tham số 'page'
    var pageIndex = url_params.get('page') == null ? 1 : url_params.get('page');
    
    if (pageIndex == total_page) {

    } else
    {
        
        pageIndex = parseInt(pageIndex) + 1;
        window.location.href = '/tin-tuc-61?page=' + pageIndex;
    }
});

var news = {
    bin_news_home: function (category_id, page) {

        $.ajax({
            dataType: 'html',
            type: 'POST',
            url: '/news/home/get-article-list.json',
            data: { category_id: category_id, page: page, view_name: "~/Views/Shared/Components/News/Home.cshtml" },
            success: function (data) {
                $('.list-news-home').html(data);

                //check xem có còn bản ghi nữa không,nếu không thì ẩn nút xem thêm
                const query_string = window.location.search;
                const url_params = new URLSearchParams(query_string);
                var total_item = $(".total_items").val();
                var total_page = Math.ceil(total_item / 10);
                var pageIndex = url_params.get('page') == null ? 1 : url_params.get('page');
                if (pageIndex == total_page) {
                    $(".expand-news").remove();
                }
                if (pageIndex >= 2)
                {
                    var active_index = (pageIndex - 1) * 10 - 3;
                    var active_item = $('.list-news .masonry-item:eq(' + active_index + ')');
                    const offset = active_item.offset().top;

                    // Cuộn đến vị trí đó
                    $('html, body').animate({
                        scrollTop: offset
                    }, 500);
                }
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error); // Thay đổi từ 'failure' sang 'error'
            }
        });

    },
    MostViewd_news: function (category_id, page) {

        $.ajax({
            dataType: 'html',
            type: 'POST',
            url: 'get-most-viewed-article.json',
            data: { category_id: -1, page: 1, view_name: "~/Views/Shared/Components/News/TopRight.cshtml" },
            success: function (data) {
                $('#list-news-most-view').append(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error); // Thay đổi từ 'failure' sang 'error'
            }
        });

    },
}