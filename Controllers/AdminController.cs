using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;
namespace ProjectManagementApplication.Controllers
{
    public class AdminController : Controller
    { 
        public IActionResult Index() {
            object errormsg;

            if (!HttpContext.Request.Cookies.ContainsKey("userid"))
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
            try
            {
                if (ModelState.IsValid)
                {
                    ProjectRepository.AddProject(project);
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
            var obj = ProjectRepository.RetrieveProject(id);
            if (obj == null)
            {
                return NotFound();
            }

            ProjectRepository.RemoveProject(obj);
            TempData["success"] = "Project deleted successfully";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DeleteUser() { return View("DeleteUser"); }
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var obj = UserRepository.RetrieveUser(id);
            if (obj == null)
            {
                return NotFound();
            }

            UserRepository.RemoveUser(obj);
            TempData["success"] = "Project deleted successfully";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult AddService() { return View(); }
        [HttpPost]
        public IActionResult AddService(Service service)
        {
            object successmsg;
            try
            {
                if (ModelState.IsValid)
                {
                    ServiceRepository.AddService(service)
                    successmsg = "Service added successfully";
                    ViewBag.success = successmsg;
                    return View("AdminDashboard");
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
