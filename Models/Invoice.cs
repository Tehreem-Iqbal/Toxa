using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Models
{
    public class Invoice
    {
        [Key]
        public int InvoiceId { get; set; } 
        public int CustomerId { get; set; }
        public int Bill { get; set; }
        public bool BillStatus { get; set; }

    }
}
