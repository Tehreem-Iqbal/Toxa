namespace ProjectManagementApplication.Utilities
{
    public class HttpUtilities
    {
        public static bool ValidateState(HttpContext context, string key)
        {
            string? cookie = context.Request.Cookies["user"];
            if (cookie != null)
            {
                string[] argv = cookie.Split(",");
                if (argv.Contains(key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
