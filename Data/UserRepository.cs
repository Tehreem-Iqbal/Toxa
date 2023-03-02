using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Models.Interfaces;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace ProjectManagementApplication.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly Dbcontext db;

        public UserRepository(Dbcontext dbcontext)
        {
            db = dbcontext;
        }

        public string AddUser(User user)
        {
            try
            {
                string msg;
                if (!FileUpload(user.Image!, user.ImageURL!))
                {
                    msg = "file-error";
                    return msg;
                }
                if (DuplicateCheck(user.UserEmail!))
                {
                    msg = "duplicate-user";
                    return msg;
                }

                db.Users.Add(user);
                db.SaveChanges();
                msg = "success";
                return msg;
            }
            catch (Exception ex)
            {
                throw new Exception($"User not added: {ex}");
            }
        }
        public User? GetUser(int id)
        {
            return db.Users.Find(id);
        }
        public string UpdateUser(User user)
        {
            try
            {
                string msg;
                db.Users.Update(user);
                db.SaveChanges();
                msg = "success";
                return msg;
            }
            catch (Exception ex)
            {
                throw new Exception($"User not Updated: {ex}");
            }
        }
        public string RemoveUser(int userId)
        {
            string msg = "success";
            User? user = GetUser(userId);
            if(user == null)
            {
                return "fail";
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return msg;
        }

        public List<User> GetAllUsers()
        {
            return db.Users.ToList<User>();
        }
        public int Count()
        {
            return db.Users.Count<User>();
        }

        // Helper Functions

        private bool FileUpload(IFormFile file, string path)
        {
            int index = path.LastIndexOf("/");
            string dir = string.Concat("wwwroot/", path.AsSpan(0, index));
            try
            {
                if (file.Length > 0)
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    using (var fileStream = new FileStream(Path.Combine(dir, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    Console.WriteLine("File has been uplaoded");
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
        private bool DuplicateCheck(string email)
        {

            IEnumerable<User> CustomersList = GetAllUsers();
            foreach (User u in CustomersList)
            {
                if (email.Equals(u.UserEmail))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
