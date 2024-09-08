using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class SweetsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
