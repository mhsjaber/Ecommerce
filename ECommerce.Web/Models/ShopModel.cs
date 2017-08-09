using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Models
{
    public class ProductList
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public string ImageLink { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public double Total { get; set; }
        public Guid ID { get; set; }
    }

    public class ShopModel
    {
        public List<ProductList> Products { get; set; }
        public double GrandTotal { get; set; }
    }

    public class CartUpdateModel
    {
        public Guid[] ProductInvoiceID { get; set; }
        public int[] Quantity { get; set; }
    }

    public class ShipmentModel
    {
        public double Total { get; set; }
    }
}