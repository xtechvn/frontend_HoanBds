using Nest;

namespace HoanBds.Models
{
    public class ArticleModel
    {
        [PropertyName("id")]
        public long id { get; set; }
        public string category_name { get; set; }
        [PropertyName("Title")]
        public string title { get; set; }
        [PropertyName("Lead")]
        public string lead { get; set; }
        [PropertyName("Image169")]
        public string image_169 { get; set; }
        [PropertyName("Image43")]
        public string image_43 { get; set; }
        [PropertyName("Image11")]
        public string image_11 { get; set; }
        [PropertyName("Body")]
        public string body { get; set; }
        [PropertyName("PublishDate")]
        public DateTime publishdate { get; set; }
        public DateTime update_last { get; set; }
        public DateTime createdon { get; set; }
        public DateTime modifiedon { get; set; }
        public int article_type { get; set; }
        public short? position { get; set; }
        public int status { get; set; }
        [PropertyName("ListCategoryId")]
        public string category_id { get; set; }



    }
}
