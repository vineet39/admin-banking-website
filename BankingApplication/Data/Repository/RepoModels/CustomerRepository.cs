using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace RepositoryWrapper
{
    public class CustomerRepository : Repository<Customer>, IRepository<Customer>
    {
        public CustomerRepository(BankAppContext context) : base(context)
        {

        }

        public Task<Customer> GetWithAccounts(int id)
        {
            return _context.Customer.Where(x => x.CustomerID == id).Include(x => x.Accounts).FirstOrDefaultAsync();
        }

        public Task<List<Customer>> GetWithLock()
        {
            return _context.Customer.Include(x => x.Login).ToListAsync();
        }
    }
}
