using System.Threading.Tasks ;
using Idasen.RESTAPI.Dtos ;

namespace Idasen.RESTAPI.Interfaces ;

public interface ISettingsRepository
{
    Task < ( bool , SettingsDto) > AddOrUpdate ( SettingsDto dto ) ;
    Task < ( bool , SettingsDto) > GetById ( string          id ) ;
    Task < ( bool , SettingsDto) > GetDefault ( string       id ) ;
}