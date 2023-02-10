﻿using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Data
{
    public class ProjectRepository
    {
        private Dbcontext db;
        private readonly HttpContext _httpContext;
        public ProjectRepository(HttpContext httpContext, int userId)
        {
            _httpContext = httpContext;
            db = new Dbcontext();
            db.userId = userId;
        }
        public void AddProject(Project Proj)
        {
            string cookie = _httpContext.Request.Cookies["user"]!;
            // Proj.CreatedByUserId();
            db.Project.Add(Proj);

            db.SaveChanges();
        }
        public Project? RetrieveProject(int id)
        {
            return db.Project.Find(id);
        }
        public void RemoveProject(Project project)
        {
            db.Project.Remove(project);
            db.SaveChanges();
        }

        public List<Project> RetrieveUserProjects(User user)
        {
            List<Project> ProjectList = new List<Project>();
            IEnumerable<Project> AllProjects = db.Project;
            foreach (Project poject in AllProjects)
            {
                if (poject.CustomerId == user.Id)
                {
                    ProjectList.Add(poject);
                }
            }
            return ProjectList;
        }
    }

}
