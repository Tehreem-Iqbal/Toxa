using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Models
{
    public class PurchasedServices
    {
        [Key]
        public int ServiceId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int CustomerId { get; set; }

        public bool Status { get; set; }
        public int Charges { get; set; }

        //public DateOnly PurchasedDate { get; set; }
        //public DateOnly ExpiryDate { get; set; }
    }
}
