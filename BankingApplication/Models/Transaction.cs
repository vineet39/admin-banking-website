using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models
{
    public class Transaction {
        public const char WithdrawTransaction = 'W';
        public const char DepositTransaction = 'D';
        public const char TransferTransaction = 'T';
        public const char BillPayTransaction = 'B';

        public const char ServiceChargeTransaction = 'S';

        public int TransactionID { get; set; }

        [Required]
        [Display (Name = "Transaction Type")]
        public char TransactionType { get; set; }

        [Required]
        [Display (Name = "Account Number")]
        public int AccountNumber { get; set; }
        public virtual Account Account { get; set; }

        [ForeignKey ("DestinationAccount")]
        [Display (Name = "Destination Account Number")]
        public int? DestinationAccountNumber { get; set; }
        public virtual Account DestinationAccount { get; set; }

        [Required]
        [Column (TypeName = "money")]
        [DataType (DataType.Currency)]
        public decimal Amount { get; set; }

        [StringLength (255)]
        public string Comment { get; set; }

        [StringLength (15)]
        [Display (Name = "Transaction Time")]
        public DateTime ModifyDate { get; set; }
    }
}