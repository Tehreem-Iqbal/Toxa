using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Models.Interfaces;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Data
{
    public class ProjectRepository : IProjectRepository
    {

        private readonly Dbcontext db;

        public ProjectRepository(Dbcontext dbcontext)
        {
            db = dbcontext;
        }
        public void AddProject(Project Proj)
        {

            //List<Project> projects = db.Project.ToList<Project>();
            //Array projectIds = projects.Select(p => p.Id).ToArray();

            //int randNum;
            //Random rnd = new Random();

            //do
            //{
            //    randNum = rnd.Next(1, 101); // generate random number between 1-100 (inclusive)
            //} while (Array.IndexOf(projectIds, randNum) != -1); // keep generating random numbers until one is not in the array

            //Console.WriteLine($"Project {Proj.Name} is assigned {randNum} ID.");
            //Proj.Id = randNum;

            //var lastRow = db.Project.OrderByDescending(r => r.Id).FirstOrDefault();

            //Proj.Id = 3;//lastRow.Id + 1;


            db.Project.Add(Proj);

            db.SaveChanges();
        }
        public Project GetProject(int id)
        {
            return db.Project.Find(id)!;
        }
        public void RemoveProject(Project project)
        {
            db.Project.Remove(project);
            db.SaveChanges();
        }
        public List<Project> GetUserProjects(int userId)
        {
            List<Project> ProjectList = new List<Project>();
            IEnumerable<Project> AllProjects = db.Project;
            foreach (Project poject in AllProjects)
            {
                if (poject.CustomerId == userId)
                {
                    ProjectList.Add(poject);
                }
            }
            return ProjectList;
        }
        public List<Project> GetAllProjects()
        {
            return db.Project.ToList<Project>();
        }
        public void UpdateProject(Project project)
        {
            db.Project.Update(project);
            db.SaveChanges();
        }
        public int Count()
        {

            return db.Project.ToList<Project>().Count();
        }
    }

}
