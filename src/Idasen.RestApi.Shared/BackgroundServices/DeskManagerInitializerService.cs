using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Hosting ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Shared.BackgroundServices ;

public class DeskManagerInitializerService : BackgroundService
{
    public DeskManagerInitializerService (
        ILogger < DeskManagerInitializerService > logger ,
        IDeskManager                              manager )
    {
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;
        Guard.ArgumentNotNull ( manager ,
                                nameof ( manager ) ) ;

        _logger  = logger ;
        _manager = manager ;
    }

    protected override async Task ExecuteAsync ( CancellationToken stoppingToken )
    {
        try
        {
            var success = false ;

            while ( ! stoppingToken.IsCancellationRequested &&
                    ! success )
            {
                _logger.LogInformation ( "Trying to initializing DeskManager..." ) ;

                success = await _manager.Initialise ( ) ;
            }

            _logger.LogInformation ( "...DeskManager initialized!" ) ;
        }
        catch ( Exception e )
        {
            _logger.LogError ( e ,
                               "Failed to initialize DeskManager." ) ;
        }
    }

    private readonly ILogger < DeskManagerInitializerService > _logger ;
    private readonly IDeskManager                              _manager ;
}