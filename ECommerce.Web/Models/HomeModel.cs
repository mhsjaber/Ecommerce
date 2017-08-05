using ECommerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Models
{
    public class HomeModel
    {
        public List<Product> Products { get; set; }
        public List<Product> FeaturedProducts { get; set; }
    }
}