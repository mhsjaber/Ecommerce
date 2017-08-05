using ECommerce.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Models
{
    public class ProductModel
    {
        public List<Product> Products { get; set; }
        public int Index { get; set; } = 1;
        public int TotalPages { get; set; }
    }
    public class ProductDetailsModel
    {
        public Product Product { get; set; }
        public List<Product> MoreProducts { get; set; }
    }
}