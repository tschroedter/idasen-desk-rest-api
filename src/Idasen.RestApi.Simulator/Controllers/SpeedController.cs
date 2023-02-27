using Idasen.RestApi.Shared.Dtos ;
using Idasen.RestApi.Shared.Filters ;
using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.AspNetCore.Mvc ;

namespace Idasen.RestApi.Simulator.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/speed" ) ]
public class SpeedController : ControllerBase
{
    public SpeedController ( ILogger < DeskController > logger ,
                             IDeskManager               manager )
    {
        _logger  = logger ;
        _manager = manager ;
    }

    [ Route ( "" ) ]
    public IActionResult GetSpeed ( )
    {
        _logger.LogInformation ( "DeskController.GetSpeed()" ) ;

        if ( _manager.Desk == null )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        var dto = new SpeedDto
                  {
                      Speed = _manager.Desk.Speed
                  } ;

        return Ok ( dto ) ;
    }

    private readonly ILogger < DeskController > _logger ;
    private readonly IDeskManager               _manager ;
}