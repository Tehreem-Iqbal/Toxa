using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Models.Interfaces;
using ProjectManagementApplication.Utilities;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace ProjectManagementApplication.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly IInvoiceRepository invoiceRepository;

        DashboardController(IUserRepository _userRepository, IProjectRepository _projectRepository,
                        IServiceRepository _serviceRepository, IInvoiceRepository _invoiceRepository)
        {
            userRepository = _userRepository;
            projectRepository = _projectRepository;
            invoiceRepository = _invoiceRepository;
            serviceRepository = _serviceRepository;
        }
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

            User user = JsonSerializer.Deserialize<User>((string)TempData["user"]!)!;
            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
            List<Project> projects = projectRepository.GetUserProjects(user.Id);
            Tuple<User, List<Project>> tuple = new Tuple<User, List<Project>>(user, projects);
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
                service.CustomerId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
                int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
                serviceRepository.AddPurchasedService(service);
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
