using AuthClaim.Domain;
using Microsoft.EntityFrameworkCore;
using AuthClaim.Authentication;  
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;  
using Microsoft.IdentityModel.Tokens;  
using System.Text;  

var builder = WebApplication.CreateBuilder(args);

void ConfigureServices(IServiceCollection services, ConfigurationManager configuration) {
    services.AddControllers();
    services.AddDbContext<ApplicationDbContext>(options => 
        options.UseSqlServer(configuration.GetConnectionString("ConnStr"))); 
    // For Identity  
    services.AddIdentity<ApplicationUser, IdentityRole>()  
            .AddEntityFrameworkStores<ApplicationDbContext>()  
            .AddDefaultTokenProviders();  
    services.AddAuthentication(options =>  
            {  
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;  
            })
            .AddJwtBearer(options =>  
            {  
                options.SaveToken = true;  
                options.RequireHttpsMetadata = false;  
                options.TokenValidationParameters = new TokenValidationParameters()  
                {  
                    ValidateIssuer = true,  
                    ValidateAudience = true,  
                    ValidAudience = configuration["JWT:ValidAudience"],  
                    ValidIssuer = configuration["JWT:ValidIssuer"],  
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))  
                };  
            });      
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

void ConfigureApp(WebApplication app){
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}

ConfigureServices(builder.Services, builder.Configuration);
ConfigureApp(builder.Build());



