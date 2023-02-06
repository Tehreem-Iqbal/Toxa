using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Data
{
    public class UserRepository
    {
        private static Dbcontext db = new Dbcontext();
        public static string AddUser(User user)
        {
            string msg;
            if (DuplicateCheck(user.UserEmail)) {
                msg = "User already exists";
                return msg;
            }
            if (!FileUpload(user.Image, user.ImageURL)) {
                msg = "File Not Uploaded";
                return msg;
            }
            db.Users.Add(user);
            db.SaveChanges();
            msg = "User Added";
            return msg;
        }
        private static bool DuplicateCheck(string email)
        {

            IEnumerable<User> CustomersList = UserRepository.RetrieveUsers();
            foreach (User u in CustomersList)
            {
                if (email.Equals(u.UserEmail))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool FileUpload(IFormFile file, string path)
        {
            Console.WriteLine("upload1");
            
            try
            {
                if (file.Length > 0)
                {
                    

                    Console.WriteLine(path);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                        Console.WriteLine("dir created");
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    Console.WriteLine("upload2");
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed saving file", ex);
            }
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
