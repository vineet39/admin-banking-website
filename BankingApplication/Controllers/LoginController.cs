using System;
using System.Threading.Tasks;
using BankingApplication.Data;
using BankingApplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RepositoryWrapper;
using SimpleHashing;

namespace BankingApplication.Controllers
{
    public class LoginController : Controller
    {
        //Repository object
        private readonly Wrapper _repo;

        public LoginController(Wrapper repo)
        {
            _repo = repo;
        }
        public IActionResult Login()
        {
            return View();
        }

        //Login Form post logic referencing Web Dev Tutorial
        [HttpPost]
        public async Task<IActionResult> Login(string userID, string password)
        {
            //LINQ query for eager loading login
            var login = await _repo.Login.GetWithCustomer(userID);
            if (login == null)
            {
                ModelState.AddModelError("LoginFailed", "Incorrect userid or password");
                return View(new LoginViewModel { UserID = userID });
            }
            if (login.Locked == true)
            {
                ModelState.AddModelError("LoginFailed", "This account is locked.");
                return View(new LoginViewModel { UserID = userID });
            }
            var attempts = HttpContext.Session.GetInt32("LoginLock");
            if (!attempts.HasValue)
            {
                HttpContext.Session.SetInt32("LoginLock", 1);
                attempts = 1;
            }
            if (login == null || !PBKDF2.Verify(login.Password, password))
            {
                if(attempts == 3)
                {
                    login.Lock(DateTime.UtcNow.AddMinutes(1));
                    await _repo.SaveChanges();
                    ModelState.AddModelError("LoginFailed", "Allowed attempts exceeded. This account is now locked.");
                    return View(new LoginViewModel { UserID = userID });
                }
                var LoginLock = 3 - attempts;
                ModelState.AddModelError("LoginFailed", $"Login failed, please try again. {LoginLock} attempts left.");
                HttpContext.Session.SetInt32("LoginLock", (int)attempts + 1);
                return View(new LoginViewModel { UserID = userID });
            }

            // Set customer session variables
            HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
            HttpContext.Session.SetString(nameof(Customer.CustomerName), login.Customer.CustomerName);
            HttpContext.Session.SetString(nameof(BankingApplication.Models.Login.UserID), login.UserID);

            //Redirect to customer page
            return RedirectToAction("Index", "Bank");
        }

        public IActionResult Logout()
        {
            //Clear Session variables
            HttpContext.Session.Clear();

            //Return to home page
            return RedirectToAction("Index", "Home");

        }
    }
    
}