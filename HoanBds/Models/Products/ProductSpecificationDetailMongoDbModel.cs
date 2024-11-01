using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HoanBds.Models.Products
{
    public class ProductSpecificationDetailMongoDbModel
    {
        [BsonElement("_id")]
        public string _id { get; set; }
        public void GenID()
        {
            _id = ObjectId.GenerateNewId().ToString();
        }
        public int attribute_id { get; set; }
        public int value_type { get; set; }
        public string value { get; set; }
        public string type_ids { get; set; }

    }
}
