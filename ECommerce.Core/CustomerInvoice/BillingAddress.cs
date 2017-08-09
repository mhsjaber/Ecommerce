using ECommerce.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.CustomerInvoice
{
    public class BillingAddress : Entity
    {
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string OptionalMobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
