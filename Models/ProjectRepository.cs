using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Models
{
    public class ProjectRepository
    {
        Dbcontext db = new Dbcontext();
        public void AddProject(Project Proj)
        {
            db.Project.Add(Proj);

            db.SaveChanges();
        }
    }
}
