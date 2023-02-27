using Idasen.RestApi.Shared.Dtos ;

namespace Idasen.RestApi.Shared.Interfaces ;

public interface ISettingsRepository
{
    Task < ( bool , SettingsDto) >                AddOrUpdate ( SettingsDto dto ) ;
    Task < ( bool , SettingsDto) >                GetById ( string          id ) ;
    Task < ( bool , SettingsDto) >                GetDefault ( string       id ) ;
    Task < (bool , IEnumerable < SettingsDto >) > GetAll ( ) ;
}