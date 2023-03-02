
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Models.Interfaces;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Data
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly Dbcontext db;

        public ServiceRepository(Dbcontext dbcontext)
        {
            db = dbcontext;
        }

        public void AddService(Service s)
        {
            db.Service.Add(s);

            db.SaveChanges();
        }
        public void AddPurchasedService(PurchasedServices service)
        {
            db.PurchasedServices.Add(service);

            db.SaveChanges();
        }
        public Service? RetrieveService(int id)
        {
            return db.Service.Find(id);
        }
        public void RemoveService(Service s)
        {
            db.Service.Remove(s);
            db.SaveChanges();
        }
        public int Count()
        {
            return db.Service.Count<Service>();
        }
        public List<Service> GetAllServices()
        {
            return db.Service.ToList<Service>();
        }
        public void UpdateService(Service service)
        {
            db.Service.Update(service);
            db.SaveChanges();
        }
    }

}
