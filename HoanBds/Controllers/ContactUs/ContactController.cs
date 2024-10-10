using Microsoft.AspNetCore.Mvc;

namespace HoanBds.Controllers.ContactUs
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
