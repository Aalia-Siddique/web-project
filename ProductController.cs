using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web_Project__MVC_.Controllers;
using WebApplication1.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<MyUser> _userManager;
        private readonly ProductRepository _productRepository;
        private readonly IRepository<Catagory> _categoryRepository;
        private readonly IRepository<Product> _prodRepository;
        private readonly IRepository<Product1> _prod1Repository;
        //string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyDB;Integrated Security=True";
        //IRepository<Product> genericProductRepository = new GenericRepository<Product>(connectionString);
        //ProductRepository productRepository = new ProductRepository(genericProductRepository);
        public ProductController(
            ILogger<HomeController> logger,
            UserManager<MyUser> userManager,
            IWebHostEnvironment env,
         ProductRepository productRepository,
            IRepository<Catagory> categoryRepository,
            IRepository<Product> prodRepository, IRepository<Product1> 
            prod1Repository)
        {
            _logger = logger;
            _env = env;
            _userManager = userManager;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _prodRepository = prodRepository;
            _prod1Repository = prod1Repository;
        }

        public async Task<IActionResult> Index1()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Email = user.Email;
            }

            ViewBag.TotalUsersCount = await _userManager.Users.CountAsync();
            ViewBag.TotalProductsCount = _productRepository.GetTotalProductsCount();
            ViewBag.TotalOrdersCount = _productRepository.GetTotalOrdersCount();

            return View();
        }

        [Authorize(Policy = "Category1")]
        public async Task<IActionResult> Admin()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                ViewBag.FirstName = user.FirstName;
                ViewBag.LastName = user.LastName;
                ViewBag.Email = user.Email;
            }

            ViewBag.TotalUsersCount = await _userManager.Users.CountAsync();
            ViewBag.TotalProductsCount = _productRepository.GetTotalProductsCount();
            ViewBag.TotalOrdersCount = _productRepository.GetTotalOrdersCount();

            //var categories = _categoryRepository.GetAll().ToList();
            //ViewBag.MyList = categories;

            return View();
        }

        [Authorize(Policy = "Category1")]
        public IActionResult AddProd()
        {
            return View();
        }

        [Authorize(Policy = "Category1")]
        public IActionResult AdminView()
        {
            var categories = _categoryRepository.GetAll().ToList();
            ViewBag.MyList = categories;

            var products = _prodRepository.GetAll().ToList();
            return View(products);
        }
       
    [Authorize]
        public async Task<IActionResult> Index2()
        {
            var categories = _categoryRepository.GetAll().ToList();
            ViewBag.MyList = categories;

            var products = _prodRepository.GetAll().ToList();
            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> Index3(string id)
        {
            var categories = _categoryRepository.GetAll().ToList();
            ViewBag.MyList = categories;

            var product = _prodRepository.FindByCategoryId(id).ToList();

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Authorize(Policy = "Category1")]
        public IActionResult UpdateProd()
        {
            return View();
        }

    //    [HttpPost]
    //    public IActionResult Update(int ProductId, string Name, string Price,
    //string Description, string Quantity, List<IFormFile> ImagePath)
    //    {
    //        if (!ModelState.IsValid)
    //        {
    //            // Log validation errors
    //            var errors = ModelState.Values.SelectMany(v => v.Errors)
    //                                          .Select(e => e.ErrorMessage)
    //                                          .ToList();
    //            return BadRequest(new { success = false, errors });
    //        }

    //         Product p = new Product
    //            {
    //                ProductId = ProductId,
    //                Name = Name,
    //                Price = Price,
    //                Description = Description,
    //                Quantity = Quantity
    //            };

    //            string wwwrootPath = _env.WebRootPath;
    //            string path = Path.Combine(wwwrootPath, "upload");
    //            if (!Directory.Exists(path))
    //            {
    //                Directory.CreateDirectory(path);
    //            }

    //            foreach (var file in ImagePath)
    //            {
    //                if (file.Length > 0)
    //                {
    //                    string fileName = file.FileName;
    //                    fileName = Path.GetFileName(fileName);
    //                    string filePath = Path.Combine(path, fileName);
    //                    p.ImagePath = Path.Combine("upload", fileName);
    //                    using (var fileStream = new FileStream(filePath, FileMode.Create))
    //                    {
    //                        file.CopyTo(fileStream);
    //                    }
    //                }
    //            }

    //            _prodRepository.Update(p);
    //            return Json(new { success = true, message = "Product updated successfully!" });
           
    //    }

        [HttpPost]
        public IActionResult Update(int ProductId, string Name, string Price, string Description, string Quantity, List<IFormFile> ImagePath)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { success = false, errors });
            }

            if (ProductId <= 0)
            {
                // Log and return an error for invalid ProductId
                _logger.LogError("Invalid ProductId: {ProductId}", ProductId);
                return BadRequest(new { success = false, message = "Invalid ProductId provided." });
            }

            try
            {
                Product p = new Product
                {
                    ProductId = ProductId,
                    Name = Name,
                    Price = Price,
                    Description = Description,
                    Quantity = Quantity
                };
                string wwwrootPath = _env.WebRootPath;
                string path = Path.Combine(wwwrootPath, "upload");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                foreach (var file in ImagePath)
                {
                    if (file.Length > 0)
                    {
                        string fileName = file.FileName;
                        fileName = Path.GetFileName(fileName);
                        string filePath = Path.Combine(path, fileName);
                        p.ImagePath = Path.Combine("upload", fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                    }
                }
                // File handling and update logic
                // ...

                _productRepository.Update(p);
                return Json(new { success = true, message = "Product updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating product: {Message}. StackTrace: {StackTrace}", ex.Message, ex.StackTrace);
                return StatusCode(500, new { success = false, message = "An error occurred while updating the product." });
            }
        }


        // Implement your update logic here


        [HttpPost]
        public IActionResult Delete(string id)
        {
            _productRepository.DeleteById(id);
            return RedirectToAction("AdminView", "Product");
        }

        [HttpPost]
        public IActionResult Add(string Name, string Price, string
     Description, string Quantity, string category, List<IFormFile> userfiles)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors });
            }
            Product p = new Product
            {
                Name = Name,
                Price = Price,
                Description = Description,
                Quantity = Quantity,
                category = category,
            };


            string wwwrootPath = _env.WebRootPath;
            string path = Path.Combine(wwwrootPath, "upload");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var file in userfiles)
            {
                if (file.Length > 0)
                {
                    string fileName = file.FileName;
                    fileName = Path.GetFileName(fileName);
                    string filePath = Path.Combine(path, fileName);
                    p.ImagePath = Path.Combine("upload", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                }
            }
            // Console.WriteLine(p.Description);
            _productRepository.Add(p);
            return Json(new { success = true, message = "Product added successfully!" });
            //  return RedirectToAction("adminview", "Product");

        }
        //[HttpPost]
        //public IActionResult GetById(string id)
        //{
        //    var product = _prodRepository.FindById(id);

        //    if (product != null)
        //    {
        //        return View(product);
        //    }

        //    return NotFound();
        //}

        //[HttpPost]
        //public IActionResult GetByName(string id)
        //{
        //    var product = _prodRepository.FindByName(id);

        //    if (product != null)
        //    {
        //        return View(product);
        //    }

        //    return NotFound();
        //}

        [HttpGet]
        public JsonResult Search(string searchQuery)
        {
            var products = new List<Product>();

            var product = _prodRepository.FindByName(searchQuery);
            if (product != null)
            {
                products.Add(product);
            }

            return Json(products);
        }

        //public IActionResult Search(string query, string searchType)
        //{
        //    Product product = null;

        //    if (searchType == "id")
        //    {
        //        product = _prodRepository.FindById(query);
        //    }
        //    else if (searchType == "name")
        //    {
        //        product = _prodRepository.FindByName(query);
        //    }

        //    if (product != null)
        //    {
        //        return PartialView("_partialLayout", product);
        //    }
        //    return NotFound();
        //}

        [HttpPost]
        public IActionResult GetById(string id)
        {
            var product = _prodRepository.FindById(id);

            if (product != null)
            {
                return PartialView("_partialLayout", product);
            }

            return NotFound();
        }


        [HttpPost]
        public IActionResult GetByName(string name)
        {
            var product = _prodRepository.FindByName(name);

            if (product != null)
            {
                return PartialView("_partialLayout", product);
            }

            return NotFound();
        }

        public IActionResult TopSearchProducts()
        {

            var topProducts = _prod1Repository.GetTopSearchedProducts();
            return View(topProducts);
        }
        //public IActionResult Index()
        //{
        //    ProductRepository product = new ProductRepository();
        //    _productRepository.CopyProductData();
        //    return RedirectToAction("Index2", "Product");

        //}
    }
}
         
