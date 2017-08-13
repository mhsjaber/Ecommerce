using ECommerce.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.CustomerInvoice
{
    public enum InvoiceStatus
    {
        InProgress = 1,
        PlacedOrder,
        OnDelivery,
        Delivered,
        Cancelled
    }

    public class Invoice : Entity
    {
        public DateTime CreatedOn { get; set; }
        public Guid? CustomerID { get; set; }
        public InvoiceStatus Status { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }
        public Guid? BillingAddressID { get; set; }
    }
}
