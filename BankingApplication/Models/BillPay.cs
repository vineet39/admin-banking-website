using BankingApplication.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models {
    public class BillPay {
        public int BillPayID { get; set; }

        [Required]
        [ForeignKey ("FKAccountNumber")]
        public int AccountNumber { get; set; }
        public Account FKAccountNumber { get; set; }

        [Required]
        [ForeignKey ("FKPayeeID")]
        public int PayeeID { get; set; }
        public Payee FKPayeeID { get; set; }

        [Required]
        [Column (TypeName = "money")]
        [DataType (DataType.Currency)]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a valid amount")]
        public decimal Amount { get; set; }

        [Required]
        [ValidDate(ErrorMessage = "Date must be greater than or equal to today")]
        [DataType(DataType.DateTime)]
        public DateTime ScheduleDate { get; set; } = DateTime.UtcNow;

        public enum Periods {
            [Display (Name = "Once Off")]
            OnceOff = 'S',
            Monthly = 'M',
            Quarterly = 'Q',
            Annually = 'Y'
        }

        [Required]
        public Periods Period { get; set; }

        [Required]
        public DateTime ModifyDate { get; set; }

        public bool Locked { get; set; } = false;

        //Method for updating bills from a billpayviewmodel object
        public void UpdateBill(BillPay bill)
        {
            AccountNumber = bill.FKAccountNumber.AccountNumber;
            FKAccountNumber = bill.FKAccountNumber;
            PayeeID = bill.FKPayeeID.PayeeID;
            FKPayeeID = bill.FKPayeeID;
            Amount = bill.Amount;
            ScheduleDate = bill.ScheduleDate;
            Period = bill.Period;
            ModifyDate = DateTime.UtcNow;
        }

        public void Lock()
        {
            Locked = true;
        }

        public void UnLock()
        {
            Locked = false;
        }
    }
}