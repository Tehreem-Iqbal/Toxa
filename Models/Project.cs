using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int Cost { get; set; }
        [Required]
        public  int CustomerId { get; set; }

        public int CompletionRate { get; set; }
        public bool ProjectStatus { get; set; } 
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }

    }
}
