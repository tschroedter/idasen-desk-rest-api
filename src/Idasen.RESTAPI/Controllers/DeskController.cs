using System.Threading.Tasks ;
using AutoMapper ;
using Idasen.RESTAPI.Dtos ;
using Idasen.RESTAPI.Filters ;
using Idasen.RESTAPI.Interfaces ;
using Microsoft.AspNetCore.Mvc ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RESTAPI.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/" ) ]
public class DeskController : ControllerBase
{
    public DeskController ( ILogger < DeskController > logger ,
                            IMapper                    mapper ,
                            IDeskManager               manager ,
                            ISettingsRepository        repository )
    {
        _logger     = logger ;
        _mapper     = mapper ;
        _manager    = manager ;
        _repository = repository ;
    }

    [ Route ( "" ) ]
    public IActionResult GetDesk ( )
    {
        _logger.LogInformation ( "DeskController.GetDesk()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        var dto = _mapper.Map < DeskDto > ( _manager.Desk ) ;

        return Ok ( dto ) ;
    }

    [ Route ( "height" ) ]
    public IActionResult GetHeight ( )
    {
        _logger.LogInformation ( "DeskController.GetHeight()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        var dto = new HeightDto { Height = _manager.Desk.Height } ;

        return Ok ( dto ) ;
    }

    [ Route ( "height" ) ]
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

    [ Route ( "speed" ) ]
    public IActionResult GetSpeed ( )
    {
        _logger.LogInformation ( "DeskController.GetSpeed()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        var dto = new SpeedDto
                  {
                      Speed = _manager.Desk.Speed
                  } ;

        return Ok ( dto ) ;
    }

    [ Route ( "heightandspeed" ) ]
    public IActionResult GetHeightAndSpeed ( )
    {
        _logger.LogInformation ( "DeskController.GetHeightAndSpeed()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        var dto = new HeightAndSpeedDto
                  {
                      Height = _manager.Desk.Height ,
                      Speed  = _manager.Desk.Speed
                  } ;

        return Ok ( dto ) ;
    }

    [ Route ( "up" ) ]
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

    [ Route ( "settings" ) ]
    [ HttpGet ]
    public async Task < IActionResult > SettingsGetAll ( )
    {
        _logger.LogInformation ( "DeskController.SettingsGetAll()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        return await DoSettingsGetAll ( ) ;
    }

    [ Route ( "settings" ) ]
    [ HttpPut ]
    [ HttpPost ]
    public async Task < IActionResult > SettingsPost ( [ FromBody ] SettingsDto dto )
    {
        _logger.LogInformation ( $"DeskController.SettingsPost({dto.Id}, {dto})" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        return await DoSettingsPost ( dto.Id ,
                                      dto ) ;
    }

    [ Route ( "settings/{id}" ) ]
    [ HttpGet ]
    public async Task < IActionResult > SettingsGetById ( [ FromQuery ] string id )
    {
        _logger.LogInformation ( $"DeskController.SettingsGetById({id})" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        return await DoSettingsGetById ( id ) ;
    }

    [ Route ( "settings/{id}" ) ]
    [ HttpPut ]
    public async Task < IActionResult > SettingsPost ( string                   id ,
                                                       [ FromBody ] SettingsDto dto )
    {
        _logger.LogInformation ( $"DeskController.SettingsPost({id}, {dto})" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        return await DoSettingsPost ( id ,
                                      dto ) ;
    }

    private async Task < IActionResult > DoSettingsPost ( string      id ,
                                                          SettingsDto dto )
    {
        if ( id != dto.Id )
            return BadRequest ( $"Failed Ids must match ('{id}' != '{dto.Id}')" ) ;

        var (status , settings) = await _repository.AddOrUpdate ( dto )
                                                   .ConfigureAwait ( false ) ;

        if ( ! status ||
             settings == null )
            return StatusCode ( 500 ,
                                $"Failed to store settings {dto}" ) ;

        return Ok ( ) ;
    }

    private async Task < IActionResult > DoSettingsGetAll ( )
    {
        var (status , settings) = await _repository.GetAll ( )
                                                   .ConfigureAwait ( false ) ;

        if ( status )
            return Ok ( settings ) ;

        return BadRequest ( "Failed to get all settings." ) ;
    }

    private async Task < IActionResult > DoSettingsGetById ( string id )
    {
        var (status , settings) = await _repository.GetById ( id )
                                                   .ConfigureAwait ( false ) ;

        if ( status )
            return Ok ( settings ) ;

        _logger.LogInformation ( $"Failed to get settings for id '{id}'. " +
                                 "Trying to get default settings." ) ;

        ( status , settings ) = await _repository.GetDefault ( id )
                                                 .ConfigureAwait ( false ) ;

        if ( status )
            return Ok ( settings ) ;

        return BadRequest ( $"Failed to get settings for id '{id}'" ) ;
    }

    private readonly ILogger < DeskController > _logger ;
    private readonly IDeskManager               _manager ;
    private readonly IMapper                    _mapper ;
    private readonly ISettingsRepository        _repository ;
}