using System;
using ProjectManagementApplication.Models.Interfaces;

namespace ProjectManagementApplication.Utilities
{
    public class UserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor accessor;
        public UserResolverService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public string GetUser()
        {
            var username = accessor?.HttpContext?.Request?.Cookies?["user"]!.Split(",")[0];
            return username ?? "unknown";
        }
    }
}

