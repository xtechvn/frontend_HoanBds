$(document).ready(function () {

    var category_id = parseInt($(".category_id").data("categoryid"));

    const query_string = window.location.search;
    // Khởi tạo URLSearchParams để xử lý query string
    const url_params = new URLSearchParams(query_string);
    var pageIndex = url_params.get('page') == null ? 1 : url_params.get('page')
    // Load tin trên trang chủ NEWS
    news.bin_news_home(category_id, pageIndex );   
    news.MostViewd_news();

    news.getList(category_id, pageIndex); 
    $(document.body).on('click', '.expand-news', function (e) {
        pageIndex = pageIndex + 1;
        news.getList(category_id, pageIndex);   
    });
})



var news = {
    bin_news_home: function (category_id, page) {

        $.ajax({
            dataType: 'html',
            type: 'POST',
            url: '/news/home/get-article-list.json',
            data: { category_id: category_id, page: page, view_name: "~/Views/Shared/Components/News/Home.cshtml" },
            success: function (data) {
                $('.list-news-page').prepend(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error); // Thay đổi từ 'failure' sang 'error'
            }
        });

    },

    getList: function (category_id, page)
    {
        $.ajax({
            dataType: 'html',
            type: 'POST',
            url: '/news/home/get-article-list',
            data: { category_id: category_id, page: page, view_name: "~/Views/Shared/Components/News/BodyHome.cshtml" },
            success: function (data) {
                $('#list-body-news').append(data);

                //check xem có còn bản ghi nữa không,nếu không thì ẩn nút xem thêm
                var total_item = $(".total_items").val();
                var total_page = Math.ceil(total_item / 10);
                var pageIndex = page;
                if (pageIndex == total_page) {
                    $(".expand-news").remove();
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