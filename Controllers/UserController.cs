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
      
        Dbcontext db = new Dbcontext();
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User user) //Add customer
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
                        db.Users.Add(user);
                        db.SaveChanges();
                        successmsg = "Account created successfully";
                        ViewBag.success = successmsg;
                        return View("Login");
                    }
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to create account. Try again, and if the problem persists contact system administrator.");
            }
            return View(user); 
        }

        [NonAction]
        public bool DuplicateCheck(string email)
        {
            IEnumerable<User> CustomersList = db.Users;
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
                IEnumerable<User> CustomersList = db.Users;
                foreach (User u in CustomersList)
                {
                    if (u.UserEmail.Equals(user.UserEmail))
                    {
                        if (u.Password.Equals(user.Password))
                        {
                            if (u.UserType)
                            {
                                errormsg = "You dont have permission to login";
                                ViewBag.msg = errormsg;
                                return View("Login");
                            }
                            else { return RedirectToAction("Dashboard", "Dashboard", u); }
                        }
                    }
                }
                errormsg = "Invalid email or password";
                ViewBag.msg = errormsg;
                return View("Login");
            }
            catch (Exception ex) { ModelState.AddModelError("", "Unable to login. Try again, and if the problem persists contact system administrator."); }
            return View("Login");
        }

    }
}
