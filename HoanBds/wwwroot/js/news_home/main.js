$(document).ready(function () {

    var category_id = -1;
    const page = 1;
    // Load tin trên trang chủ 
    _news_home.bin_news_home(category_id, page);
})

var _news_home = {
    bin_news_home: function (category_id, page) {

        $.ajax({
            dataType: 'html',
            type: 'POST',
            url: '/news/home/get-article-list.json',
            data: { category_id: category_id, page: page, view_name: "~/Views/Shared/Components/News/TopHome.cshtml" },
            success: function (data) {
                $('.list-news-home').html(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error); // Thay đổi từ 'failure' sang 'error'
            }
        });

    },
}