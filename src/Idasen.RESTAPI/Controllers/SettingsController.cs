using System.Threading.Tasks ;
using Idasen.RESTAPI.Dtos ;
using Idasen.RESTAPI.Filters ;
using Idasen.RESTAPI.Interfaces ;
using Microsoft.AspNetCore.Mvc ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RESTAPI.Controllers ;

[ ApiKeyAuth ]
[ Route ( "desk/settings/" ) ]
public class SettingsController : ControllerBase
{
    public SettingsController ( ILogger < DeskController > logger ,
                                IDeskManager               manager ,
                                ISettingsRepository        repository )
    {
        _logger     = logger ;
        _manager    = manager ;
        _repository = repository ;
    }

    [ Route ( "" ) ]
    [ HttpGet ]
    public async Task < IActionResult > SettingsGetAll ( )
    {
        _logger.LogInformation ( "DeskController.SettingsGetAll()" ) ;

        if ( ! _manager.IsReady )
            return StatusCode ( 500 ,
                                "DeskManger isn't ready" ) ;

        return await DoSettingsGetAll ( ) ;
    }

    [ Route ( "" ) ]
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

        var (status , settings) = await _repository.AddOrUpdate ( dto ) ;

        if ( ! status ||
             settings == null )
            return StatusCode ( 500 ,
                                $"Failed to store settings {dto}" ) ;

        return Ok ( ) ;
    }

    private async Task < IActionResult > DoSettingsGetAll ( )
    {
        var (status , settings) = await _repository.GetAll ( ) ;

        if ( status )
            return Ok ( settings ) ;

        return BadRequest ( "Failed to get all settings." ) ;
    }

    private async Task < IActionResult > DoSettingsGetById ( string id )
    {
        var (status , settings) = await _repository.GetById ( id ) ;

        if ( status )
            return Ok ( settings ) ;

        _logger.LogInformation ( $"Failed to get settings for id '{id}'. " +
                                 "Trying to get default settings." ) ;

        ( status , settings ) = await _repository.GetDefault ( id ) ;

        if ( status )
            return Ok ( settings ) ;

        return BadRequest ( $"Failed to get settings for id '{id}'" ) ;
    }

    private readonly ILogger < DeskController > _logger ;
    private readonly IDeskManager               _manager ;
    private readonly ISettingsRepository        _repository ;
}