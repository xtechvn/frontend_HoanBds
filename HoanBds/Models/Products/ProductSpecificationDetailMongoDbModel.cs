namespace HoanBds.Models.Products
{
    public class ProductSpecificationDetailMongoDbModel
    {
       
        public string _id { get; set; }
  
        public int attribute_id { get; set; }
        public int value_type { get; set; }
        public string value { get; set; }

    }
}
