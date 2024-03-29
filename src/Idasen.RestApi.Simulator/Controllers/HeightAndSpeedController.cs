﻿using Idasen.RestApi.Shared.Dtos ;
using Idasen.RestApi.Shared.Filters ;
using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.AspNetCore.Mvc ;

namespace Idasen.RestApi.Simulator.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/heightandspeed" ) ]
public class HeightAndSpeedController : ControllerBase
{
    public HeightAndSpeedController ( ILogger < DeskController > logger ,
                                      IDeskManager               manager )
    {
        _logger  = logger ;
        _manager = manager ;
    }

    [ Route ( "" ) ]
    public IActionResult GetHeightAndSpeed ( )
    {
        _logger.LogInformation ( "DeskController.GetHeightAndSpeed()" ) ;

        if ( _manager.Desk == null )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        var dto = new HeightAndSpeedDto
                  {
                      Height = _manager.Desk.Height ,
                      Speed  = _manager.Desk.Speed
                  } ;

        return Ok ( dto ) ;
    }

    private readonly ILogger < DeskController > _logger ;
    private readonly IDeskManager               _manager ;
}