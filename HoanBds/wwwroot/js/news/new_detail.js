$(document).ready(function () {
    news_detail.topRightNews();
});

var news_detail =
{
    topRightNews: function ()
    {
        $.ajax({
            dataType: 'html',
            type: 'POST',
            url: '/news/home/get-article-list.json',
            data: { category_id: -1, page: 1, view_name: "~/Views/Shared/Components/News/TopRight.cshtml" },
            success: function (data) {
                $('.list-news-latest').html(data);
            },
            error: function (xhr, status, error) {
                console.log("Error: " + error); // Thay đổi từ 'failure' sang 'error'
            }
        });
    }
}