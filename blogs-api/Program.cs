using Microsoft.EntityFrameworkCore;
using blogs_api.domain;
using blogs_api.services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services, builder.Configuration);
// build app after configuring services
var app = builder.Build();
configureApp(app);

// Add services to the container.
void ConfigureServices(IServiceCollection services, ConfigurationManager configuration){
    services.AddHealthChecks();
    // Call AddTypeActivatedCheck to pass arguments to a health check implementation. 
    services.AddHealthChecks().AddTypeActivatedCheck<DatabaseCheck>("database-check",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "database" },
        args: new object[] { 1, "Arg" });
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

 //   https://aws.amazon.com/blogs/developer/developing-a-microsoft-net-core-web-api-application-with-aurora-database-using-aws-cdk/
//    https://www.youtube.com/watch?v=mAdYsD25GQg
//    https://www.telerik.com/blogs/health-checks-in-aspnet-core
    app.MapHealthChecks("/healthz", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("database"),
            ResultStatusCodes =
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json; charset=utf-8";
                var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(report));
                await context.Response.Body.WriteAsync(bytes);
            }
        })
       .RequireHost("*:5001");

    app.MapControllers();

    app.Run();
}
    