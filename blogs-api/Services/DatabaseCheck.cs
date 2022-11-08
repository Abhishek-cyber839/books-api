using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace blogs_api.services
{
   
    public class DatabaseCheck: IHealthCheck {

    private readonly int _arg1;
    private readonly string _arg2;

    public DatabaseCheck(int arg1, string arg2)
        => (_arg1, _arg2) = (arg1, arg2);     
        
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var isHealthy = true;

        // ...

        if (isHealthy)
        {
            return Task.FromResult(
                HealthCheckResult.Healthy("A healthy result."));
        }

        return Task.FromResult(
            new HealthCheckResult(
                context.Registration.FailureStatus, "An unhealthy result."));
    }

}
}
