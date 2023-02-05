using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Data
{
    public class UserRepository
    {
        private static Dbcontext db = new Dbcontext();
        public static void AddUser(User user)
        {
            db.Users.Add(user);

            db.SaveChanges();
        }

        public static IEnumerable<User> RetrieveUsers()
        {
            return db.Users;
        }
        public static User? RetrieveUser(int id)
        {
            return db.Users.Find(id);
        }
        public static void RemoveUser(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }
    }
}
