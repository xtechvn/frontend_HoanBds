using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HoanBds.Models
{
    public class ClientInfomationModel
    {
        [BsonElement("id")]
        public string id { get; set; }
        public void GenID()
        {
            id = ObjectId.GenerateNewId().ToString();
        }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public DateTime createdTime { get; set; }
        public DateTime updatedTime { get; set; }
        public string content { get; set; }
    }
}
