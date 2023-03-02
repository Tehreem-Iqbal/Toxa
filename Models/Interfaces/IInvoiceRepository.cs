using System;
namespace ProjectManagementApplication.Models.Interfaces
{
	public interface IInvoiceRepository
    {

        void AddInvoice(Invoice invoice);
        void RemoveInvoice(Invoice invoice);
        List<Invoice> RetrieveAllInvoices();
        Invoice? RetrieveInvoice(int id);
        List<Invoice> RetrieveUserInvoice(User user);
        void UpdateInvoice(Invoice invoice);
    }
}

