using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BankingApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected BankAppContext _context;
        public Repository(BankAppContext context) => _context = context;
        public ValueTask<T> GetByID(int id)
        {
            return _context.Set<T>().FindAsync(id);
        }

        public Task<List<T>> GetAll()
        {
            return _context.Set<T>().ToListAsync();
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
