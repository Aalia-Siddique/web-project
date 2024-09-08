using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using WebApplication1.Models;
//using System.Web.mvc;
using Microsoft.AspNetCore.Mvc;
//namespace WebApplication1.Controllers
//{
//    public class ContactController : Controller
//    {
//        // GET: Contact
//        public  ActionResult Index()
//        {
//            return View();
//        }
//        public ActionResult Submit()
//        {
//            return View();
//        }

//        [HttpPost]
//        public ActionResult SendContactMessage(ContactFormModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var fromAddress = new MailAddress("bsef21m034@pucit.edu.pk",
//                    "Aalia");
//                var toAddress = new MailAddress("bsef21m034@pucit.edu.pk", 
//                    "Ayesha");
//                const string fromPassword = "your-email-password";
//                string subject = model.Subject;
//                string body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}";

//                var smtp = new SmtpClient
//                {
//                    Host = "smtp.example.com",
//                    Port = 587,
//                    EnableSsl = true,
//                    DeliveryMethod = SmtpDeliveryMethod.Network,
//                    UseDefaultCredentials = false,
//                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
//                };
//                using (var message = new MailMessage(fromAddress, toAddress)
//                {
//                    Subject = subject,
//                    Body = body
//                })
//                {
//                    smtp.Send(message);
//                }

//                ViewBag.Message = "Your message has been sent successfully!";
//                return View("ContactUs", model);
//            }

//            return View("ContactUs", model);
//        }

//        public ActionResult ContactUs()
//        {
//            return View(new ContactFormModel());
//        }
//    }

//}



using System;
using System.Net;
using System.Net.Mail;
//using System.Web.Mvc;
//using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ContactController : Controller
    {
        private readonly IRepository<Catagory> _categoryRepository;
        public ContactController(

            IRepository<Catagory> categoryRepository)

        {

            _categoryRepository = categoryRepository;
            // _prodRepository = prodRepository;
        }

        // GET: Contact
        public ActionResult Index()
        {
            //string conn = $"Data Source=(localdb)\\MSSQLLocalDB" +
            //    $";Initial Catalog=MyDB;Integrated Security=True";
            //IRepository<Catagory> rep = new GenericRepository<Catagory>
            //    (conn);
            List<Catagory> products = _categoryRepository.GetAll().ToList();
            ViewBag.MyList = products;
            return View();
        }

        [HttpPost]
        public ActionResult SendContactMessage(ContactFormModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var fromAddress = new MailAddress("bsef21m034@pucit." +
                        "edu.pk",
                        "Aalia");
                    var toAddress = new MailAddress("bsef21m022@pucit.edu.pk",
                        "Rubaisha Zaidi");
                    const string fromPassword = "021@pucit";
                    string subject = model.Subject;
                    string body = $"Name: {model.Name}\nEmail: {model.Email}\nMessage: {model.Message}";

                    var smtp = new SmtpClient
                    {
                        Host = "smtp.example.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                    };

                    using (var message = new MailMessage(fromAddress, toAddress)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(message);
                    }

                    ViewBag.Message = "Your message has been sent successfully!";
                    return View("ContactUs", model);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"An error occurred while " +
                        $"sending the email: {ex.Message}";
                    return View("ContactUs", model);
                }
            }
            else
            {
                return View("ContactUs", model);
            }
        }

        public ActionResult ContactUs()
        {
            return View(new ContactFormModel());
        }
    }
}
