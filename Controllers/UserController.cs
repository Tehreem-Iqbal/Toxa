using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ProjectManagementApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment Environment;
        public UserController(IWebHostEnvironment environment)
        {
            Environment = environment;
        }    

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AddCookie(user);
                    user.UserType = true;
                    string userdir = "Uploads/Users/" + user.UserName;
                    user.ImageURL = userdir + "/" + user.Image!.FileName;


                    int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
                    UserRepository repo = new(HttpContext,userId);
                    ViewBag.msg = repo.AddUser(user);
                    return View("Login");              
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex + 
                "ERROR: Unable to create account. Try again, and if the problem persists contact system administrator.");
            }
            return View(user); 
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user) //Aauthenticate
        {
            try
            {
                object errormsg;

                // int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);

                UserRepository repo = new(HttpContext,user.Id);
                IEnumerable<User> UsersList = repo.RetrieveUsers();
               
                foreach (User _user in UsersList)
                {
                    if (_user.UserEmail!.Equals(user.UserEmail))
                    {
                        if (_user.Password!.Equals(user.Password))
                        {
                            AddCookie(_user);
                            TempData["user"] =JsonSerializer.Serialize(_user);
                            return (_user.UserType) ?
                                RedirectToAction("Index", "Admin") :
                                RedirectToAction("Index", "Dashboard");
                            
                        }
                    }
                }
                errormsg = "cred-error";
                ViewBag.cred_msg = errormsg;
                return View("Login");
            }
            catch (Exception ex) {
                ModelState.AddModelError("", ex +
                "Unable to login. Try again, and if the problem persists contact system administrator.");
            }
            return View("Login");
        }

        [NonAction]
        public void AddCookie(User user)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(30);
            
            // Add cookies in the formate: "userid,userType"
            Response.Cookies.Append("user", user.Id.ToString()+","+user.UserType.ToString());
        }

      
    }
}
