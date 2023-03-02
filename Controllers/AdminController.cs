using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Models.Interfaces;
using ProjectManagementApplication.Utilities;
using System.Data;
using System.Globalization;
using System.Linq;
namespace ProjectManagementApplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly IInvoiceRepository invoiceRepository;

        public AdminController(IUserRepository _userRepository, IProjectRepository _projectRepository,
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
        public IActionResult Index() {
            object errormsg;
            string userType = "True";
            if (!HttpUtilities.ValidateState(HttpContext,userType))
            {
                errormsg = "YOU ARE NOT LOGGED IN";
                TempData["login_error"] = errormsg;
                return RedirectToAction("Login", "User", errormsg);
            }
            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
            User user = userRepository.GetUser(userId)!;
            List<Invoice> invoices = invoiceRepository.RetrieveAllInvoices();

            int users_count = userRepository.Count();
            int services_count = serviceRepository.Count();
            int projects_count = serviceRepository.Count();

            Tuple<int, int, int> info_tuple = new(projects_count, services_count, users_count);
            Tuple<User,List<Invoice>, Tuple<int,int,int>> tuple = new(user,invoices,info_tuple);
            return View(tuple);
        }

        // Projects
        [HttpGet]
        public IActionResult AddProject() {
            Console.WriteLine("Ustaad g very great.");

            return View();
        }
        [HttpPost]
        public IActionResult AddProject(Project project)
        {
                    Console.WriteLine("Ustaad g great.");
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
                    projectRepository.AddProject(project);
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
        public IActionResult EditProject(int projectId)
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
            Project proj = projectRepository.GetProject(projectId);
            Console.WriteLine($"Project reterived {proj.Name}");
            return View(proj);
        }
        [HttpPost]
        public IActionResult EditProject(Project project)
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
                    projectRepository.UpdateProject(project);
                    TempData["success"] = "Project details updated successfully";
                    return RedirectToAction("DisplayAllProjects");
                }
            }
            catch (DataException)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction("DisplayAllProjects");
        }
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
            var obj = projectRepository.GetProject(id);
            if (obj == null)
            {
                return NotFound();
            }

            projectRepository.RemoveProject(obj);
            TempData["success"] = "Project deleted successfully";
            return RedirectToAction("Index");
        }
        public IActionResult DisplayAllProjects()
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

            List<Project> projects = projectRepository.GetAllProjects();

            Tuple<User, List<Project>> tuple = new(userRepository.GetUser(userId)!, projects);

            return View(tuple);
        }


        // Services
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
                    serviceRepository.AddService(service);
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
        [HttpGet]
        public IActionResult EditService(int serviceId)
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
            Service? service = serviceRepository.RetrieveService(serviceId);
            if(service == null)
            {
                Console.WriteLine("AdminController.EditServiceERRO: Service not found.");
                return NotFound();
            }
            Console.WriteLine($"Service reterived {service.Name}");
            return View(service);
        }
        [HttpPost]
        public IActionResult EditService(Service service)
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
                    serviceRepository.UpdateService(service);
                    TempData["success"] = "Service details updated successfully";
                    return RedirectToAction("DisplayAllServices");
                }
            }
            catch (DataException)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return RedirectToAction("DisplayAllServices");
        }
        public IActionResult DeleteService(int id)
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
            Service? service = serviceRepository.RetrieveService(id);
            if (service == null)
            {
                return NotFound();
            }

            serviceRepository.RemoveService(service);
            TempData["success"] = "Service deleted successfully";
            return RedirectToAction("DisplayAllServices");
        }
        public IActionResult DisplayAllServices()
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

            List<Service> services = serviceRepository.GetAllServices();

            Tuple<User, List<Service>> tuple = new(userRepository.GetUser(userId)!, services);

            return View(tuple);
        }



        // Users
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

            userRepository.RemoveUser(id);
            TempData["success"] = "Project deleted successfully";
            return View("DisplayAllUsers");
        }
        public IActionResult UserDetails(int userid)
        {
            int userId = int.Parse(HttpContext.Request.Cookies["user"]!.Split(",")[0]);
            User? user = userRepository.GetUser(userid);
            if(user == null)
            {
                Console.WriteLine("AdminController.UserDetailsERROR: user not found.");
                return NotFound();
            }
            return View(user);
        }
        public IActionResult DisplayAllUsers()
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
            User user = userRepository.GetUser(userId)!;
            Tuple<User, List<User>> tuple = new(user, userRepository.GetAllUsers());
            return View(tuple);

        }

    }
}
