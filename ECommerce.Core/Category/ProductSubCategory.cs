using ECommerce.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Category
{
    public class ProductSubCategory : Entity
    {
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
    }
}
