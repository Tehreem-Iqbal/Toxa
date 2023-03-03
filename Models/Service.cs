using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Models
{
    [Serializable]
    public class Service : FullAuditModel
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public int Charges { get; set; }
      
    }
}
