using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BankingApplication.Models {
    //Account object contains business methods for atm transactions and paying bills
    public class Account {
        public const decimal WithdrawServiceCharge = 0.1M;
        public const decimal TransferServiceCharge = 0.2M;
        private const decimal minBalSavings = 0;
        private const decimal minBalCheckings = 200;

        [Key, DatabaseGenerated (DatabaseGeneratedOption.None)]
        public int AccountNumber { get; set; }

        [Required]
        public char AccountType { get; set; }

        [Required]
        public int CustomerID { get; set; }
        [JsonIgnore]
        public virtual Customer Customer { get; set; }

        [Column (TypeName = "money")]
        [DataType (DataType.Currency)]
        [Required]
        public decimal Balance { get; set; }

        [Required]
        [StringLength (8)]
        public DateTime ModifyDate { get; set; }
        [JsonIgnore]
        public List<Transaction> Transactions { get; set; }
        [JsonIgnore]
        public List<BillPay> Bills { get; set; }

        public void GenerateTransaction (char type, decimal amount, int destinationAccountNumber = 0, string comment = null) {
            Transaction transaction = new Transaction () {
            AccountNumber = this.AccountNumber,
            TransactionType = type,
            Amount = amount,
            ModifyDate = DateTime.UtcNow,
            Comment = comment
            };

            if (destinationAccountNumber == 0) {
                transaction.DestinationAccountNumber = null;
            } else {
                transaction.DestinationAccountNumber = destinationAccountNumber;
            }

            this.Transactions.Add (transaction);
        }

        // Check if after withdrawing min balance of 0 remains in savings and 200 in checkings.
        // Withdrawal amount includes service charges for withdraw and transfer.
        public bool CheckIfFundsSufficient (decimal amount, decimal transactionCharge) {
            var filteredList = Transactions.Where (t => t.TransactionType != Transaction.ServiceChargeTransaction);

            if (filteredList.Count () >= 5) {
                if ((Balance - amount - transactionCharge < minBalSavings && AccountType == 'S') ||
                    (Balance - amount - transactionCharge < minBalCheckings && AccountType == 'C')) {
                    return false;
                }
            } else {
                if ((Balance - amount < minBalSavings && AccountType == 'S') ||
                    (Balance - amount < minBalCheckings && AccountType == 'C')) {
                    return false;
                }
            }

            return true;
        }

        public bool Withdraw (decimal amount) {
            bool haveSufficientFunds = CheckIfFundsSufficient (amount, WithdrawServiceCharge);

            // Abort if funds are less.
            if (!haveSufficientFunds) {
                return false;
            }

            // Filtered list of transactions excluding service charges.
            var filteredList = Transactions.Where (t => t.TransactionType != Transaction.ServiceChargeTransaction);

            Balance -= amount;

            // If number of transactions greater than 5, charge user.
            if (filteredList.Count () >= 5) {
                Balance -= WithdrawServiceCharge;
                GenerateTransaction (Transaction.ServiceChargeTransaction, WithdrawServiceCharge);
            }

            GenerateTransaction (Transaction.WithdrawTransaction, amount);

            return true;
        }

        public void Deposit (decimal amount) {
            Balance += amount;

            GenerateTransaction (Transaction.DepositTransaction, amount);
        }

        public bool Transfer (decimal amount, Account receiverAccount, string comment = null) {
            bool haveSufficientFunds = CheckIfFundsSufficient (amount, TransferServiceCharge);
            
            // Abort if funds are less.
            if (!haveSufficientFunds) {
                return false;
            }
            // If number of transactions greater than 5, charge user.
            var filteredList = Transactions.Where (t => t.TransactionType != Transaction.ServiceChargeTransaction);

            // Updating sender account balance.
            Balance -= amount;
           
            // If number of transactions greater than 5, charge user.
            if (filteredList.Count () >= 5) {
                Balance -= TransferServiceCharge;
                GenerateTransaction (Transaction.ServiceChargeTransaction, TransferServiceCharge);
            }

            // Generate a trasanction indicating money withdrawed from sender account.
            GenerateTransaction (Transaction.TransferTransaction, amount, receiverAccount.AccountNumber, comment);

            // Updating receiver account balance.
            receiverAccount.Balance = receiverAccount.Balance + amount;

            // Generate a transaction indicating money deposited into receiver account.
            receiverAccount.GenerateTransaction (Transaction.TransferTransaction, amount, 0, comment);

            return true;
        }

        public bool PayBill(BillPay bill)
        {
            if(!CheckIfFundsSufficient(bill.Amount, 0))
            {
                return false;
            }

            Balance -= bill.Amount;
            GenerateTransaction(Transaction.BillPayTransaction, bill.Amount, 0);
            if(bill.Period != BillPay.Periods.OnceOff)
            {
                switch (bill.Period)
                {
                    case BillPay.Periods.Monthly:
                        bill.ScheduleDate = bill.ScheduleDate.AddMonths(1);
                        bill.ModifyDate = DateTime.UtcNow;
                        break;
                    case BillPay.Periods.Quarterly:
                        bill.ScheduleDate = bill.ScheduleDate.AddMonths(3);
                        bill.ModifyDate = DateTime.UtcNow;
                        break;
                    case BillPay.Periods.Annually:
                        bill.ScheduleDate = bill.ScheduleDate.AddYears(1);
                        bill.ModifyDate = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
            return true;

        }
    }
}