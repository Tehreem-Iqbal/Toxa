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
                        UserRepository repo = new UserRepository();
                        repo.AddUser(user);
                        successmsg = "Account created successfully";
                        ViewBag.success = successmsg;
                        return View("Login");
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
                IEnumerable<User> CustomersList = (new UserRepository()).RetrieveUsers();
               
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
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            object errormsg, successmsg;

            try
            {
                if (ModelState.IsValid)
                {
                    if (DuplicateCheck(user.UserEmail)) { errormsg = "User already registered"; ViewBag.msg = errormsg; return View(user); }
                    else
                    {
                        user.UserType = false;
                        UserRepository repo = new UserRepository();
                        repo.AddUser(user);
                        successmsg = "Account created successfully";
                        ViewBag.success = successmsg;
                        return View("Login");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex + "ERROR: Unable to create account. Try again, and if the problem persists contact system administrator.");
            }
            return View(user);
        }
    }
}
