using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
