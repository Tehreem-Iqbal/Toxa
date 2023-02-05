using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Data
{
    public class ProjectRepository
    {
        private static Dbcontext db = new Dbcontext();
        public static void AddProject(Project Proj)
        {
            db.Project.Add(Proj);

            db.SaveChanges();
        }
        public static Project? RetrieveProject(int id)
        {
            return db.Project.Find(id);
        }
        public static void RemoveProject(Project project)
        {
            db.Project.Remove(project);
            db.SaveChanges();
        }
    }

}
