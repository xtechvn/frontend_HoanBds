using Amazon.Runtime.Internal.Util;
using HoanBds.Contants;
using HoanBds.Controllers.News.Service;
using HoanBds.Models;
using HoanBds.Service.MongoDb;
using HoanBds.Service.Redis;
using HoanBds.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Nest;
using System.Reflection;

namespace HoanBds.ViewComponents.Article
{
    public class ArticleMostViewedViewComponent : ViewComponent
    {
        private readonly IConfiguration configuration;
        private readonly RedisConn redisService;
        private readonly IMemoryCache _cache; // Inject IMemoryCache
        public ArticleMostViewedViewComponent(IConfiguration _Configuration, RedisConn _redisService, IMemoryCache cache)
        {
            configuration = _Configuration;
            redisService = _redisService;
            _cache = cache;
        }

        /// <summary>       
        /// type_view = 0 : box footer trang chủ | 1: box news
        /// zone_info: là 1 chuỗi json dựa vào đây để hiển thị tin theo cấu hình
        /// </summary>
        /// <returns>Load cac bai viet theo chuyen muc</returns>
        public async Task<IViewComponentResult?> InvokeAsync(CategoryConfigModel _zone_info) //(int category_id,int top,int type_view)
        {
            try
            {
                var cacheKey = "NEWS_RIGHT_VIEW" + _zone_info.category_id + _zone_info.skip + _zone_info.take; //Cache view trang tin home

                if (!_cache.TryGetValue(cacheKey, out var cached_view)) // Kiểm tra xem có trong cache không
                {
                    NewsMongoService newsMongoService = new NewsMongoService(configuration);
                    ArticleViewModel articleViewModel = new ArticleViewModel();
                    articleViewModel.obj_article_list = new List<CategoryArticleModel>();
                    var list = await newsMongoService.GetMostViewedArticle();
                    var obj_cate = new NewsService(configuration, redisService);
                    if (list != null && list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            var article = await obj_cate.getArticleDetailById(item.articleID);

                            if (article != null) 
                            {
                                var articelmodel = new CategoryArticleModel()
                                {
                                    id = article.id,
                                    title = article.title,
                                    lead = article.lead,
                                    image_169 = article.image_169,
                                    image_43 = article.image_43,
                                    image_11 = article.image_11,
                                };
                                articleViewModel.obj_article_list.Add(articelmodel);
                                // Lưu vào cache với thời gian hết hạn dc set. 
                            }
                        }
                        _cache.Set(cacheKey, articleViewModel, TimeSpan.FromSeconds(Convert.ToInt32(configuration["redis:cate_time_view:second_list_box_news"])));
                        return articleViewModel != null ? View(_zone_info.view_name, articleViewModel) : Content("");
                    }

                    if (cached_view != null)
                    {
                        // Lưu vào cache với thời gian hết hạn dc set. 
                        _cache.Set(cacheKey, cached_view, TimeSpan.FromSeconds(Convert.ToInt32(configuration["redis:cate_time_view:second_list_box_news"])));
                    }
                }
                return cached_view != null ? View(_zone_info.view_name, cached_view) : Content("");

            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                Utilities.LogHelper.InsertLogTelegramByUrl(configuration["log_telegram:token"], configuration["log_telegram:group_id"], error_msg);
                return Content("");
            }
        }
    }
}
