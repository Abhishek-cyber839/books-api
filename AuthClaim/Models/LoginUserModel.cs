using System.ComponentModel.DataAnnotations;

namespace AuthClaim.Models {
    public class LoginUserModel {
        [EmailAddress]  
        [Required(ErrorMessage = "Email is required")]  
        public string Email { get; set; }  
  
        [Required(ErrorMessage = "Password is required")]  
        public string Password { get; set; }  
    }
}