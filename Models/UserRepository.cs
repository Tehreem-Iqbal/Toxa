using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Models
{
    public class UserRepository
    {
        Dbcontext db = new Dbcontext();
        public void AddUser(User user)
        {
            db.Users.Add(user);

            db.SaveChanges();
        }

        public IEnumerable<User> RetrieveUsers()
        {
            return db.Users;
        }
    }
}
