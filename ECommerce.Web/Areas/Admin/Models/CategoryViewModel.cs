using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECommerce.Web.Areas.Admin.Models
{
    public class CategoryViewModel
    {
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
    }

    public class SubCategoryViewModel
    {
        public Guid SubCategoryID { get; set; }
        public Guid CategoryID { get; set; }
        public string Name { get; set; }
    }
}