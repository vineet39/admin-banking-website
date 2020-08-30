using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryWrapper;

//Admin API controller
namespace BankingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly Wrapper _repo;

        //Repository object
        public AdminController(Wrapper repo)
        {
            _repo = repo;
        }

        //Return customers with login for obtaining locked status
        [HttpGet("customers")]
        public Task<List<Customer>> Get()
        {
            return _repo.Customer.GetWithLock();
        }

        //Return a customer by ID
        [HttpGet("customers/{id}")]
        public ValueTask<Customer> Get(int id)
        {
            return  _repo.Customer.GetByID(id);
        }

        //Delete a customer
        [HttpPost("deletecustomer")]
        public async Task<IActionResult> DeleteCustomer([FromBody] int id)
        {
            var customer =  await _repo.Customer.GetByID(id);
            if(customer != null) 
            {
            _repo.Customer.Delete(customer);
            await _repo.SaveChanges();
            return Ok();
            }
            return BadRequest();
        }

        //Toggle the lock on a customer account
        [HttpPost("togglelock")]
        public async Task<IActionResult> Post([FromBody] string id)
        {
            var login = await _repo.Login.GetWithCustomer(id);
            if (login != null)
            {
                if (!login.Locked)
                {
                    login.Lock(DateTime.UtcNow.AddMinutes(1));
                }
                else
                {
                    login.UnLock();
                }
                await _repo.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        //Get account by ID
        [HttpGet("accounts/{id}")]
        public Task<List<Account>> GetAccountsByCustomerID(int id)
        {
            return _repo.Account.GetAccountsByCustomerID(id);
        }

        //Get transactions by account ID
        [HttpGet("transactions/{id}")]
        public Task<List<Transaction>> GetTransactionsByAccountID(int id)
        {
            return _repo.Transaction.GetTransactionsByAccountID(id);
        }

        //Get transactions within a specified date
        [HttpGet("transactions/{id}:{startdate}:{enddate}")]
        public Task<List<Transaction>> GetTransactionsByDate(int id, string startdate, string enddate)
        {
            return _repo.Transaction.GetWithinDate(id, DateTime.Parse(startdate), DateTime.Parse(enddate));
        }

        //Get list of bills by account
        [HttpGet("billpay/{id}")]
        public Task<List<BillPay>> GetBillPayByAccountID(int id)
        {
            return _repo.BillPay.GetByAccountID(id);
        }

        //Lock a bill
        [HttpPost("billlock")]
        public async Task<IActionResult> Post([FromBody] int id)
        {
            var bill = await _repo.BillPay.GetByID(id);
            if (bill != null)
            {
                if (!bill.Locked)
                {
                    bill.Lock();
                }
                else
                {
                    bill.UnLock();
                }
                await _repo.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }

        //Update customer profile
        [HttpPost("updatecustomer")]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            if (customer != null)
            {
                _repo.Customer.Update(customer);
                await _repo.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}