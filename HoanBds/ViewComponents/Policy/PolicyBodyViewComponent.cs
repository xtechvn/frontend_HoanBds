using HoanBds.Controllers.Home.Service;
using HoanBds.Controllers.News.Service;
using HoanBds.Models;
using HoanBds.Service.MongoDb;
using HoanBds.Service.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace HoanBds.ViewComponents.Policy
{
    public class PolicyBodyViewComponent : ViewComponent // Chỉnh sửa tên class
    {
        private readonly IConfiguration _configuration;
        private readonly RedisConn _redisService;
        private readonly IMemoryCache _cache; // Inject IMemoryCache
        public PolicyBodyViewComponent(IConfiguration configuration, RedisConn redisService, IMemoryCache cache)
        {
            _configuration = configuration;
            _redisService = redisService;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync(long Policy_Id)
        {
            try
            {
                var cacheKey = "Policy_" + Policy_Id;

                if (!_cache.TryGetValue(cacheKey, out var cached_view)) // Kiểm tra xem có trong cache không
                {
                    var obj_cate = new NewsService(_configuration, _redisService);

                    // Tin theo chuyên mục
                    cached_view = await obj_cate.getListNews((int)Policy_Id,0,10);//giới hạn chỉ hiển thị ra 10 bài/1 policy

                    if (cached_view != null)
                    {
                        // Lưu vào cache với thời gian hết hạn dc set. 
                        _cache.Set(cacheKey, cached_view, TimeSpan.FromSeconds(Convert.ToInt32(_configuration["redis:cate_time_view:second_list_box_news"])));
                    }
                }
                return View("~/Views/Shared/Components/Policyfooter/BodyPolicy.cshtml", cached_view);

            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                Utilities.LogHelper.InsertLogTelegramByUrl(_configuration["log_telegram:token"], _configuration["log_telegram:group_id"], error_msg);
                return Content("");
            }
        }
    }
}
