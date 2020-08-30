using Newtonsoft.Json;
using SimpleHashing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models {
    //Login object contain business methods for changing passwords
    public class Login {
        [Key, DatabaseGenerated (DatabaseGeneratedOption.None), Required, StringLength (50)]
        public string UserID { get; set; }

        [Required]
        public int CustomerID { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }

        [Required, StringLength (64)]
        public string Password { get; set; }

        [Required, StringLength (15)]
        public DateTime ModifyDate { get; set; }

        [StringLength(15)]
        public DateTime LockoutTime { get; set; }

        public bool Locked { get; set; } = false;

        public (string, string) ChangePassword(string oldpassword , string newpassword, string confirmnewpassword )
        {
            if (!PBKDF2.Verify(Password, oldpassword))
            {
                return("PasswordChangeFailed", "Old password entered is incorrect.");
            }
            if (oldpassword == newpassword)
            {
                return("PasswordChangeFailed", "Old password and new password cannot be same.");
            }

            if (newpassword != confirmnewpassword)
            {
                return("PasswordChangeFailed", "New password and confirmed new password do not match");
            }

            Password = PBKDF2.Hash(newpassword);
            ModifyDate = DateTime.UtcNow;
            return ("PasswordChangeSuccess", "Password changed successfully.");
        }

        public void Lock(DateTime lockoutTime)
        {
            Locked = true;
            LockoutTime = lockoutTime;
        }

        public void UnLock()
        {
            Locked = false;
        }


    }
}