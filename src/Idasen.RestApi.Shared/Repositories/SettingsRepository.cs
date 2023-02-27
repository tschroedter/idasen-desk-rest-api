using Idasen.RestApi.Shared.Dtos ;
using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Shared.Repositories ;

public class SettingsRepository
    : ISettingsRepository
{
    public SettingsRepository ( ILogger < SettingsRepository > logger ,
                                ISettingsStorage            storage )
    {
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;
        Guard.ArgumentNotNull ( storage ,
                                nameof ( storage ) ) ;

        _logger  = logger ;
        _storage = storage ;
    }

    public Task < (bool , IEnumerable < SettingsDto >) > GetAll ( )
    {
        _logger.LogInformation ( "Get all settings" ) ;

        return _storage.TryLoadAllFromJson ( ) ;
    }

    public Task < (bool , SettingsDto) > GetById ( string id )
    {
        _logger.LogInformation ( $"Id: {id}" ) ;

        return _storage.TryLoadFromJson ( id ) ;
    }

    public Task < (bool , SettingsDto) > GetDefault ( string id )
    {
        _logger.LogInformation ( "Getting default settings" ) ;

        return _storage.GetDefaultSettings ( id ) ;
    }

    public Task < (bool , SettingsDto) > AddOrUpdate ( SettingsDto dto )
    {
        _logger.LogInformation ( $"Id: {dto?.Id}" ) ;

        return _storage.TrySaveAsJson ( dto ) ;
    }

    private readonly ILogger < SettingsRepository > _logger ;

    private readonly ISettingsStorage _storage ;
}