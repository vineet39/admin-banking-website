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
    public class BillPayRepository : Repository<BillPay>, IRepository<BillPay>
    {
        public BillPayRepository(BankAppContext context) : base(context)
        {

        }

        public Task<List<BillPay>> GetDueBills()
        {
            return  _context.BillPay.Where(x => x.ScheduleDate < DateTime.UtcNow)
                .Where(x => x.Locked == false).ToListAsync();
        }

        public Task<List<BillPay>> GetByAccountID(int id)
        {
            return _context.BillPay.Where(x => x.AccountNumber == id).ToListAsync();
        }
    }
}
