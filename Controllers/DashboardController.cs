﻿using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Controllers
{
    public class DashboardController : Controller
    {
        Dbcontext db = new Dbcontext();
        public IActionResult Dashboard(User u)
        {
            IEnumerable<Project> ProjectList = db.Project;
            List<Project> UserProjects = new List<Project>();
            foreach (Project p in ProjectList)
            {
                if (p.CustomerId == u.UserId)
                {
                   UserProjects.Add(p);
                }
            }
            return View(UserProjects);
        }

        public IActionResult PurchaseService(Service s) {
            PurchasedServices service = new PurchasedServices();
            service.Name = s.Name;
            service.Description = s.Description;
            service.Charges = s.Charges;
            service.Status = true;
            service.CustomerId = 1;
            try
            {
                db.PurchasedServices.Add(service);
                db.SaveChanges();
                return View("Services");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to create account. Try again, and if the problem persists contact system administrator.");
            }
            return View(Services);

        }
    }
}