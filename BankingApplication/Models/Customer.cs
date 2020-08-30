using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models
{
    public class Customer {
        [DatabaseGenerated (DatabaseGeneratedOption.None)]
        public int CustomerID { get; set; }

        [Required, StringLength (50)]
        [Display (Name = "Name")]
        public string CustomerName { get; set; }

        [StringLength (11)]
        [RegularExpression ("[0-9]\\d{10}",
            ErrorMessage = "Enter a 11 digit TFN.")]
        public string TFN { get; set; }

        [StringLength (50)]
        public string Address { get; set; }

        [StringLength (40)]
        [RegularExpression ("^[A-Z][a-z]+$",
            ErrorMessage = "Enter a valid city name.")]
        public string City { get; set; }

        [StringLength (20)]
        [RegularExpression ("[A-Z]{3}",
            ErrorMessage = "Enter your state eg: VIC.")]
        public string State { get; set; }

        [StringLength (10)]
        [RegularExpression ("[1-9]\\d{3}",
            ErrorMessage = "Enter a valid 4 digit postcode.")]
        public string PostCode { get; set; }

        [Required, StringLength (15)]
        [RegularExpression ("^[(]61[)][\\s][-][\\s][1-9]\\d{7}$",
            ErrorMessage = "Phone number should be formatted like (61) - XXXXXXXX")]
        public string Phone { get; set; }

        public List<Account> Accounts { get; set; }

        public Login Login { get; set; }
    }
}