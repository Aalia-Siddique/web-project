using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FeauturesController : Controller
    {
        private readonly IRepository<Catagory> _categoryRepository;
        public FeauturesController(

            IRepository<Catagory> categoryRepository)

        {

            _categoryRepository = categoryRepository;
            // _prodRepository = prodRepository;
        }
        public IActionResult Index()
        {
            string conn = $"Data Source=(localdb)\\MSSQLLocalDB" +
                $";Initial Catalog=MyDB;Integrated Security=True";
           // IRepository<Catagory> rep = new GenericRepository<Catagory>
               // (conn);
            List<Catagory> products = _categoryRepository.GetAll().ToList();
            ViewBag.MyList = products;
            return View();
        }
    }
}
