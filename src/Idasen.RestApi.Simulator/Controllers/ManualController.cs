using Idasen.RestApi.Shared ;
using Idasen.RestApi.Shared.Filters ;
using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.AspNetCore.Mvc ;

namespace Idasen.RestApi.Simulator.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/" ) ]
public class ManualController : ControllerBase
{
    public ManualController ( ILogger < DeskController > logger ,
                              IDeskManager               manager ,
                              ICommandFactory            command ,
                              IChannelWriter          writer )
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

        if ( _manager.Desk == null )
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