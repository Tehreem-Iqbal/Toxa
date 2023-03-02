using System;
namespace ProjectManagementApplication.Models.Interfaces
{
	public interface IProjectRepository
    {
        void AddProject(Project Proj);
        void UpdateProject(Project project);
        void RemoveProject(Project project);
        Project GetProject(int id);

        int Count();
        List<Project> GetAllProjects();
        List<Project> GetUserProjects(int UserId);
    }
}

