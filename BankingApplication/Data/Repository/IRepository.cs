using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository
{
    public interface IRepository<T>
    {
        ValueTask<T> GetByID(int id);
        Task<List<T>> GetAll();
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
