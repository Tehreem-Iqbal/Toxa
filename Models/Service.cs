using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int Charges { get; set; }
      
    }
}
