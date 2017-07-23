using ECommerce.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Member
{
    public enum CustomerStatus
    {
        Temporary =1,
        General,
        Premium
    }

    public class Customer : Entity
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public DateTime CreatedOn { get; set; }
        public CustomerStatus Status { get; set; }
    }
}
