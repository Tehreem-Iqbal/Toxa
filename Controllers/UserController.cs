using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;

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
            object errormsg, successmsg;
 
            try
            {
                if (ModelState.IsValid)
                {
                    if (DuplicateCheck(user.UserEmail)) {  errormsg = "User already exists"; ViewBag.msg = errormsg;  return View(user); }
                    else
                    {
                        user.UserType = false;
                        
                        UserRepository.AddUser(user);
                        if(FileUpload(user.Image, user.UserName))
                        {
                            successmsg = "Account created successfully";
                            ViewBag.success = successmsg;
                            return View("Login");
                        }
                                         
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex + "ERROR: Unable to create account. Try again, and if the problem persists contact system administrator.");
            }
            return View(user); 
        }

        [NonAction]
        public bool DuplicateCheck(string? email)
        {
            
            IEnumerable<User> CustomersList = UserRepository.RetrieveUsers();
            foreach (User u in CustomersList)
            {
                if (email.Equals(u.UserEmail))
                {
                    return true;  
                }
            }
            return false;
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
                IEnumerable<User> CustomersList = UserRepository.RetrieveUsers();
               
                foreach (User u in CustomersList)
                {
                    if (u.UserEmail.Equals(user.UserEmail))
                    {
                        if (u.Password.Equals(user.Password))
                        {
                            addCookie(u);
                            return (u.UserType) ?  RedirectToAction("Index", "Admin",u) : RedirectToAction("Index", "Dashboard", u);
                            
                        }
                    }
                }
                errormsg = "Invalid email or password";
                ViewBag.msg = errormsg;
                return View("Login");
            }
            catch (Exception ex) { ModelState.AddModelError("", ex + "Unable to login. Try again, and if the problem persists contact system administrator."); }
            return View("Login");
        }

        [NonAction]
        public void addCookie(User user)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(30);

            Response.Cookies.Append("userid", user.UserId.ToString());
            Response.Cookies.Append("usertype", user.UserType.ToString());
        }

        [NonAction]
        public bool FileUpload(IFormFile? file, string? username)
        {
            Console.WriteLine("upload1");
            string path = "";
            try
            {        
                if (file.Length > 0)
                {
                    string wwwPath = this.Environment.WebRootPath;
                    string userdir = "Users/" + username;
                    path = Path.GetFullPath(Path.Combine(wwwPath , userdir));

                    Console.WriteLine(path);
                    if (!Directory.Exists(path))
                    { 
                        Directory.CreateDirectory(path);
                        Console.WriteLine("dir created");
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    Console.WriteLine("upload2");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed saving file", ex);
            }
        }

      
    }
}
