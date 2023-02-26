using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Linq;
using System;

namespace ProjectManagementApplication.Data
{
    public class UserRepository
    {
        private readonly HttpContext _httpContext;
        private Dbcontext db;
        public UserRepository(HttpContext httpContext, int userId)
        {
            db = new Dbcontext();
            db.userId = userId;

            _httpContext = httpContext;
        }
        
        public string AddUser(User user)
        {
            try
            {
                string msg;
                if (DuplicateCheck(user.UserEmail!))
                {
                    msg = "duplicate-user";
                    return msg;
                }
                if (!FileUpload(user.Image!, user.ImageURL!))
                {
                    msg = "file-error";
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
        private bool DuplicateCheck(string email)
        {

            IEnumerable<User> CustomersList = RetrieveUsers();
            foreach (User u in CustomersList)
            {
                if (email.Equals(u.UserEmail))
                {
                    return true;
                }
            }
            return false;
        }
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

        public IEnumerable<User> RetrieveUsers()
        {
            return db.Users;
        }
        public User? RetrieveUser(int id)
        {
            return db.Users.Find(id);
        }
        public void RemoveUser(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public int Count()
        {
            return db.Users.Count<User>();
        }
    }
}
