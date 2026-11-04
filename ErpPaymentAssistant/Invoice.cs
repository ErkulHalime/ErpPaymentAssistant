using System;
using System.Collections.Generic;
using System.Text;

namespace ErpPaymentAssistant
{
    public class Invoice
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public decimal Amount { get; set; }

        public DateTime InvoiceDate { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsPaid { get; set; }
    }
}
