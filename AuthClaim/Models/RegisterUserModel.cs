using System.ComponentModel.DataAnnotations;

namespace AuthClaim.Models {
    public class RegisterUserModel {
        [Required(ErrorMessage = "Username should not be empty")]
        [MaxLength(10)]
        [MinLength(5)]
        public string firstName {get; set;}

        [Required(ErrorMessage = "Username should not be empty")]
        [MaxLength(10)]
        [MinLength(5)]
        public string lastName {get; set;}

        
        [EmailAddress]  
        [Required(ErrorMessage = "Email is required")]  
        public string Email { get; set; }  
  
        [Required(ErrorMessage = "Password is required")]  
        public string Password { get; set; }  
    }
}