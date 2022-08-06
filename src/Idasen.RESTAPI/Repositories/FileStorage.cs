﻿using System ;
using System.Collections.Generic ;
using System.IO ;
using System.Text.Json ;
using System.Threading.Tasks ;
using Idasen.BluetoothLE.Core ;
using Idasen.RESTAPI.Dtos ;
using Idasen.RestApi.Interfaces ;
using JetBrains.Annotations ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RESTAPI.Repositories ;

public class FileStorage : ISettingsStorage
{
    public FileStorage ( [ NotNull ] ILogger < FileStorage > logger )
    {
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;

        _logger = logger ;

        Folder = Path.Combine ( new [ ]
                                {
                                    ApplicationData ,
                                    "idasen-desk-rest-api" ,
                                    "settings"
                                } ) ;

        if ( ! Directory.Exists ( Folder ) )
            Directory.CreateDirectory ( Folder ) ;
    }

    public async Task<(bool, IEnumerable<SettingsDto>)> TryLoadAllFromJson()
    {
        try
        {
            return await DoTryLoadAllFromJson();
        }
        catch (Exception e)
        {
            _logger.LogError(e,
                             "Failed to load all instances");

            return (false, null);
        }
    }

    public async Task < (bool , SettingsDto ) > TrySaveAsJson ( SettingsDto dto )
    {
        try
        {
            return await DoTrySaveAsJson ( dto ) ;
        }
        catch ( Exception e )
        {
            _logger.LogError ( e ,
                               $"Failed to save instance for '{dto?.Id}'" ) ;

            return ( false , null ) ;
        }
    }

    public async Task < (bool , SettingsDto) > TryLoadFromJson ( string id )
    {
        try
        {
            return await DoTryLoadFromJson ( id ) ;
        }
        catch ( Exception e )
        {
            _logger.LogError ( e ,
                               $"Failed to load settings for '{id}' " +
                               $"because of '{e.Message}'" ) ;

            return ( false , null ) ;
        }
    }

    public Task < (bool , SettingsDto) > GetDefaultSettings ( string id )
    {
        try
        {
            var dto = new SettingsDto { Id = id } ;

            return Task.FromResult ( ( true , dto ) ) ;
        }
        catch ( Exception e )
        {
            _logger.LogError ( e ,
                               "Failed to get default settings" ) ;

            return Task.FromResult ( ( false , ( SettingsDto )null ) ) ;
        }
    }

    public string ApplicationData => Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData ) ;
    public string Folder          { get ; }

    private async Task<(bool, IEnumerable<SettingsDto>)> DoTryLoadAllFromJson()
    {
        var files = Directory.GetFiles ( Folder ,
                                         $"*.{FileType}" ) ;

        List < SettingsDto > dtos = new( ) ;

        foreach ( var fullname in files )
        {
            var (success , dto) = await DoTryLoadByFullnameAsJson ( fullname ) ;

            if (success)
                dtos.Add ( dto );
        }

        return (true, dtos);
    }

    private async Task < (bool , SettingsDto) > DoTryLoadByFullnameAsJson ( string fullname )
    {
        var id = GetIdFromFullname ( fullname ) ;

        var (success , dto ) = await DoTryLoadFromJson ( id ) ;

        if ( success )
            return ( true , dto ) ;

        _logger.LogWarning ( $"Failed to load settings for id '{fullname}'" ) ;

        return (true, dto) ;
    }

    private static string GetIdFromFullname ( string fullname )
    {
        return Path.GetFileName ( fullname )
                   .Replace ( $".{FileType}" ,
                              "" ) ;
    }

    private async Task < (bool , SettingsDto) > DoTrySaveAsJson ( SettingsDto dto )
    {
        var fullname = CreateFullname ( dto.Id ) ;
        var json     = JsonSerializer.Serialize ( dto ) ;

        await File.WriteAllTextAsync ( fullname ,
                                       json ) ;

        return ( true , dto ) ;
    }

    private const string FileType = "json" ;

    private string CreateFullname ( string id )
    {
        return Path.Combine ( Folder ,
                              Path.GetFileName ( $"{id}.{FileType}" ) ) ;
    }

    private async Task < (bool , SettingsDto) > DoTryLoadFromJson ( string id )
    {
        var fullname = CreateFullname ( id ) ;

        if ( ! File.Exists ( fullname ) )
        {
            _logger.LogError ( $"Failed to load settings for id '{id}' " +
                               $"because file '{fullname}' doesn't exist" ) ;

            return ( false , null ) ;
        }

        var json = await File.ReadAllTextAsync ( fullname ) ;

        var dto = JsonSerializer.Deserialize < SettingsDto > ( json ) ;

        return ( dto != null , dto ) ;
    }

    private readonly ILogger < FileStorage > _logger ;
}