using BankingApplication.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

//Wrapper for utilizing generic repository
//Uses objects created in RepoModels folder
namespace RepositoryWrapper
{
    public class Wrapper
    {
        protected BankAppContext _context;
        private AccountRepository _account;
        private CustomerRepository _customer;
        private TransactionRepository _transaction;
        private BillPayRepository _billpay;
        private LoginRepository _login;
        private PayeeRepository _payee;

        public Wrapper(BankAppContext context)
        {
            _context = context;
        }

        public AccountRepository Account
        {
            get {
                if (_account == null)
                {
                    _account = new AccountRepository(_context);
                }

                return _account;
            }
        }

        public CustomerRepository Customer
        {
            get
            {
                if(_customer == null)
                {
                    _customer = new CustomerRepository(_context);
                }

                return _customer;
            }
        }

        public TransactionRepository Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new TransactionRepository(_context);
                }

                return _transaction;
            }
        }

        public BillPayRepository BillPay
        {
            get
            {
                if (_billpay == null)
                {
                    _billpay = new BillPayRepository(_context);
                }

                return _billpay;
            }
        }

        public LoginRepository Login
        {
            get
            {
                if (_login == null)
                {
                    _login = new LoginRepository(_context);
                }

                return _login;
            }
        }

        public PayeeRepository Payee
        {
            get
            {
                if (_payee == null)
                {
                    _payee = new PayeeRepository(_context);
                }

                return _payee;
            }
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }





    }
}
