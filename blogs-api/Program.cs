using Microsoft.EntityFrameworkCore;
using blogs_api.Models;
using blogs_api.domain;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);
// build app after configuring services
var app = builder.Build();
configureApp(app);

// Add services to the container.
void ConfigureServices(IServiceCollection services, ConfigurationManager configuration){
    services.AddControllers();
    services.AddDbContext<BlogsContext>(options => options.UseNpgsql(configuration.GetConnectionString("BlogsDbPostGreSQL")));
    /**
    services.AddDbContext<BlogsContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("BlogsDb")));
    **/    
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
}

void configureApp(WebApplication app){
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        /*
        In a development environment, this code retrieves an instance of the BlogsContext from the service collection,
        creates the database if it doesn’t exist, and seeds it if it is empty. It is also common to use EnsureDeleted 
        before EnsureCreated to start from an empty database each time.
        */
        using(var scope = app.Services.CreateScope())
        {
            var blogsContext = scope.ServiceProvider.GetRequiredService<BlogsContext>();
            blogsContext.Database.EnsureCreated();
            blogsContext.Seed();
        }
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
    