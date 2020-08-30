using System.Collections.Generic;
using System.Linq;

namespace BankingApplication.Models
{
    //ViewModel for presenting the BillSchedule page
    public class BillScheduleViewModel
    {
        public Account account;
        public List<Payee> payees;
        public List<NameBill> bills;

        //LINQ for getting a list of bills matching payees.
        //Made in order to obtain payee names
        public BillScheduleViewModel(Account a, List<Payee> p)
        {
            bills =
                (from bill in a.Bills
                 join payee in p
                 on bill.PayeeID equals payee.PayeeID
                 select new NameBill { PayeeName = payee.PayeeName, Bill = bill })
                 .ToList();
        }

        //Object for combining payees and matching bills
        public class NameBill
        {
            public string PayeeName { get; set; }
            public BillPay Bill { get; set; }
        }
    }
}
