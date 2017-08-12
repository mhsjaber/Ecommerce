using ECommerce.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Member
{
    public enum AdminType
    {
        SuperAdmin,
        Admin
    }

    public class Admin : Entity
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedOn { get; set; }
        public AdminType Type { get; set; }
    }
}
