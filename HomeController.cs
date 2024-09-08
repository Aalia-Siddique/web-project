using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
//using System.Web.Mvc;

namespace Web_Project__MVC_.Controllers
{
    //[Authorize(Policy = "LoggedIn")]
    public class HomeController : Controller
    {
        private readonly IRepository<Catagory> _categoryRepository;
        public HomeController(

            IRepository<Catagory> categoryRepository)

        {

            _categoryRepository = categoryRepository;
            // _prodRepository = prodRepository;
        }

        public IActionResult location()

        {
            return View();
        }
       // [Authorize(Policy = "LoggedIn")]

        public ViewResult Index()
        {
           
           
            //string conn = $"Data Source=(localdb)\\MSSQLLocalDB" +
            //    $";Initial Catalog=MyDB;Integrated Security=True";
            //IRepository<Catagory> rep = new GenericRepository<Catagory>
            //    (conn);
            List<Catagory> products = _categoryRepository.GetAll().ToList();
            ViewBag.MyList = products;

            return View("Index");
        }
        public ViewResult About()
        {
            //string conn = $"Data Source=(localdb)\\MSSQLLocalDB" +
            //   $";Initial Catalog=MyDB;Integrated Security=True";
            //IRepository<Catagory> rep = new GenericRepository<Catagory>
            //    (conn);
            List<Catagory> products = _categoryRepository.GetAll().ToList();
            ViewBag.MyList = products;

            return View();
        }

    }
}