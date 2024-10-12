﻿$(document).ready(function () {

    var category_id = 39;
    const page = 1;
    // Load tin trên trang chủ 
    news.bin_news_home(category_id, page);
})

var news = {
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