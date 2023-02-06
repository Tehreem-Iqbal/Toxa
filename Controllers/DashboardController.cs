using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Utilities;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace ProjectManagementApplication.Controllers
{
    public class DashboardController : Controller
    {
        
        public IActionResult Index()
        {
            object errormsg;
            string userType = "False";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }

            //User user = JsonSerializer.Deserialize<User>(HttpContext.Session.GetString("user")!)!;
            User user = JsonSerializer.Deserialize<User>((string)TempData["user"])!;
            List<Project> projects = ProjectRepository.RetrieveUserProjects(user);
            Tuple<User, List<Project>> tuple = new Tuple<User, List<Project>>(user, projects);
            Console.WriteLine(tuple.Item1.UserName);
            return View(tuple);
        }

        public IActionResult PurchaseService(Service s)
        {
            object errormsg;
            string userType = "False";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }

            PurchasedServices service = new PurchasedServices();
            try
            {
                service.Name = s.Name;
                service.Description = s.Description;
                service.Charges = s.Charges;
                service.Status = true;
                service.CustomerId = int.Parse(HttpContext.Request.Cookies["user"].Split(",")[0]);
                ServiceRepository.AddPurchasedService(service);
                return View("Services");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to create account. Try again, and if the problem persists contact system administrator.");
            }
            return View("Services");

        }
    }
}
