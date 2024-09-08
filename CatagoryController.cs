using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web_Project__MVC_.Controllers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CatagoryController : Controller
    {
        private readonly IRepository<Catagory> _categoryRepository;
        public CatagoryController(
            
            IRepository<Catagory> categoryRepository)
           
        {
            
            _categoryRepository = categoryRepository;
           // _prodRepository = prodRepository;
        }

        public IActionResult AdminViewCatagory()
        {
            
            List<Catagory> c = _categoryRepository.GetAll().ToList();
                return View(c);
        }
        public IActionResult UserViewCatagory()
        {
           
            List<Catagory> c = _categoryRepository.GetAll().ToList();
            return View(c);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        //public IActionResult Add(string catagoryName)
        //{

        //    Catagory c = new Catagory
        //    {
        //        catagoryName = catagoryName
        //    };
        //    _categoryRepository.Add(c);
        //    return RedirectToAction("AdminViewCatagory", "Catagory");
        //}
        public IActionResult Add(Catagory catagory)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Add(catagory);
                // Update logic here
                return Json(new { success = true, message = "Category  Added successfully!" });
            }

            // Collect validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
        }
        public IActionResult Update()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Update(Catagory model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _categoryRepository.Update(model);
        //        return RedirectToAction("AdminViewCatagory", "Catagory");
        //    }

        //    // Handle the case where the model is not valid
        //    return View(model);
        //}

        [HttpPost]
        public IActionResult Update(Catagory catagory)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(catagory);
                // Update logic here
                return Json(new { success = true, message = "Category updated successfully!" });
            }

            // Collect validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
        }
        //  [HttpPost]
        public IActionResult Delete(string id)
        {
            string conn = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
           // IRepository<Catagory> rep = new GenericRepository<Catagory>(conn);
            _categoryRepository.Delete(id);
            return RedirectToAction("AdminViewCatagory", "Catagory");
 
        }
    }
}
