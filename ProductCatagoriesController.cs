using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Web_Project__MVC_.Controllers
{
    public class ProductCatagoriesController : Controller
    {
        // GET: ProductCataogoriesjk
        
        public ActionResult ViewAll()
        {
            return View();
        }
        public ActionResult Breakfast()
        {
            return View();
        }
        public ActionResult Lunch()
        {
            return View();
        }
        public ActionResult SweetDishes()
        {
            return View();
        }
        public ActionResult Drinks()
        {
            return View();
        }


    }
}