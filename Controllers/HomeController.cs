using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Diagnostics;
using ProjectManagementApplication.Models.Interfaces;

namespace ProjectManagementApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IProjectRepository projectRepository;
        private readonly IServiceRepository serviceRepository;
        private readonly IInvoiceRepository invoiceRepository;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUserRepository _userRepository, IProjectRepository _projectRepository,
                        IServiceRepository _serviceRepository, IInvoiceRepository _invoiceRepository)
        {
            _logger = logger;

            userRepository = _userRepository;
            projectRepository = _projectRepository;
            invoiceRepository = _invoiceRepository;
            serviceRepository = _serviceRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Services()
        {
            List<Service> ServiceList = serviceRepository.GetAllServices();
            return View(ServiceList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}