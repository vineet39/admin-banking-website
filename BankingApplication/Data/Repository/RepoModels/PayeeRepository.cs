using BankingApplication.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Repository;
using BankingApplication.Data;

namespace RepositoryWrapper
{
    public class PayeeRepository : Repository<Payee>, IRepository<Payee>
    {
        public PayeeRepository(BankAppContext context) : base(context)
        {

        }
    }
}
