using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Repository;
using BankingApplication.Data;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryWrapper
{
    public class AccountRepository : Repository<Account>, IRepository<Account>
    {
        public AccountRepository(BankAppContext context) : base(context)
        {
        }

        public Task<Account> GetWithTransactions(int id)
        {
            return _context.Account.Where(x => x.AccountNumber == id).Include(x => x.Transactions).FirstOrDefaultAsync();
        }

        public Task<List<Account>> GetWithTransactionsByCustomerId(int id)
        {
            return _context.Account.Where(x => x.CustomerID == id).Include(x => x.Transactions).ToListAsync();
        }

        public Task<Account> GetWithBills(int id)
        {
            return _context.Account.Where(x => x.AccountNumber == id).Include(x => x.Bills).FirstOrDefaultAsync();
        }

        public Task<List<Account>> GetAccountsByCustomerID(int id)
        {
            return _context.Account.Where(x => x.CustomerID == id).ToListAsync();
        }
    }
}
