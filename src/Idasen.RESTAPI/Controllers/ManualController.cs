using System.Threading.Tasks ;
using Idasen.RESTAPI.Filters ;
using Idasen.RESTAPI.Interfaces ;
using Microsoft.AspNetCore.Mvc ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RESTAPI.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/" ) ]
public class ManualController : ControllerBase
{
    public ManualController ( ILogger < DeskController > logger ,
                              IDeskManager               manager )
    {
        _logger  = logger ;
        _manager = manager ;
    }

    [ Route ( "up" ) ]
    [ HttpPut ]
    [ HttpPost ]
    public async Task < IActionResult > Up ( )
    {
        _logger.LogInformation ( "DeskController.Up()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        await _manager.Desk.MoveUpAsync ( )
                      .ConfigureAwait ( false ) ;

        return Ok ( ) ;
    }

    [ Route ( "down" ) ]
    [ HttpPut ]
    [ HttpPost ]
    public async Task < IActionResult > Down ( )
    {
        _logger.LogInformation ( "DeskController.Down()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        await _manager.Desk.MoveDownAsync ( )
                      .ConfigureAwait ( false ) ;

        return Ok ( ) ;
    }

    [ Route ( "stop" ) ]
    [ HttpPut ]
    [ HttpPost ]
    public async Task < IActionResult > Stop ( )
    {
        _logger.LogInformation ( "DeskController.Stop()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        await _manager.Desk
                      .MoveStopAsync ( )
                      .ConfigureAwait ( false ) ;

        return Ok ( ) ;
    }

    private readonly ILogger < DeskController > _logger ;
    private readonly IDeskManager               _manager ;
}