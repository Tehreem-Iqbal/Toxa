
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Data
{
    public class ServiceRepository
    {
        private static Dbcontext db = new Dbcontext();
        public static void AddService(Service s)
        {
            db.Service.Add(s);

            db.SaveChanges();
        }
        public static Service? RetrieveProject(int id)
        {
            return db.Service.Find(id);
        }
        public static void RemoveProject(Service s)
        {
            db.Service.Remove(s);
            db.SaveChanges();
        }
    }

}
