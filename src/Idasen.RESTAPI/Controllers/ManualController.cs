using System.Threading ;
using System.Threading.Tasks ;
using Idasen.BluetoothLE.Core ;
using Idasen.RESTAPI.Filters ;
using Idasen.RestApi.Interfaces ;
using Idasen.RestApi.Shared.Interfaces ;
using JetBrains.Annotations ;
using Microsoft.AspNetCore.Mvc ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RESTAPI.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/" ) ]
public class ManualController : ControllerBase
{
    public ManualController ( [ NotNull ] ILogger < DeskController > logger ,
                              [ NotNull ] IDeskManager               manager ,
                              [ NotNull ] ICommandFactory            command ,
                              [ NotNull ] IChannelWriter             writer )
    {
        Guard.ArgumentNotNull ( manager ,
                                nameof ( manager ) ) ;
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;
        Guard.ArgumentNotNull ( command ,
                                nameof ( command ) ) ;
        Guard.ArgumentNotNull ( writer ,
                                nameof ( writer ) ) ;

        _logger  = logger ;
        _manager = manager ;
        _command = command ;
        _writer  = writer ;
    }

    [ Route ( "up" ) ]
    [ HttpPut ]
    [ HttpPost ]
    public async Task < IActionResult > Up ( CancellationToken cancellationToken )
    {
        _logger.LogInformation ( "DeskController.Up()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        await _writer.WriteAsync ( _command.Up ( ) ,
                                   cancellationToken ) ;

        return Ok ( ) ;
    }

    [ Route ( "down" ) ]
    [ HttpPut ]
    [ HttpPost ]
    public async Task < IActionResult > Down ( CancellationToken cancellationToken )
    {
        _logger.LogInformation ( "DeskController.Down()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        await _writer.WriteAsync ( _command.Down ( ) ,
                                   cancellationToken ) ;

        return Ok ( ) ;
    }

    [ Route ( "stop" ) ]
    [ HttpPut ]
    [ HttpPost ]
    public async Task < IActionResult > Stop ( CancellationToken cancellationToken )
    {
        _logger.LogInformation ( "DeskController.Stop()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        await _manager.Desk
                      .MoveStopAsync ( ) ;

        return Ok ( ) ;
    }

    private readonly ICommandFactory            _command ;
    private readonly ILogger < DeskController > _logger ;
    private readonly IDeskManager               _manager ;
    private readonly IChannelWriter             _writer ;
}