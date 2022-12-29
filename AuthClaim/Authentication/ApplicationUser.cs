using Microsoft.AspNetCore.Identity;

namespace AuthClaim.Authentication {
    public class ApplicationUser : IdentityUser {
        public string firstName {get; set;}
        public string lastName {get; set;}
    }
}