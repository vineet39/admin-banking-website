using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RepositoryWrapper;
using BankingApplication.Attributes;

namespace BankingApplication.Controllers
{   
    
    [AuthorizeCustomer]
    public class CustomerController : Controller
    {
        //Repository object
        private readonly Wrapper _repo;
        public CustomerController(Wrapper repo)
        {
            _repo = repo;
        }
        private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

        private async Task<Customer> GetCustomerData() 
        {
            var customer = await _repo.Customer.GetWithAccounts(CustomerID);
            return customer;
        }

        // Go to edit profile page.
        public async Task<IActionResult> EditProfile() {
            
           var customer =  await GetCustomerData();

            return View(customer);
        } 

        // Go to change password page.
        public ViewResult ChangePassword() {
            return View();
        } 
        

        // Editing all customer attributes except password.
        [HttpPost]
        public async Task<IActionResult> EditProfile(Customer customer){
            if (ModelState.IsValid)
            {
                _repo.Customer.Update(customer);
                await _repo.SaveChanges();
                ModelState.AddModelError("EditSuccess", "Profile edited successfully.");
                // Updating session variable storing customer name.
                HttpContext.Session.SetString(nameof(Customer.CustomerName), customer.CustomerName);
            }
            return View(customer);

        }


        // Editing customer's password.
        public async Task<IActionResult> SavePassword(string oldpassword,string newpassword,string confirmnewpassword){
            var userID = HttpContext.Session.GetString(nameof(Login.UserID));
            var login = await _repo.Login.GetByID(int.Parse(userID));
            var result = login.ChangePassword(oldpassword, newpassword, confirmnewpassword);
            ModelState.AddModelError(result.Item1, result.Item2);
            await _repo.SaveChanges();
            return View("ChangePassword"); 
        }

    }
}