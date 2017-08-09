using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Models
{
    public class ShipAddressModel
    {
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public string OptionalMobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}