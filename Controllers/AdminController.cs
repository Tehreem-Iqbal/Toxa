using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Utilities;
using System.Data;
using System.Globalization;
using System.Linq;
namespace ProjectManagementApplication.Controllers
{
    public class AdminController : Controller
    { 
        public IActionResult Logout()
        {
            if (Request.Cookies["user"] != null)
            {
                HttpContext.Response.Cookies.Delete("user");
               
            }
            Console.WriteLine("Loging out");
            string msg = "You are Logged Out.";
            TempData["msg"] = msg;
            return RedirectToAction("Login", "User", msg);
        }
        public IActionResult Index() {
            object errormsg;
            string userType = "True";
            if (!HttpUtilities.ValidateState(HttpContext,userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }

            return View();
        }
        [HttpGet]
        public IActionResult AddProject() { return View(); }
        [HttpPost]
        public IActionResult AddProject(Project project)
        {
            object errormsg;
            string userType = "True";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
                    ProjectRepository repo = new(HttpContext,userId);
                    repo.AddProject(project);
                    TempData["success"] = "Project added successfully";
                    return RedirectToAction("Index", "Admin");
                }
            }
            catch (DataException)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(project);
        }

        [HttpGet]
        public IActionResult DeleteProject() { return View("DeleteProject"); }
        [HttpPost]
        public IActionResult DeleteProject(int id)
        {
            object errormsg;
            string userType = "True";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }
            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
            ProjectRepository repo = new(HttpContext,userId);
            var obj = repo.RetrieveProject(id);
            if (obj == null)
            {
                return NotFound();
            }

            repo.RemoveProject(obj);
            TempData["success"] = "Project deleted successfully";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DeleteUser() { return View("DeleteUser"); }
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            object errormsg;
            string userType = "True";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }
            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
            UserRepository repo = new(HttpContext,userId);
            var obj = repo.RetrieveUser(id);
            if (obj == null)
            {
                return NotFound();
            }

            repo.RemoveUser(obj);
            TempData["success"] = "Project deleted successfully";
            return RedirectToAction("Index");
        }
        public IActionResult UserDetails(int userid)
        {
            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
            UserRepository repo = new(HttpContext,userId);
            User user = repo.RetrieveUser(userid)!;
            return View(user);
        }
        public IActionResult DisplayAllUsers()
        {
            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
            UserRepository repo = new(HttpContext,userId);
            List<User> users = repo.RetrieveUsers().ToList<User>();
            return View(users);
            
        }
        [HttpGet]
        public IActionResult AddService() { return View(); }
        [HttpPost]
        public IActionResult AddService(Service service)
        {
            object errormsg;
            string userType = "True";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }

            object successmsg;
            try
            {
                if (ModelState.IsValid)
                {
                    int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
                    ServiceRepository repo = new(HttpContext, userId);
                    repo.AddService(service);
                    successmsg = "Service added successfully";
                    ViewBag.success = successmsg;
                    return View("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(service);
        }
    
        /*
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(User user) //Aauthenticate
        {


            object errormsg;
            IEnumerable<User> CustomersList = db.Users;
            foreach (User u in CustomersList)
            {
                if (u.UserEmail.Equals(user.UserEmail))
                {
                    if (u.Password.Equals(user.Password))
                    {
                        if (!u.UserType) {
                            errormsg = "You dont have permission to login";
                            ViewBag.msg = errormsg;
                            return View("Login");
                        }
                        else { return RedirectToAction("AdminDashboard", "Admin"); }
                            
                        
                    }
                }
            }
            errormsg = "Invalid email or password";
            ViewBag.msg = errormsg;
            return View("Login");
        }
        */

    }
}
