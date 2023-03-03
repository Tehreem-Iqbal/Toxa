using System.ComponentModel.DataAnnotations;

namespace ProjectManagementApplication.Models
{
    [Serializable]
    public class Invoice : FullAuditModel
    {
        public int CustomerId { get; set; }
        public int Bill { get; set; }
        public bool BillStatus { get; set; }

    }
}
