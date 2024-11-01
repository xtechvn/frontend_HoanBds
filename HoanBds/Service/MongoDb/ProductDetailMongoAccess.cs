﻿using HoanBds.Models.Products;
using HoanBds.Utilities;
using HoanBds.Utilities.contants;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Reflection;
using System.Text.Json.Serialization;

namespace HoanBds.Service.MongoDb
{
    public class ProductDetailMongoAccess
    {
        private readonly IConfiguration _configuration;
        private IMongoCollection<ProductMongoDbModel> _productDetailCollection;

        public ProductDetailMongoAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            string url = "mongodb://" + configuration["MongoServer:user"] +
                 ":" + configuration["MongoServer:pwd"] +
                 "@" + configuration["MongoServer:Host"] +
                 ":" + configuration["MongoServer:Port"] +
                 "/?authSource=" + configuration["MongoServer:catalog"] + "";

            var client = new MongoClient(url);
            IMongoDatabase db = client.GetDatabase(configuration["MongoServer:catalog"]);
            _productDetailCollection = db.GetCollection<ProductMongoDbModel>("ProductDetail");
        }

        public async Task<string> UpdateAsync(ProductMongoDbModel model)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.And(
                    filter.Eq("_id", model._id));
                await _productDetailCollection.FindOneAndReplaceAsync(filterDefinition, model);
                return model._id;
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegramByUrl(_configuration["log_telegram:token"], _configuration["log_telegram:group_id"], error_msg);
                return null;
            }
        }


        public async Task<ProductMongoDbModel> GetByID(string id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x._id, id); ;
                var model = await _productDetailCollection.Find(filterDefinition).FirstOrDefaultAsync();
                return model;
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegramByUrl(_configuration["log_telegram:token"], _configuration["log_telegram:group_id"], error_msg);
                return null;
            }
        }


        public async Task<List<ProductMongoDbModel>> Listing(string keyword = "", int group_id = -1, int page_index = 1, int page_size = 10)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                if (keyword != null && keyword.Trim() != "")
                {
                    filterDefinition &= Builders<ProductMongoDbModel>.Filter.Regex(x => x.name, keyword);

                }
                if (group_id > 0)
                {
                    filterDefinition &= Builders<ProductMongoDbModel>.Filter.Regex(x => x.group_product_id, group_id.ToString());
                }
                var sort_filter = Builders<ProductMongoDbModel>.Sort;
                var sort_filter_definition = sort_filter.Descending(x => x.updated_last);
                var model = _productDetailCollection.Find(filterDefinition);
                model.Options.Skip = page_index < 1 ? 0 : (page_index - 1) * page_size;
                model.Options.Limit = page_size;
                var result = await model.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegramByUrl(_configuration["log_telegram:token"], _configuration["log_telegram:group_id"], error_msg);
                return null;
            }
        }
        public async Task<ProductListResponseModel> ListingByPriceRange(double amount_min, double amout_max, int group_product_id = -1, int page_index = 1, int page_size = 12,int? districtCode = null)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Regex(x => x.group_product_id, group_product_id.ToString());
               
                if (districtCode < 0)
                {
                    var excludedDistricts = new[] {
                        DistrictCode.Ba_dinh.ToString(),
                        DistrictCode.Cau_giay.ToString(),
                        DistrictCode.Dong_da.ToString(),
                        DistrictCode.Hoan_kiem.ToString(),
                        DistrictCode.Thanh_xuan.ToString(),
                        DistrictCode.Tu_liem.ToString()
                    };
                    filterDefinition &= Builders<ProductMongoDbModel>.Filter.Where(x => x.group_product_id.Contains(DistrictCode.Ba_dinh.ToString()));
                    LogHelper.InsertLogTelegramByUrl(_configuration["BotSetting:bot_token"], _configuration["BotSetting:bot_group_id"], districtCode.ToString() + group_product_id);
                }
                if (amount_min > 0 && amout_max > 0)
                {
                    var priceFilter = Builders<ProductMongoDbModel>.Filter.And(
                     Builders<ProductMongoDbModel>.Filter.Gt(p => p.price, 0),              // Price greater than 0
                     Builders<ProductMongoDbModel>.Filter.Gte(p => p.price, amount_min),      // Price greater than or equal to minPrice
                     Builders<ProductMongoDbModel>.Filter.Lte(p => p.price, amout_max)       // Price less than or equal to maxPrice
                );

                    // Condition 2: Amount > 0 and Price is within the range
                    var amountFilter = Builders<ProductMongoDbModel>.Filter.And(
                        Builders<ProductMongoDbModel>.Filter.Gt(p => p.amount_min, 0),             // Amount greater than 0
                        Builders<ProductMongoDbModel>.Filter.Gte(p => p.amount_min, amount_min),      // Price greater than or equal to minPrice
                        Builders<ProductMongoDbModel>.Filter.Lte(p => p.amount_min, amout_max)       // Price less than or equal to maxPrice
                    );
                    filterDefinition &= Builders<ProductMongoDbModel>.Filter.Or(priceFilter, amountFilter);
                }
                var sort_filter = Builders<ProductMongoDbModel>.Sort;
                var sort_filter_definition = sort_filter.Descending(x => x.updated_last);
                var model = _productDetailCollection.Find(filterDefinition);
                long count = await model.CountDocumentsAsync();
                model.Options.Skip = page_index < 1 ? 0 : (page_index - 1) * page_size;
                model.Options.Limit = page_size;
                var result = await model.ToListAsync();
                return new ProductListResponseModel()
                {
                    items = result,
                    count = count,
                    page_index = page_index,
                    page_size = page_size
                };
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegramByUrl(_configuration["log_telegram:token"], _configuration["log_telegram:group_id"], error_msg);
                return null;
            }
        }

        public async Task<string> DeactiveByParentId(string id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.parent_product_id, id);
                var update = Builders<ProductMongoDbModel>.Update.Set(x => x.status, (int)ProductStatus.DEACTIVE);

                var updated_item = await _productDetailCollection.UpdateManyAsync(filterDefinition, update);
                return id;
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegramByUrl(_configuration["log_telegram:token"], _configuration["log_telegram:group_id"], error_msg);
            }
            return null;

        }

        public async Task<List<ProductMongoDbModel>> searchByProductName(string keyword)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;

                // Use regex for case-insensitive partial match
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Regex(x => x.name, new BsonRegularExpression(keyword, "i"));
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.parent_product_id, "");
                var model = _productDetailCollection.Find(filterDefinition);
                var result = await model.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegramByUrl(_configuration["log_telegram:token"], _configuration["log_telegram:group_id"], error_msg);
                return null;
            }
        }

        public async Task<ProductDetailResponseModel> GetFullProductById(string id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x._id, id);
                var detail = await _productDetailCollection.Find(filterDefinition).FirstOrDefaultAsync();
                var result = new ProductDetailResponseModel()
                {
                    product_main = detail,
                    product_sub = await SubListing(id)
                };
                return result;
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegramByUrl(_configuration["log_telegram:token"], _configuration["log_telegram:group_id"], error_msg);
                return null;
            }
        }
        public async Task<ProductListResponseModel> ResponseListing(string keyword = "", int group_id = -1, int page_index = 1, int page_size = 10)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                if (keyword != null && keyword.Trim() != "")
                {
                    string regex_keyword_pattern = keyword;
                    var keyword_split = keyword.Split(" ");
                    if (keyword_split.Length > 0)
                    {
                        regex_keyword_pattern = "";

                        foreach (var word in keyword_split)
                        {
                            string w = word.Trim();
                            if (StringHelper.HasSpecialCharacterExceptVietnameseCharacter(word))
                            {
                                w = StringHelper.RemoveSpecialCharacterExceptVietnameseCharacter(word);
                            }
                            regex_keyword_pattern += "(?=.*" + w + ".*)";

                        }
                    }
                    regex_keyword_pattern = "^" + regex_keyword_pattern + ".*$";
                    var regex = new BsonRegularExpression(regex_keyword_pattern.Trim().ToLower(), "i");

                    filterDefinition &= Builders<ProductMongoDbModel>.Filter.Or(
                       Builders<ProductMongoDbModel>.Filter.Regex(x => x.name, regex), // Case-insensitive regex
                       Builders<ProductMongoDbModel>.Filter.Regex(x => x.sku, regex), // Case-insensitive regex
                       Builders<ProductMongoDbModel>.Filter.Regex(x => x.code, regex)  // Case-insensitive regex
                    );
                }

                if (group_id > 0)
                {
                    filterDefinition &= Builders<ProductMongoDbModel>.Filter.Regex(x => x.group_product_id, group_id.ToString());
                }
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.parent_product_id, "");
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.status, (int)ProductStatus.ACTIVE);

                var sort_filter = Builders<ProductMongoDbModel>.Sort;
                var sort_filter_definition = sort_filter.Descending(x => x.updated_last);
                var model = _productDetailCollection.Find(filterDefinition);
                int count = model.ToList().Count;
                model.Options.Skip = page_index < 1 ? 0 : (page_index - 1) * page_size;
                model.Options.Limit = page_size;

                return new ProductListResponseModel()
                {
                    items = await model.ToListAsync(),
                    count = count
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ProductMongoDbModel>> SubListing(string parent_id)
        {
            try
            {
                var filter = Builders<ProductMongoDbModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.parent_product_id, parent_id);
                filterDefinition &= Builders<ProductMongoDbModel>.Filter.Eq(x => x.status, (int)ProductStatus.ACTIVE); ;

                var model = _productDetailCollection.Find(filterDefinition);
                var result = await model.ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegramByUrl(_configuration["log_telegram:token"], _configuration["log_telegram:group_id"], error_msg);
                return null;
            }
        }
    }
}
