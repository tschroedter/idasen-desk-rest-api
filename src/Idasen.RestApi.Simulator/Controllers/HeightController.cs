using Idasen.RestApi.Shared ;
using Idasen.RestApi.Shared.Dtos ;
using Idasen.RestApi.Shared.Filters ;
using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.AspNetCore.Mvc ;

namespace Idasen.RestApi.Simulator.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/height/" ) ]
public class HeightController : ControllerBase
{
    public HeightController ( ILogger < DeskController > logger ,
                              IDeskManager               manager ,
                              ICommandFactory            command ,
                              IChannelWriter             writer )
    {
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;
        Guard.ArgumentNotNull ( manager ,
                                nameof ( manager ) ) ;
        Guard.ArgumentNotNull ( command ,
                                nameof ( command ) ) ;
        Guard.ArgumentNotNull ( writer ,
                                nameof ( writer ) ) ;

        _logger  = logger ;
        _manager = manager ;
        _command = command ;
        _writer  = writer ;
    }

    [ Route ( "" ) ]
    public IActionResult GetHeight ( )
    {
        _logger.LogInformation ( "DeskController.GetHeight()" ) ;

        if ( _manager.Desk == null )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        var dto = new HeightDto { Height = _manager.Desk.Height } ;

        return Ok ( dto ) ;
    }

    [ Route ( "" ) ]
    [ HttpPut ]
    [ HttpPost ]
    public async Task < IActionResult > SetHeight ( [ FromBody ] HeightDto dto ,
                                                    CancellationToken      cancellationToken )
    {
        _logger.LogInformation ( $"DeskController.SetHeight({dto})" ) ;

        if ( _manager.Desk == null )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        await _writer.WriteAsync ( _command.ToHeight ( dto.Height ) ,
                                   cancellationToken ) ;

        return Accepted ( _manager.Desk.Height ) ;
    }

    private readonly ICommandFactory _command ;

    private readonly ILogger < DeskController > _logger ;
    private readonly IDeskManager               _manager ;
    private readonly IChannelWriter             _writer ;
}