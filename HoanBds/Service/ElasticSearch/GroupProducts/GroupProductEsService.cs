using Elasticsearch.Net;
using HoanBds.Contants;
using HoanBds.Models;
using HoanBds.Models.GroupProduct;
using HoanBds.Utilities;
using Nest;
using System.Collections.Generic;
using System.Reflection;

namespace HoanBds.Service.ElasticSearch.GroupProducts
{
    public class GroupProductEsService
    {
        public string index = "";
        private readonly IConfiguration configuration;
        private static string _ElasticHost;
        private static ElasticClient _elasticClient;

        public GroupProductEsService(string Host, IConfiguration _configuration)
        {
            _ElasticHost = Host;
            configuration = _configuration;
            index = _configuration["Elastic:Index:GroupProduct"];

        }
        /// <summary>
        /// Lấy ra các chuyên mục con theo chuyên mục cha
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<CategoryModel> GetListGroupProductByParentId(long parent_id, int size = 10)
        {
            try
            {
                if (_elasticClient == null)
                {
                    var nodes = new Uri[] { new Uri(_ElasticHost) };
                    var connectionPool = new SniffingConnectionPool(nodes); // Sử dụng Sniffing để khám phá nút khác trong cụm
                    var connectionSettings = new ConnectionSettings(connectionPool)
                        .RequestTimeout(TimeSpan.FromMinutes(2))  // Tăng thời gian chờ nếu cần
                        .SniffOnStartup(true)                     // Khám phá các nút khi khởi động
                        .SniffOnConnectionFault(true)             // Khám phá lại các nút khi có lỗi kết nối
                        .EnableHttpCompression();                 // Bật nén HTTP để truyền tải nhanh hơn

                    _elasticClient = new ElasticClient(connectionSettings);

                }

                // Thực hiện truy vấn với ElasticClient
                var query = _elasticClient.Search<CategoryModel> (sd => sd
                    .Index(index)
                    .Size(size)
                    .Query(q => q
                        .Bool(b => b
                            .Must(
                                m => m.Term(t => t.Field("status").Value(ArticleStatus.PUBLISH.ToString())),
                                m => m.Term(t => t.Field("parentid").Value(parent_id))
                            )
                        )
                    )
                );

                // Kiểm tra nếu query hợp lệ
                if (query.IsValid)
                {
                    var data = query.Documents.ToList();
                    return data;
                }
                else
                {
                    // Ghi log nếu truy vấn không thành công
                    LogHelper.InsertLogTelegramByUrl(configuration["log_telegram:token"], configuration["log_telegram:group_id"], "Query Invalid: " + query.DebugInformation);
                }
            }
            catch (Exception ex)
            {
                string error_msg = Assembly.GetExecutingAssembly().GetName().Name + "->" + MethodBase.GetCurrentMethod().Name + "=>" + ex.Message;
                LogHelper.InsertLogTelegramByUrl(configuration["log_telegram:token"], configuration["log_telegram:group_id"], error_msg);
            }

            return null;
        }
    }
}
