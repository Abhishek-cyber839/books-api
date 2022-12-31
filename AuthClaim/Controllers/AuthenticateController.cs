using Microsoft.AspNetCore.Mvc;
using AuthClaim.Models;
using AuthClaim.Authentication;
using AuthClaim.Roles;
using Microsoft.AspNetCore.Http;  
using Microsoft.AspNetCore.Identity;    
using Microsoft.Extensions.Configuration;  
using Microsoft.IdentityModel.Tokens;  
using System;  
using System.Collections.Generic;  
using System.IdentityModel.Tokens.Jwt;  
using System.Security.Claims;  
using System.Text;  
using System.Threading.Tasks;
using System.Linq; 

namespace AuthClaim.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController: ControllerBase {

        private readonly UserManager<ApplicationUser> _userManager;  
        private readonly RoleManager<IdentityRole> _roleManager;  
        private readonly IConfiguration _configuration;  
        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
        IConfiguration configuration) {
            this._userManager = userManager;  
            this._roleManager = roleManager;  
            this._configuration = configuration;  
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserModel userModel){
            var user = await _userManager.FindByEmailAsync(userModel.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, userModel.Password))  
            {  
                var userRoles = await _userManager.GetRolesAsync(user);  
                var authClaims = new List<Claim>  
                {  
                    new Claim(ClaimTypes.Email, user.Email),  
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
                };
                foreach (var userRole in userRoles)  
                {  
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));  
                }  
  
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));  
  
                var token = new JwtSecurityToken(  
                    issuer: _configuration["JWT:ValidIssuer"],  
                    audience: _configuration["JWT:ValidAudience"],  
                    expires: DateTime.Now.AddHours(3),  
                    claims: authClaims,  
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)  
                    );  
  
                return Ok(new  
                {  
                    token = new JwtSecurityTokenHandler().WriteToken(token),  
                    expiration = token.ValidTo  
                });  
            }  
            return Unauthorized();  
        }

        [HttpPost("register")]    
        public async Task<IActionResult> Register([FromBody] RegisterUserModel userModel, [FromQuery] bool registerAsAnAdmin = false)  
        {  
            var userExists = await _userManager.FindByEmailAsync(userModel.Email);
            if (userExists != null)  
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { status = "Error", message = "User already exists!" });  
  
            ApplicationUser user = new ApplicationUser()  
            {  
                Email = userModel.Email,  
                SecurityStamp = Guid.NewGuid().ToString(),  
                firstName = userModel.firstName,
                lastName = userModel.lastName,
                UserName = userModel.firstName + userModel.Email.Split("@")[0]  
            };  
            var result = await _userManager.CreateAsync(user, userModel.Password);  
            if (!result.Succeeded)  
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel 
                                                                      { status = "Error",
                                                                       message = "User creation failed! Please check user details and try again." });   
            
            if(registerAsAnAdmin) {
                if (!await _roleManager.RoleExistsAsync(UserRoles.admin))  
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.admin));  
                if (!await _roleManager.RoleExistsAsync(UserRoles.Student))  
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Student));  
    
                if (await _roleManager.RoleExistsAsync(UserRoles.admin))  
                {  
                    await _userManager.AddToRoleAsync(user, UserRoles.admin);  
                }  
            }
            return Ok(new ResponseModel { status = "Success", message = "User created successfully!" });  
        }  

    }
}