using HoanBds.Models;
using HoanBds.Utilities;
using MongoDB.Driver;

namespace HoanBds.Service.MongoDb
{
    public class ClientInfomationMongoService
    {
        private IMongoCollection<ClientInfomationModel> ClientInfoCollection;
        private IConfiguration Configuration;
        public ClientInfomationMongoService(IConfiguration _Configuration)
        {
            Configuration = _Configuration;
            string url = "mongodb://" + _Configuration["MongoServer:user"] + ":" + _Configuration["MongoServer:pwd"] + "@" + _Configuration["MongoServer:Host"] + ":" + _Configuration["MongoServer:Port"] + "/" + _Configuration["MongoServer:catalog_core"];
            var client = new MongoClient(url);
            IMongoDatabase db = client.GetDatabase(_Configuration["MongoServer:catalog_core"]);
            ClientInfoCollection = db.GetCollection<ClientInfomationModel>("ClientInfomation");

        }

        public async Task<string> AddNewOrReplace(ClientInfomationModel model)
        {
            try
            {
                var filter = Builders<ClientInfomationModel>.Filter;
                var filterDefinition = filter.Empty;
                filterDefinition &= Builders<ClientInfomationModel>.Filter.Eq(x => x.phone, model.phone);
                var exists_model = await ClientInfoCollection.Find(filterDefinition).FirstOrDefaultAsync();
                if (exists_model != null)
                {
                    exists_model.name = model.name;
                    exists_model.email = model.email;
                    exists_model.phone = model.phone;
                    exists_model.updatedTime = DateTime.UtcNow;
                    exists_model.content = model.content;
                    await ClientInfoCollection.FindOneAndReplaceAsync(filterDefinition, exists_model);
                    return exists_model.id;
                }
                else
                {
                    model.GenID();
                    model.createdTime = DateTime.UtcNow;
                    await ClientInfoCollection.InsertOneAsync(model);
                    return model.id;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegramByUrl(Configuration["log_telegram:token"], Configuration["log_telegram:group_id"], "AddNewOrReplace - NewsMongoService: " + ex);
                return null;
            }
        }
    }
}
