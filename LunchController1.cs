/*using Microsoft.AspNetCore.Mvc;
using Web_Project__MVC_.Controllers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LunchController1 : Controller
    {
        
            private readonly ILogger<HomeController> _logger;
            private readonly IWebHostEnvironment _env;

            public LunchController1(ILogger<HomeController> logger, IWebHostEnvironment env)
            {
                _logger = logger;
                _env = env;
            }



            public IActionResult Index()
            {
                ProductRepository productsRepository = new ProductRepository();
                List<Product> products = productsRepository.GetAll();
                return View(products);
            }

        public IActionResult Index1()
        {

            return View("lunnch");
        }


        public IActionResult Add(string Name, string Description)
         {

             Product p = new Product { Name = Name, Description = Description };
             ProductRepository productRepository = new ProductRepository();
             productRepository.Add(p);
             return RedirectToAction("Index", "Product");
         }
    }
    }*/

