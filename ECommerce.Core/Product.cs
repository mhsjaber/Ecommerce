using ECommerce.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public Guid SubCategoryID { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemNumber { get; set; }
        public int Price { get; set; }
        public int UnitAvailable { get; set; }
        public string Image { get; set; }
    }
}