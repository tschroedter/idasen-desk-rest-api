using System;
using System.Threading;
using System.Threading.Tasks;
using Idasen.BluetoothLE.Core;
using Idasen.RESTAPI.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Idasen.RestApi.BackgroundServices;

public class DeskManagerInitializerService : BackgroundService
{
    private readonly ILogger<DeskManagerInitializerService> _logger;
    private readonly IDeskManager                           _manager;

    public DeskManagerInitializerService (
        [ NotNull ] ILogger < DeskManagerInitializerService > logger ,
        [ NotNull ] IDeskManager                              manager )
    {
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;
        Guard.ArgumentNotNull ( manager ,
                                nameof ( manager ) ) ;

        _logger  = logger ;
        _manager = manager ;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var success = false;

            while (!cancellationToken.IsCancellationRequested &&
                    !success)
            {
                _logger.LogInformation("Trying to initializing DeskManager...");

                success = await _manager.Initialise();
            }

            _logger.LogInformation("...DeskManager initialized!");
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                               "Failed to initialize DeskManager.");
        }
    }
}