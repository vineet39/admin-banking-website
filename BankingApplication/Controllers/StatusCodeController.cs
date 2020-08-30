using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BankingApplication.Controllers
{
    //Custom error page setup referencing
    //https://gooroo.io/GoorooTHINK/Article/17086/Creating-Custom-Error-Pages-in-ASPNET-core-10/32407
    public class StatusCodeController : Controller
    {
        [HttpGet("/StatusCode/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            return View(statusCode);
        }
    }
}