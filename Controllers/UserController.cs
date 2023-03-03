using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Models.Interfaces;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace ProjectManagementApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly IInvoiceRepository invoiceRepository;

        private readonly IWebHostEnvironment Environment;
        public UserController(IWebHostEnvironment environment, IUserRepository _userRepository, IProjectRepository _projectRepository,
                        IServiceRepository _serviceRepository, IInvoiceRepository _invoiceRepository)
        {
            Environment = environment;
            userRepository = _userRepository;
            projectRepository = _projectRepository;
            invoiceRepository = _invoiceRepository;
            serviceRepository = _serviceRepository;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp([FromBody] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AddCookie(user);
                    user.UserType = false;
                    string userdir = "Uploads/Users/" + user.UserName;
                    user.ImageURL = userdir + "/" + user.Image!.FileName;

                    int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
                    string msg = userRepository.AddUser(user);
                    ViewBag.msg = msg;
                    if(msg == "success")
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest();              
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex + 
                "ERROR: Unable to create account. Try again, and if the problem persists contact system administrator.");
            }
            return BadRequest(); 
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

                List<User> UsersList = userRepository.GetAllUsers();
               
                foreach (User _user in UsersList)
                {
                    if (_user.UserEmail!.Equals(user.UserEmail))
                    {
                        if (_user.Password!.Equals(user.Password))
                        {
                            AddCookie(_user);
                            TempData["user"] = JsonSerializer.Serialize(_user);
                                Console.WriteLine("I am going to user Dashbaord.");
                            if (_user.UserType)
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Dashboard");

                            }
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
            
            Response.Cookies.Append("user", user.Id.ToString()+","+user.UserType.ToString());
        }

      
    }
}
