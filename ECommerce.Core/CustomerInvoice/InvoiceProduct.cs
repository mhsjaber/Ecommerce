using ECommerce.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.CustomerInvoice
{
    public class InvoiceProduct : Entity
    {
        public Guid InvoiceID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
    }
}
