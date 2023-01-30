using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ProjectManagementApplication.Models
{
    public class User
    {
     
        [Key]
        public int UserId { get; set; }
        public bool UserType { get; set; } 

        [Required(ErrorMessage = "This field is required")]
        public string? UserName { get; set; }
         
        [Required] 
        [EmailAddress] //gail.com
        public string? UserEmail { get; set; }
     
        [Required(ErrorMessage = "This field is required")]
        public string? Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required")]
        public string? Password { get; set; } = string.Empty;


    }
}
