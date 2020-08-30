using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Models {
    //Viewmodel for presenting the login page
    public class LoginViewModel {
        [Required]
        public string UserID { get; set; }

        [Required]
        public string Password { get; set; }
    }
}