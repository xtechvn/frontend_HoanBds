using HoanBds.Contants;
using HoanBds.Models;
using HoanBds.Models.GroupProduct;
using HoanBds.Service.ElasticSearch.News;
using HoanBds.Service.Redis;
using HoanBds.Utilities;
using HoanBds.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace HoanBds.Controllers.News.Service
{
    public class NewsService
    {
        private readonly IConfiguration configuration;
        private readonly RedisConn redisService;
        private readonly ArticleService article_es;

        public NewsService(IConfiguration _configuration, RedisConn _redisService)
        {
            configuration = _configuration;
            redisService = _redisService;
            article_es = new ArticleService(configuration["Elastic:Host"], _configuration);
        }
        /// <summary>
        /// Chi tiết bài viết
        /// </summary>
        /// <param name="article_id"></param>
        /// <returns></returns>
        public async Task<ArticleModel?> getArticleDetailById(long article_id)
        {
            try
            {
                int node_redis = Convert.ToInt32(configuration["Redis:Database:db_article"]);
                var article_detail = new ArticleModel();

                string cache_name = CacheType.ARTICLE_ID + article_id;
                var j_data = await redisService.GetAsync(cache_name, node_redis);

                // Kiểm tra có trong cache không ?
                if (!string.IsNullOrEmpty(j_data))
                {
                    article_detail = JsonConvert.DeserializeObject<ArticleModel>(j_data);
                }
                else // Không có trong cache
                {
                    article_detail = await article_es.GetArticleDetailById(article_id);

                    if (article_detail != null)
                    {
                        redisService.Set(cache_name, JsonConvert.SerializeObject(article_detail), node_redis);
                    }
                }
                return article_detail;
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                Utilities.LogHelper.InsertLogTelegramByUrl(configuration["log_telegram:token"], configuration["log_telegram:group_id"], error_msg);
                return null;
            }
        }

        /// <summary>
        /// Lấy ra các tin mới nhất trang chủ dc set top của tất cả các chuyên mục
        /// </summary>
        /// <param name="category_id"></param>
        /// <param name="top"></param>ssjujuuj
        /// <param name="skip"></param>
        /// <returns></returns>
        public async Task<ArticleViewModel?> getListNews(int category_id, int skip, int top)
        {
            try
            {
                int node_redis = Convert.ToInt32(configuration["Redis:Database:db_article"]);
                /*                var _category_detail = new GroupProductModel();*/
                var list_article = new List<CategoryArticleModel>();
                int total_max_cache = 100; // số bản ghi tối đa để cache                    

                string cache_name = Contants.CacheType.ARTICLE_CATEGORY_ID + category_id + skip + top;
                var j_data = await redisService.GetAsync(cache_name, node_redis);

                // Kiểm tra có trong cache không ?
                if (!string.IsNullOrEmpty(j_data))
                {
                    list_article = JsonConvert.DeserializeObject<List<CategoryArticleModel>>(j_data);
                    // Nếu tổng số bản ghi muốn lấy vượt quá số bản ghi trong Redis thì vào ES lấy                        
                    if (top > list_article.Count())
                    {
                        // Lấy ra trong es
                        list_article = await article_es.getListNews(category_id, top);
                    }
                }
                else // Không có trong cache
                {
                    // Lấy ra số bản ghi tối đa để cache
                    list_article = await article_es.getListNews(category_id, Math.Max(total_max_cache, top));

                    if (list_article.Count() > 0)
                    {
                        redisService.Set(cache_name, JsonConvert.SerializeObject(list_article), node_redis);
                    }
                }

                if (list_article != null && list_article.Count() > 0)
                {

                    var model = new ArticleViewModel
                    {
                        obj_article_list = list_article.Skip(skip).Take(top).ToList()
                    };

                    return model;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                Utilities.LogHelper.InsertLogTelegramByUrl(configuration["log_telegram:token"], configuration["log_telegram:group_id"], error_msg);
                return null;
            }
        }

        /// <summary>
        /// Tổng bài viết của 1 cate để phân trang
        /// </summary>
        /// <param name="category_id"></param>
        /// <returns></returns>
        public async Task<int> getTotalNews(int category_id)
        {
            try
            {
                // Lấy ra trong es
                var total = await article_es.getTotalItemNewsByCategoryId(category_id);
                return total;
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                Utilities.LogHelper.InsertLogTelegramByUrl(configuration["log_telegram:token"], configuration["log_telegram:group_id"], error_msg);
                return 0;
            }
        }

    }
}
