using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Models;
using ProjectManagementApplication.Models.Interfaces;
using ProjectManagementApplication.Utilities;
using System.Data;
using System.Globalization;
using System.Linq;

namespace ProjectManagementApplication.Data
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly Dbcontext db;

        public InvoiceRepository(Dbcontext dbcontext)
        {
            db = dbcontext;
        }
        public void AddInvoice(Invoice invoice)
        {
            db.Invoice.Add(invoice);

            db.SaveChanges();
        }
        public Invoice? RetrieveInvoice(int id)
        {
            return db.Invoice.Find(id);
        }
        public void RemoveInvoice(Invoice invoice)
        {
            db.Invoice.Remove(invoice);
            db.SaveChanges();
        }

        public List<Invoice> RetrieveUserInvoice(User user)
        {
            List<Invoice> InvoiceList = new List<Invoice>();
            IEnumerable<Invoice> AllInvoices = db.Invoice;
            foreach (Invoice invoice in AllInvoices)
            {
                if (invoice.CustomerId == user.Id)
                {
                    InvoiceList.Add(invoice);
                }
            }
            return InvoiceList;
        }

        public List<Invoice> RetrieveAllInvoices()
        {
            return db.Invoice.ToList<Invoice>();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            db.Invoice.Update(invoice);
            db.SaveChanges();
        }
    }

}
