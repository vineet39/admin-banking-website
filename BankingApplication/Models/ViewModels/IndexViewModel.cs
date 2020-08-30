using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Models {
    //Viewmodel for presenting the atm page
    public class IndexViewModel {
        public Customer Customer { get; set; }

        [Required (ErrorMessage = "The amount is required")]
        [Range (1, double.MaxValue, ErrorMessage = "Please enter a valid amount")]
        [DataType (DataType.Currency)]
        public decimal Amount { get; set; }

        [Required (ErrorMessage = "The destination account number is required")]
        [RegularExpression ("[1-9]\\d{0}[0-9]\\d{2}",
            ErrorMessage = "Enter a valid 4 digit account number.")]
        public int DestinationAccountNumber { get; set; }

        [StringLength (255)]
        public string Comment { get; set; }
    }
}