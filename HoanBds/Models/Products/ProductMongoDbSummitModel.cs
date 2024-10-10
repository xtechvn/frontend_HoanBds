namespace HoanBds.Models.Products
{
    public class ProductMongoDbSummitModel: ProductMongoDbModel
    {
        public List<ProductDetailVariationMongoDbModel> variations { get; set; }


    }
}
