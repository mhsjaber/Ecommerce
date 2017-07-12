using ECommerce.Core.Category;
using ECommerce.Core.Data;
using ECommerce.Core.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core
{
    public class ECommerceUnitOfWork : IDisposable
    {
        private ECommerceContext _context;
        public ECommerceUnitOfWork(ECommerceContext context)
        {
            _context = context;
        }

        private IGenericRepository<ProductSubCategory> productSubCategoryRepository;
        public IGenericRepository<ProductSubCategory> ProductSubCategoryRepository
        {
            get
            {
                if (productSubCategoryRepository == null)
                {
                    productSubCategoryRepository = new GenericRepository<ProductSubCategory>(_context);
                }
                return productSubCategoryRepository;
            }
        }

        private IGenericRepository<ProductCategory> productCategoryRepository;
        public IGenericRepository<ProductCategory> ProductCategoryRepository
        {
            get
            {
                if (productCategoryRepository == null)
                {
                    productCategoryRepository = new GenericRepository<ProductCategory>(_context);
                }
                return productCategoryRepository;
            }
        }

        private IGenericRepository<Product> productRepository;
        public IGenericRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new GenericRepository<Product>(_context);
                }
                return productRepository;
            }
        }

        private IGenericRepository<Customer> customerRepository;
        public IGenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (customerRepository == null)
                {
                    customerRepository = new GenericRepository<Customer>(_context);
                }
                return customerRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
