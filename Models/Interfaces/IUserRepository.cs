using System;
//using ProjectManagementApplication.Models;

namespace ProjectManagementApplication.Models.Interfaces
{
	public interface IUserRepository
    {

        public string AddUser(User user);
		public string UpdateUser(User user);
		public string RemoveUser(int userId);
		public User? GetUser(int Id);

		public int Count();
		public List<User> GetAllUsers();

	}
}

