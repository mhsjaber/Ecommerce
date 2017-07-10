using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : Entity
    {
        private ECommerceContext _context;

        public GenericRepository(ECommerceContext context)
        {
            _context = context;
        }

        private IDbSet<T> DbSet
        {
            get
            {
                return _context.Set<T>();
            }
        }

        public void Add(T item)
        {
            DbSet.Add(item);
        }

        public void Delete(T item)
        {
            DbSet.Remove(item);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.AsQueryable();
        }

        public T GetById(Guid Id)
        {
            return DbSet.Find(Id);
        }

        public void Update(T item)
        {
            DbSet.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
        }
    }
}