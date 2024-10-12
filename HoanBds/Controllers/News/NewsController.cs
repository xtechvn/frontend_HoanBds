// Create By: cuonglv
// Create Date: 09-10-2024

using HoanBds.Controllers.News.Service;
using HoanBds.Models;
using HoanBds.Service.Redis;
using Microsoft.AspNetCore.Mvc;

namespace HoanBds.Controllers.News
{
    public class NewsController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RedisConn redisService;
        public NewsController(IConfiguration _configuration, RedisConn _redisService)
        {
            configuration = _configuration;
            redisService = _redisService;
        }
        
        /// <summary>
        /// action này load ra trang chủ news
        /// </summary>
        /// <param name="path"></param>
        /// <param name="category_id"></param>
        /// <param name="page"></param>
        /// <param name="category_path_child"></param>
        /// <returns></returns>
        [Route("tin-tuc")]
        [HttpGet]
        public async Task<IActionResult> Index(string path, int category_id, int page = 1, string category_path_child = "")
        {
            // Khởi tạo các param phân vào các ViewComponent
            var article_sv = new NewsService(configuration, redisService);

            ViewBag.category_id = -1;// Convert.ToInt32(configuration["menu:news_parent_id"]);
            ViewBag.page = page;
            ViewBag.page_size = Convert.ToInt32(configuration["blognews:page_size"]);
            ViewBag.total_items = await article_sv.getTotalNews(-1); // Lấy ra tổng toàn bộ bản ghi theo chuyên mục
            return View();
        }

        /// <summary>
        /// Trang chuyên mục
        /// </summary>
        /// <param name="path"></param>
        /// <param name="category_path_child"></param>
        /// <param name="category_id"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [Route("tin-tuc/{category_path_child}/{category_id}")]
        [HttpGet]
        public async Task<IActionResult> Category(string path, string category_path_child, int category_id, int page = 1)
        {
            var article_sv = new NewsService(configuration, redisService);
            ViewBag.category_id = category_id;
            ViewBag.page_size = Convert.ToInt32(configuration["blognews:page_size"]);
            ViewBag.total_items = await article_sv.getTotalNews(category_id); // Lấy ra tổng toàn bộ bản ghi theo chuyên mục
            ViewBag.page = page;

            // Chung view với trang chủ news
            return View("~/Views/News/Index.cshtml");
        }

        /// <summary>
        /// Lấy ra chi tiết bài viết
        /// </summary>
        /// <param name="title"></param>
        /// <param name="article_id"></param>
        /// <returns></returns>
        [Route("{title}-{article_id}.html")]
        [HttpGet]
        public async Task<IActionResult> ArticleDetail(string title, long article_id)
        {
            var article_sv = new NewsService(configuration, redisService);
            var article = await article_sv.getArticleDetailById(article_id);
            return View("~/Views/News/ArticleDetail.cshtml", article);
        }

        /// <summary>
        /// Lấy ra các bài viết mới nhất của các chuyên mục
        /// Lấy ra các bài theo chuyên mục
        /// </summary>
        /// <param name="category_id">-1 là quét all các bài viết của các mục, >0 lấy theo chuyên mục</param>        
        /// <returns></returns>
        [Route("news/home/get-article-list.json")]
        [HttpPost]
        public async Task<IActionResult> getArticleListByCategoryIdComponent(int category_id, string view_name, int page)
        {
            try
            {
                // Tính phân trang load tin
                int page_size = Convert.ToInt32(configuration["blognews:page_size"]);
                page = page == 0 ? 1 : page;
                int skip = (page - 1) * page_size;

                var model = new CategoryConfigModel
                {
                    category_id = category_id,                
                    view_name = view_name,                    
                    skip = skip,
                    take = page_size
                };
                return ViewComponent("ArticleList", model);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                //_logger.LogError(ex, "Error loading header component");
                return StatusCode(500); // Trả về lỗi 500 nếu có lỗi
            }
        }
    }
}
