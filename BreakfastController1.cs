/*using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Web_Project__MVC_.Controllers;
using WebApplication1.Models;
namespace WebApplication1.Controllers
{
    public class BreakfastController1 : Controller
    {
        
        public IActionResult Index()
        {
            ProductRepository productsRepository = new ProductRepository();
            List<Product> products = productsRepository.GetBreakfast();
            return View(products);
        }
        public IActionResult Index1()
        {
           
            return View("breakfast");
        }

    }
}*/
