﻿using System.Collections.Generic ;
using System.Threading.Tasks ;
using Idasen.RestApi.Shared.Dtos ;

namespace Idasen.RestApi.Interfaces ;

public interface ISettingsStorage
{
    Task < (bool , SettingsDto) >                 TrySaveAsJson ( SettingsDto dto ) ;
    Task < (bool , SettingsDto) >                 TryLoadFromJson ( string    id ) ;
    Task < (bool , SettingsDto) >                 GetDefaultSettings ( string id ) ;
    Task < (bool , IEnumerable < SettingsDto >) > TryLoadAllFromJson ( ) ;
}