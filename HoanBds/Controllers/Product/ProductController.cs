using HoanBds.Controllers.Product.Service;
using HoanBds.Models.Products;
using HoanBds.Service.Redis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace HoanBds.Controllers.Product
{
    public class ProductController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly RedisConn redisService;
        private readonly ProductsService productsService;
        private readonly IMemoryCache _cache; // Inject IMemoryCache

        public ProductController(IConfiguration _configuration, RedisConn _redisService, IMemoryCache cache)
        {
            configuration = _configuration;
            redisService = _redisService;
            productsService=new ProductsService(_configuration, redisService);
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///Sản phẩm nổi bật
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult loadProductTopComponent(long group_product_id)
        {
            try
            {  
                return ViewComponent("ProductList");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần             
                return StatusCode(500); // Trả về lỗi 500 nếu có lỗi
            }
        }
        [HttpGet("san-pham/{title}-{product_code}.html")]
        public async Task<ActionResult> Detail(string title, string product_code)
        {
            var request = new ProductDetailRequestModel
            {
                id = product_code
            };
            var obj_detail = await productsService.GetProductDetail(product_code);
            ViewBag.ProductCode = product_code;
            return View(obj_detail);

        }

        [HttpPost("san-pham/search")]
        public async Task<ActionResult> getListGroupProduct(int _group_product_id, int? pricecode,int _page_index, int? districtcode,int? typecode,int _page_size, string view_name)
        {
            try
            {                
                return ViewComponent("ProductSearchList", new { _group_product_id, pricecode, _page_index, districtcode, typecode, _page_size, view_name });
            }
            catch (Exception ex)
            {
                return StatusCode(500); // Trả về lỗi 500 nếu có lỗi                
            }
        }
    }
}
