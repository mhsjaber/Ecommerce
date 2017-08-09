using ECommerce.Core.Category;
using ECommerce.Core.CustomerInvoice;
using ECommerce.Core.Member;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core
{
    public class ECommerceContext : DbContext
    {
        public ECommerceContext()
            : base("name=DefaultConnection")
        {

        }

        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<ProductCategory> ProductCategory { get; set; }
        public virtual DbSet<ProductSubCategory> ProductSubCategory { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<InvoiceProduct> InvoiceProduct { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<FeaturedProduct> FeaturedProduct { get; set; }
        public virtual DbSet<BillingAddress> BillingAddress { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}