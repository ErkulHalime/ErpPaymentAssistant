using System;
using System.Collections.Generic;
using System.Text;

namespace ErpPaymentAssistant
{
    public class InvoiceService
    {
        public List<Invoice> GetOverdueInvoices(List<Invoice> invoices)
        {
            return invoices
                .Where(invoice =>
                    invoice.IsPaid == false &&
                    invoice.DueDate.Date < DateTime.Today)
                .ToList();
        }

        public decimal GetTotalOverdueAmount(
    int customerId,
    List<Invoice> invoices)
        {
            return GetOverdueInvoices(invoices)
                .Where(invoice => invoice.CustomerId == customerId)
                .Sum(invoice => invoice.Amount);
        }
    }
}
