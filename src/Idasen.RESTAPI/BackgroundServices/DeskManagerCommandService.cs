using System ;
using System.Threading ;
using System.Threading.Tasks ;
using Idasen.BluetoothLE.Core ;
using Idasen.RestApi.Interfaces ;
using JetBrains.Annotations ;
using Microsoft.Extensions.Hosting ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.BackgroundServices ;

public class DeskManagerCommandService : BackgroundService
{
    public DeskManagerCommandService (
        [ NotNull ] ILogger < DeskManagerInitializerService > logger ,
        [ NotNull ] IChannelReader                            reader )
    {
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;
        Guard.ArgumentNotNull ( reader ,
                                nameof ( reader ) ) ;

        _logger = logger ;
        _reader = reader ;
    }

    protected override Task ExecuteAsync ( CancellationToken cancellationToken )
    {
        return BeginConsumeAsync ( cancellationToken ) ;
    }

    public async Task BeginConsumeAsync ( CancellationToken cancellationToken = default )
    {
        _logger.LogInformation ( "Consumer starting..." ) ;

        try
        {
            await foreach ( var message in _reader.ReadAllAsync ( cancellationToken ) )
            {
                _logger.LogInformation ( $"Consumer Received: {message.CommandName}" ) ;

                await ExecuteCommand ( message ) ;
            }
        }
        catch ( OperationCanceledException )
        {
            _logger.LogInformation ( "Consumer forced stop" ) ;
        }

        _logger.LogInformation ( "Consumer shutting down" ) ;
    }

    private async Task ExecuteCommand ( ICommand message )
    {
        var status = await message.Execute ( ) ;

        if ( status )
            _logger.LogInformation ( $"Consumed command: {message.CommandName}" ) ;
        else
            _logger.LogError ( $"Failed to consume command: {message.CommandName}" ) ;
    }

    [ NotNull ] private readonly ILogger < DeskManagerInitializerService > _logger ;
    [ NotNull ] private readonly IChannelReader                            _reader ;
}