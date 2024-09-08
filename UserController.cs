using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNetCore.Identity;

//using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace Web_Project__MVC_.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<MyUser> _userManager;
        private readonly SignInManager<MyUser> _signInManager;
        public UserController(UserManager<MyUser> 
            userManager, SignInManager<MyUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var firstName = user.FirstName;
                var lastName = user.LastName;
                var email = user.Email;

                ViewBag.FirstName = firstName;
                ViewBag.LastName = lastName;
                ViewBag.Email = email;
            }

            return View();
        }

        public async Task<IActionResult> AllUsers()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }
        // GET: User
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }

    }
}