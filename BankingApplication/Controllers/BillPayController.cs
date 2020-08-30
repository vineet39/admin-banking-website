using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryWrapper;
using BankingApplication.Attributes;
using System;

namespace BankingApplication.Controllers
{
    [AuthorizeCustomer]
    public class BillPayController : Controller
    {
        //Repository object
        private readonly Wrapper _repo;

        public BillPayController(Wrapper repo) {
            _repo = repo;
        }

        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        //Method for preparing create bill page
        //This controller method handles both creation of a new bill
        //and taking a modified bill from the bill schedule page
        //Utilizes the BillViewModel object
        public async Task<IActionResult> CreateBill(int billID = 0)
        {
            HttpContext.Session.SetInt32("Mod", 0);
            ViewData["Mod"] = 0;
            var customer = await _repo.Customer.GetWithAccounts(CustomerID);
            var billviewmodel = new BillViewModel { Customer = customer };
            if (billID != 0)
            {
                var bill = await _repo.BillPay.GetByID(billID);
                billviewmodel.Billpay = bill;
                HttpContext.Session.SetInt32("Mod", billID);
                ViewData["Mod"] = billID;
            }
            var list = await _repo.Payee.GetAll();
            billviewmodel.SetPayeeDictionary(list);
            return View(billviewmodel);
        }


        //Method for creating bill from posted information
        //Handles both new bills and modified bills
        //Utilizes the BillViewModel object
        [HttpPost]
        public async Task<IActionResult> CreateBill(BillViewModel billv)
        {
            var customer = await _repo.Customer.GetWithAccounts(CustomerID);
            var list = await _repo.Payee.GetAll();
            billv.Customer = customer;
            billv.SetPayeeDictionary(list);
            if (!ModelState.IsValid)
            {
                return View(billv);
            }
            var mod = HttpContext.Session.GetInt32("Mod");
            billv.Billpay.FKAccountNumber = await _repo.Account
                .GetByID(billv.SelectedAccount);
            billv.Billpay.FKPayeeID = await _repo.Payee.GetByID(billv.SelectedPayee);
            if (mod != 0)
            {
                var bill = await _repo.BillPay.GetByID((int)mod);
                bill.UpdateBill(billv.Billpay);
                _repo.BillPay.Update(bill);
            }
            else { _repo.BillPay.Update(billv.Billpay);}
            await _repo.SaveChanges();
            ModelState.AddModelError("BillCreatedSuccess", "Bill has been saved.");
            var billviewmodel = new BillViewModel { Customer = customer };
            billviewmodel.SetPayeeDictionary(list);
            return View(billviewmodel);
        }

        //Method for prompting user to select account to view bills from
        public async Task<IActionResult> SelectAccount()
        {
            var accounts = await _repo.Account.GetAccountsByCustomerID(CustomerID);

            return View(accounts);
        }

        //Method for preparing BillSchedule page
        //Utilizes BillScheduleViewModel object
        public async Task<IActionResult> BillSchedule(int accountNumber)
        {
            var account = await _repo.Account.GetWithBills(accountNumber);
            var list = await _repo.Payee.GetAll();
            BillScheduleViewModel bills = new BillScheduleViewModel(account, list);
            return View(bills);
        }

        //Method for returning partial view of balance total
        public async Task<IActionResult> SeeMyBalance(int id)
        {
            
            var account = await _repo.Account.GetByID(id);
            return PartialView(account);
            
        }

        //Method for handling deletion of existing bills
        public async Task<IActionResult> DeleteBill(int id)
        {
            var bill = await _repo.BillPay.GetByID(id);
            _repo.BillPay.Delete(bill);
            await _repo.SaveChanges();
            return RedirectToAction("CreateBill", "BillPay");
        }
    }
}