using ProjectManagementApplication.Models.Interfaces;
using ProjectManagementApplication.Utilities;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System.Data;
using System.Linq;

namespace ProjectManagementApplication.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly IInvoiceRepository invoiceRepository;

        public DashboardController(IUserRepository _userRepository, IProjectRepository _projectRepository,
                        IServiceRepository _serviceRepository, IInvoiceRepository _invoiceRepository)
        {
            userRepository = _userRepository;
            projectRepository = _projectRepository;
            invoiceRepository = _invoiceRepository;
            serviceRepository = _serviceRepository;
        }
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

            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
            int numberOfProjects = projectRepository.GetUserProjects(userId).Count();
            int numberOfServices = serviceRepository.GetUserPurchasedServices(userId).Count();

            User user = userRepository.GetUser(userId)!;

            Console.WriteLine($"User: {user} is going to user admin");
            Tuple<User, Tuple<int, int>> tuple = new(user, new Tuple<int, int>(numberOfProjects, numberOfServices));
            return View(tuple);
        }

        public IActionResult DisplayUserProjects()
        {
            object errormsg;
            string userType = "False";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }

            int userId =  int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);

            User user = userRepository.GetUser(userId)!;

            List<Project> projects = projectRepository.GetUserProjects(userId);

            Tuple<User, List<Project>> tuple = new Tuple<User, List<Project>>(user, projects);
            return View(tuple);
        }
        public IActionResult DisplayUserServices() {
            object errormsg;
            string userType = "False";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }

            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);

            User user = userRepository.GetUser(userId)!;

            List<PurchasedServices> purchasedServices = serviceRepository.GetUserPurchasedServices(userId);
        
            Tuple<User, List<PurchasedServices>> tuple = new Tuple<User, List<PurchasedServices>>(user, purchasedServices);
            return View(tuple);
        }

        public IActionResult DeleteService(int serviceId)
        {
            object errormsg;
            string userType = "False";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }
            //serviceRepository.RemoveService()
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult PurchaseService()
        {
            object errormsg;
            string userType = "False";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }

            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);

            User user = userRepository.GetUser(userId)!;

            List<Service> services = serviceRepository.GetAllServices();

            Tuple<User, List<Service>> tuple = new Tuple<User, List<Service>>(user,services);
            return View(tuple);
        }
        [HttpPost]
        public IActionResult PurchaseService(int serviceId)
        {
            object errormsg;
            string userType = "False";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }
            Service s = serviceRepository.RetrieveService(serviceId)!;

            PurchasedServices service = new PurchasedServices();
            try
            {
                service.Name = s.Name;
                service.Description = s.Description;
                service.Charges = s.Charges;
                service.Status = true;
                service.CustomerId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);

                serviceRepository.AddPurchasedService(service);

                return RedirectToAction("Index");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to create account. Try again, and if the problem persists contact system administrator.");
            }
            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult PlaceProject()
        {
            object errormsg;
            string userType = "False";
            if (!HttpUtilities.ValidateState(HttpContext, userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }

            return View();
        }

        [HttpPost]
        public IActionResult PlaceProject(Project project)
        {
            object errormsg;
            string userType = "False";
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
                    project.CustomerId = userId;
                    project.CompletionRate = 0;
                    project.ProjectStatus = false;

                    projectRepository.AddProject(project);
                    TempData["success"] = "Project details added successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction("Index");
        }
    }
}
