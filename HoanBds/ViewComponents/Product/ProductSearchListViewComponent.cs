﻿using HoanBds.Controllers.Product.Service;
using HoanBds.Service.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace HoanBds.ViewComponents.Product
{
    public class ProductSearchListViewComponent : ViewComponent
    {
        private readonly IConfiguration configuration;
        private readonly RedisConn _redisService;
        private readonly IMemoryCache _cache; // Inject IMemoryCache
        private readonly ProductsService productsService; // Inject IMemoryCache
        public ProductSearchListViewComponent(IConfiguration _Configuration, RedisConn redisService, IMemoryCache cache)
        {
            configuration = _Configuration;
            _redisService = redisService;
            _cache = cache;
            productsService = new ProductsService(_Configuration, redisService);
        }

        /// <summary>
        // load ra data sản phẩm theo nhóm
        /// </summary>
        /// <returns>group_product_id: id của nhóm</returns>
        public async Task<IViewComponentResult?> InvokeAsync(int _group_product_id, int? pricecode, int _page_index, int? districtcode, int? typecode, int _page_size, string view_name)
        {
            try
            {
                float amount_min = 0;
                float amount_max = 0;
                if (pricecode != null) 
                {
                    switch (pricecode) 
                    {
                        case 1: 
                            amount_min = 1;
                            amount_max = 10000000000;
                            break;
                        case 2:
                            amount_min = 10000000000;
                            amount_max = 20000000000;
                            break;
                        case 3:
                            amount_min = 20000000000;
                            amount_max = 30000000000;
                            break;
                        case 4:
                            amount_min = 30000000000;
                            amount_max = float.MaxValue;
                            break;
                        default:
                            amount_min = 30000000001;
                            amount_max = float.MaxValue;
                            break;
                    }
                }

                if (districtcode != null)
                {
                    _group_product_id = districtcode.Value;
                }
                else if (typecode != null) 
                {
                    _group_product_id = typecode.Value;
                }
                // Nếu không có trong cache, gọi dịch vụ
                var cacheKey = "product_list_" + _group_product_id + "_" + _page_index + _page_size + pricecode; // Đặt khóa cho cache
                if (!_cache.TryGetValue(cacheKey, out var cached_view)) // Kiểm tra xem có trong cache không
                {
                    var objMenu = new ProductsService(configuration, _redisService);
                    cached_view = await productsService.ListingByPriceRange(amount_min,amount_max, _group_product_id, 1, _page_size);
                    if (cached_view != null)
                    {
                        // Lưu vào cache với thời gian hết hạn 
                        _cache.Set(cacheKey, cached_view, TimeSpan.FromSeconds(20));
                    }
                }
                return View(view_name, cached_view);
            }
            catch (Exception)
            {
                return Content("");
            }
        }
    }
}