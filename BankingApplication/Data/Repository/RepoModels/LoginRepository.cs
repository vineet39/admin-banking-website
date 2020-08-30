using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Repository;
using BankingApplication.Data;
using System.Threading.Tasks;
using System.Linq;

namespace RepositoryWrapper
{
    public class LoginRepository : Repository<Login>, IRepository<Login>
    {
        public LoginRepository(BankAppContext context) : base(context)
        {
        }

        public Task<Login> GetWithCustomer(string id)
        {
            return _context.Login.Where(x => x.UserID == id).Include(x => x.Customer).FirstOrDefaultAsync();
        }

        public Task<List<Login>> GetLocked()
        {
            return _context.Login.Where(x => x.Locked == true).ToListAsync();
        }
    }
}
