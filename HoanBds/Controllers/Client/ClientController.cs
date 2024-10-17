using HoanBds.Controllers.Home;
using HoanBds.Models;
using HoanBds.Service.MongoDb;
using Microsoft.AspNetCore.Mvc;

namespace HoanBds.Controllers.Client
{
    public class ClientController : Controller
    {
        private readonly ILogger<ClientController> _logger;
        private readonly IConfiguration configuration;
        private readonly ClientInfomationMongoService _mongoService;
        public ClientController(IConfiguration configuration,ILogger<ClientController> logger)
        {
            this.configuration = configuration;
            _logger = logger;
            _mongoService = new ClientInfomationMongoService(configuration);
        }

        [HttpPost]
        public async Task<IActionResult> SendClientInfor(ClientInfomationModel model)
        {
            try
            {
                await _mongoService.AddNewOrReplace(model);
                return Ok();
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                _logger.LogError(ex, "Error loading GroupProduct");
                return StatusCode(500); // Trả về lỗi 500 nếu có lỗi
            }
        }
    }
}
