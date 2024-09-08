using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace Web_Project__MVC_.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly ILogger<FeedbackController> _logger;
        private readonly IRepository<Catagory> _catagoryRepository;
        private readonly IRepository<Feedback> _feedbackRepository;
        private readonly IFeedbackRepository _feedbackRepo;

        public FeedbackController(
            ILogger<FeedbackController> logger,
            IRepository<Catagory> catagoryRepository,
            IRepository<Feedback> feedbackRepository,
            IFeedbackRepository feedbackRepo)
        {
            _logger = logger;
            _catagoryRepository = catagoryRepository;
            _feedbackRepository = feedbackRepository;
            _feedbackRepo = feedbackRepo;
        }
        // GET: Feedback
        public ActionResult Index()
        {
            List<Catagory> products = _catagoryRepository.GetAll().ToList();
            ViewBag.MyList = products;
            return View();
        }

        public ActionResult AdminViewFeedback()
        {
            List<Catagory> products = _catagoryRepository.GetAll().ToList();
            ViewBag.MyList = products;

            List<Feedback> feedback = _feedbackRepository.GetAll().ToList();
            return View(feedback);
        }

        public IActionResult UpdateFeedback()
        {
            List<Catagory> products = _catagoryRepository.GetAll().ToList();
            ViewBag.MyList = products;
            return View();
        }

        public IActionResult DeleteFeedback()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            _feedbackRepository.Delete(id);
            return RedirectToAction("ViewFeedback");
        }

        [HttpPost]
        public IActionResult Update(int id, string description)
        {
            _feedbackRepo.Update(id, description);
            return RedirectToAction("ViewFeedback");
        }
        public ActionResult ViewFeedback()
        {
            List<Catagory> products = _catagoryRepository.GetAll().ToList();
            ViewBag.MyList = products;

            List<Feedback> feedback = _feedbackRepository.GetAll().ToList();
            return View(feedback);
        }

        [HttpPost]
        public IActionResult Add([FromForm] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _feedbackRepository.Add(feedback);
                return Json(new { success = true });
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors)
                                           .Select(e => e.ErrorMessage)
                                           .ToList();
            return Json(new { success = false, message = string.Join("; ", errors) });
        }
    }
}
