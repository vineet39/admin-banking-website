using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using BankingApplication.Models;

namespace BankingApplication.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            //Seed Data example referenced from Web Dev Tutorials
            using var context = new BankAppContext(serviceProvider.GetRequiredService<DbContextOptions<BankAppContext>>());

            // Look for customers.
            if(context.Customer.Any())
                return; // DB has already been seeded.

            context.Customer.AddRange(
                new Customer
                {
                    CustomerID = 2100,
                    CustomerName = "Matthew Bolger",
                    Address = "123 Fake Street",
                    City = "Melbourne",
                    PostCode = "3000",
                    Phone = "123456789"
                },
                new Customer
                {
                    CustomerID = 2200,
                    CustomerName = "Rodney Cocker",
                    Address = "456 Real Road",
                    City = "Melbourne",
                    PostCode = "3005",
                    Phone = "123456789"
                },
                new Customer
                {
                    CustomerID = 2300,
                    CustomerName = "Shekhar Kalra",
                    Phone = "123456789"
                });

            context.Login.AddRange(
               new Login
               {
                   UserID = "12345678",
                   CustomerID = 2100,
                   Password = "YBNbEL4Lk8yMEWxiKkGBeoILHTU7WZ9n8jJSy8TNx0DAzNEFVsIVNRktiQV+I8d2",
                   ModifyDate = DateTime.UtcNow
               },
               new Login
               {
                   UserID = "38074569",
                   CustomerID = 2200,
                   Password = "EehwB3qMkWImf/fQPlhcka6pBMZBLlPWyiDW6NLkAh4ZFu2KNDQKONxElNsg7V04",
                   ModifyDate = DateTime.UtcNow
               },
               new Login
               {
                   UserID = "17963428",
                   CustomerID = 2300,
                   Password = "LuiVJWbY4A3y1SilhMU5P00K54cGEvClx5Y+xWHq7VpyIUe5fe7m+WeI0iwid7GE",
                   ModifyDate = DateTime.UtcNow
               });

            context.Account.AddRange(
                new Account
                {
                    AccountNumber = 4100,
                    AccountType = 'S',
                    CustomerID = 2100,
                    Balance = 100
                },
                new Account
                {
                    AccountNumber = 4101,
                    AccountType = 'C',
                    CustomerID = 2100,
                    Balance = 500
                },
                new Account
                {
                    AccountNumber = 4200,
                    AccountType = 'S',
                    CustomerID = 2200,
                    Balance = 500.95m
                },
                new Account
                {
                    AccountNumber = 4300,
                    AccountType = 'C',
                    CustomerID = 2300,
                    Balance = 1250.50m
                });

            const string openingBalance = "Opening balance";
            const string format = "dd/MM/yyyy hh:mm:ss tt";
            context.Transaction.AddRange(
                new Transaction
                {
                    TransactionType = 'D',
                    AccountNumber = 4100,
                    Amount = 100,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = 'D',
                    AccountNumber = 4101,
                    Amount = 500,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 08:30:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = 'D',
                    AccountNumber = 4200,
                    Amount = 500.95m,
                    Comment = openingBalance,
                    ModifyDate = DateTime.ParseExact("19/12/2019 09:00:00 PM", format, null)
                },
                new Transaction
                {
                    TransactionType = 'D',
                    AccountNumber = 4300,
                    Amount = 1250.50m,
                    Comment = "Opening balance",
                    ModifyDate = DateTime.ParseExact("19/12/2019 10:00:00 PM", format, null)
                });

            context.Payee.AddRange(
                new Payee
                {
                    PayeeID = 1111,
                    PayeeName = "RMIT",
                    Address = "123 Fake Street",
                    City = "Melbourne",
                    State = "VIC",
                    PostCode = "4444",
                    Phone = "123456789"
                },
                new Payee
                {
                    PayeeID = 2222,
                    PayeeName = "PayeeINC",
                    Address = "124 Fake Street",
                    City = "Sometown",
                    State = "VIC",
                    PostCode = "5555",
                    Phone = "123456788"
                },
                new Payee
                {
                    PayeeID = 3333,
                    PayeeName = "BankCorp",
                    Address = "142 Fake Street",
                    City = "Melbourne",
                    State = "VIC",
                    PostCode = "5554",
                    Phone = "123436788"
                },
                new Payee
                {
                    PayeeID = 4444,
                    PayeeName = "Telstra",
                    Address = "165 Fake Street",
                    City = "Melbourne",
                    State = "VIC",
                    PostCode = "5654",
                    Phone = "123436788"
                });

            context.SaveChanges();
        }
    }
}
