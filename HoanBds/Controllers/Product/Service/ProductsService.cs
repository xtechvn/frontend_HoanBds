using HoanBds.Contants;
using HoanBds.Models.Products;
using HoanBds.Service.MongoDb;
using HoanBds.Service.Redis;
using HoanBds.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HoanBds.Controllers.Product.Service
{
    public class ProductsService
    {
        private readonly IConfiguration configuration;
        private readonly RedisConn redisService;
        private readonly ProductDetailMongoAccess _productDetailMongoAccess;

        public ProductsService(IConfiguration _configuration, RedisConn _redisService)
        {
            configuration = _configuration;
            _productDetailMongoAccess = new ProductDetailMongoAccess(_configuration);
            redisService = _redisService;
        }

        /// <summary>
        /// Load danh sách menu, category, ngành hàng
        /// </summary>
        /// <param name="parent_id"></param>
        /// <returns>
        public async Task<ProductListResponseModel?> getProductListByGroupProductId(int group_product_id, int page_index, int page_size)
        {
            try
            {
                var connect_api_us = new ConnectApi(configuration, redisService);
                var input_request = new Dictionary<string, int>
                {
                    {"group_id",group_product_id },
                    {"page_index",page_index},
                    {"page_size",page_size}
                };
                var response_api = await connect_api_us.CreateHttpRequest("/api/product/get-list.json", input_request);

                // Nhan ket qua tra ve                            
                var JsonParent = JArray.Parse("[" + response_api + "]");
                int status = Convert.ToInt32(JsonParent[0]["status"]);

                if (status == ((int)ResponseType.SUCCESS))
                {
                    string data = JsonParent[0]["data"].ToString();
                    int total = Convert.ToInt32(JsonParent[0]["total"]);
                    var model = new ProductListResponseModel
                    {
                        items = JsonConvert.DeserializeObject<List<ProductMongoDbModel>>(data),
                        count = total,
                        page_index = page_index,
                        page_size = page_size
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
                Utilities.LogHelper.InsertLogTelegramByUrl(configuration["log_telegram:Token"], configuration["log_telegram:GroupId"], "getListMenuHelp " + ex.Message);
                return null;
            }
        }
        public async Task<ProductDetailResponseModel?> GetProductDetail(string product_id)
        {
            try
            {
                int node_redis = Convert.ToInt32(configuration["Redis:Database:db_search_result"]);

                var cache_name = CacheType.PRODUCT_DETAIL + product_id;
                var j_data = await redisService.GetAsync(cache_name, node_redis);
                if (j_data != null && j_data.Trim() != "")
                {
                    var result = JsonConvert.DeserializeObject<ProductDetailResponseModel>(j_data);
                    if (result != null)
                    {
                        return result;
                    }
                }
                var data = await _productDetailMongoAccess.GetFullProductById(product_id);
                if (data != null)
                {
                    redisService.Set(cache_name, JsonConvert.SerializeObject(data), node_redis);
                }
                return data;
            }
            catch (Exception ex)
            {
                Utilities.LogHelper.InsertLogTelegramByUrl(configuration["log_telegram:Token"], configuration["log_telegram:GroupId"], "GetProductDetail " + ex.Message);
                return null;
            }
        }
        public async Task<ProductListResponseModel> GetProductList(ProductListRequestModel request)
        {
            try
            {
                //var request = JsonConvert.DeserializeObject<ProductListRequestModel>(objParr[0].ToString());
                int node_redis = Convert.ToInt32(configuration["Redis:Database:db_search_result"]);
                var obj_product = new ProductListResponseModel();
                int total_max_cache = 100; // số bản ghi tối đa để cache    
                int group_product_id = request.group_id;
                int skip = request.page_index;
                int top = request.page_size;

                string cache_name = CacheType.PRODUCT_LISTING + group_product_id;
                var j_data = await redisService.GetAsync(cache_name, node_redis);

                // Kiểm tra có trong cache không ?
                if (!string.IsNullOrEmpty(j_data))
                {
                    obj_product = JsonConvert.DeserializeObject<ProductListResponseModel>(j_data);
                    // Nếu tổng số bản ghi muốn lấy vượt quá số bản ghi trong Redis thì vào ES lấy                        
                    if (top > obj_product.items.Count())
                    {
                        // Lấy ra trong Mongo
                        var data = await _productDetailMongoAccess.ResponseListing(request.keyword, request.group_id, 0, top);
                    }
                }
                else // Không có trong cache
                {
                    // Lấy ra số bản ghi tối đa để cache                        
                    obj_product = await _productDetailMongoAccess.ResponseListing(request.keyword, request.group_id, 0, Math.Max(total_max_cache, top));

                    if (obj_product.count > 0)
                    {

                        redisService.Set(cache_name, JsonConvert.SerializeObject(obj_product), node_redis);
                    }
                }

                if (obj_product != null && obj_product.items.Count() > 0)
                {
                    var model = new ProductListResponseModel
                    {
                        count = obj_product.count,
                        items = obj_product.items.Skip(skip).Take(top).ToList()
                    };
                    return model;
                }
            }
            catch
            {
            }
            return null;

        }

        public async Task<List<ProductMongoDbModel>> Search(string keyword)
        {
            try
            {
                string response_api = string.Empty;
                var connect_api_us = new ConnectApi(configuration, redisService);
                var input_request = new Dictionary<string, string>
                {
                    {"keyword",keyword }
                };

                response_api = await connect_api_us.CreateHttpRequest("/api/product/search.json", input_request);

                // Nhan ket qua tra ve                            
                var JsonParent = JArray.Parse("[" + response_api + "]");
                int status = Convert.ToInt32(JsonParent[0]["status"]);

                if (status == ((int)ResponseType.SUCCESS))
                {
                    var product_list = JsonConvert.DeserializeObject<List<ProductMongoDbModel>>(JsonParent[0]["data"].ToString());
                    return product_list;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
