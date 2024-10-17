namespace HoanBds.Models.Products
{
    public class ProductListResponseModel
    {
        public List<ProductMongoDbModel> items { get; set; }
        public long count { get; set; }
        public int page_index { get; set; }
        public int page_size { get; set; }
    }
}
