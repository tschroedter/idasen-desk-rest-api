using AutoMapper ;
using Idasen.RestApi.Shared.Dtos ;
using Idasen.RestApi.Shared.Filters ;
using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.AspNetCore.Mvc ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/" ) ]
public class DeskController : ControllerBase
{
    public DeskController ( ILogger < DeskController > logger ,
                            IMapper                    mapper ,
                            IDeskManager               manager )
    {
        _logger  = logger ;
        _mapper  = mapper ;
        _manager = manager ;
    }

    [ Route ( "" ) ]
    [ HttpGet ]
    public IActionResult GetDesk ( )
    {
        _logger.LogInformation ( "DeskController.GetDesk()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        var dto = _mapper.Map < DeskDto > ( _manager.Desk ) ;

        return Ok ( dto ) ;
    }


    private readonly ILogger < DeskController > _logger ;
    private readonly IDeskManager               _manager ;
    private readonly IMapper                    _mapper ;
}