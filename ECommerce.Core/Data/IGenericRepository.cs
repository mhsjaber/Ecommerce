using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Data
{
    public interface IGenericRepository<T> where T : Entity
    {
        void Add(T item);
        IEnumerable<T> GetAll();
        T GetById(Guid Id);
        void Delete(T item);
        void Update(T item);
    }
}
