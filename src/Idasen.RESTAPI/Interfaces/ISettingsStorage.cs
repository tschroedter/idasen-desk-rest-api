using System.Threading.Tasks ;
using Idasen.RESTAPI.Dtos ;

namespace Idasen.RestApi.Interfaces ;

public interface ISettingsStorage
{
    Task < (bool , SettingsDto) > TrySaveAsJson ( SettingsDto dto ) ;
    Task < (bool , SettingsDto) > TryLoadFromJson ( string    id ) ;
    Task < (bool , SettingsDto) > GetDefaultSettings ( string id ) ;
}