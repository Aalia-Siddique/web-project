/*using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class drinksController : Controller
    {
        
        public IActionResult Index()
        {
            ProductRepository productsRepository = new ProductRepository();
            List<Product> products = productsRepository.GetAll();
            return View(products);
        }
        public IActionResult Index1()
        {
            
            return View("drinks");
        }

    }
}
*/