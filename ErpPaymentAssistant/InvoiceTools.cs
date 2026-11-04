using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ErpPaymentAssistant
{
    public class InvoiceTools
    {
        private readonly InvoiceService _invoiceService;
        private readonly List<Invoice> _invoices;

        public InvoiceTools(
            InvoiceService invoiceService,
            List<Invoice> invoices)
        {
            _invoiceService = invoiceService;
            _invoices = invoices;
        }

       

        [Description("Belirtilen müşterinin toplam gecikmiş borcunu hesaplar.")]
        public decimal GetCustomerOverdueAmount(
    [Description("ERP sistemindeki müşteri numarası.")]
    int customerId)
        {
            return _invoiceService.GetTotalOverdueAmount(
                customerId,
                _invoices);
        }
    }
}
