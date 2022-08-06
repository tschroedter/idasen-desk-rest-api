using System.Threading.Tasks ;
using Idasen.RESTAPI.Dtos ;
using Idasen.RESTAPI.Filters ;
using Idasen.RESTAPI.Interfaces ;
using Microsoft.AspNetCore.Mvc ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RESTAPI.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/height/" ) ]
public class HeightController : ControllerBase
{
    public HeightController ( ILogger < DeskController > logger ,
                              IDeskManager               manager )
    {
        _logger  = logger ;
        _manager = manager ;
    }

    [ Route ( "" ) ]
    public IActionResult GetHeight ( )
    {
        _logger.LogInformation ( "DeskController.GetHeight()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        var dto = new HeightDto { Height = _manager.Desk.Height } ;

        return Ok ( dto ) ;
    }

    [ Route ( "" ) ]
    [ HttpPut ]
    [ HttpPost ]
    public async Task < IActionResult > SetHeight ( [ FromBody ] HeightDto dto )
    {
        _logger.LogInformation ( $"DeskController.SetHeight({dto})" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        await _manager.Desk.MoveToAsync ( dto.Height )
                      .ConfigureAwait ( false ) ;

        return Ok ( _manager.Desk.Height ) ;
    }

    private readonly ILogger < DeskController > _logger ;
    private readonly IDeskManager               _manager ;
}