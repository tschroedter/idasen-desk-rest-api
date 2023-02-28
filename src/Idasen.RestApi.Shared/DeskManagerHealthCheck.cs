using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Diagnostics.HealthChecks ;

namespace Idasen.RestApi.Shared ;

public class DeskManagerHealthCheck : IHealthCheck
{
    public DeskManagerHealthCheck ( IDeskManager manager )
    {
        Guard.ArgumentNotNull ( manager ,
                                nameof ( manager ) ) ;

        _manager = manager ;
    }

    public Task < HealthCheckResult > CheckHealthAsync ( HealthCheckContext context ,
                                                         CancellationToken  cancellationToken = default )
    {
        try
        {
            return _manager.IsReady
                       ? Task.FromResult ( HealthCheckResult.Healthy ( ) )
                       : Task.FromResult ( HealthCheckResult.Unhealthy ( ) ) ;
        }
        catch ( Exception ex )
        {
            return Task.FromResult ( HealthCheckResult.Unhealthy ( ex.Message ) ) ;
        }
    }

    private readonly IDeskManager _manager ;
}