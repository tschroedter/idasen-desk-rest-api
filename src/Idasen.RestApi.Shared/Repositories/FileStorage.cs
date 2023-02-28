using System.Text.Json ;
using Idasen.RestApi.Shared.Dtos ;
using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Shared.Repositories ;

public class FileStorage : ISettingsStorage
{
    public FileStorage ( ILogger < FileStorage > logger )
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

    public async Task < (bool , IEnumerable < SettingsDto >) > TryLoadAllFromJson ( )
    {
        try
        {
            return await DoTryLoadAllFromJson ( ) ;
        }
        catch ( Exception e )
        {
            _logger.LogError ( e ,
                               "Failed to load all instances" ) ;

            return ( false , Array.Empty < SettingsDto > ( ) ) ;
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
                               $"Failed to save instance for '{dto.Id}'" ) ;

            return ( false , SettingsDto.Failed ) ;
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

            return ( false , SettingsDto.Failed ) ;
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

            return Task.FromResult ( ( false , SettingsDto.Failed ) ) ;
        }
    }

    private const string FileType = "json" ;

    public string ApplicationData => Environment.GetFolderPath ( Environment.SpecialFolder.ApplicationData ) ;
    public string Folder          { get ; }

    private async Task < (bool , IEnumerable < SettingsDto >) > DoTryLoadAllFromJson ( )
    {
        var files = Directory.GetFiles ( Folder ,
                                         $"*.{FileType}" ) ;

        List < SettingsDto > dtos = new( ) ;

        foreach ( var fullname in files )
        {
            var (success , dto) = await DoTryLoadByFullnameAsJson ( fullname ) ;

            if ( success )
                dtos.Add ( dto ) ;
        }

        return ( true , dtos ) ;
    }

    private async Task < (bool , SettingsDto) > DoTryLoadByFullnameAsJson ( string fullname )
    {
        var id = GetIdFromFullname ( fullname ) ;

        var (success , dto) = await DoTryLoadFromJson ( id ) ;

        if ( success )
            return ( true , dto ) ;

        _logger.LogWarning ( $"Failed to load settings for id '{fullname}'" ) ;

        return ( true , dto ) ;
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

            return ( false , SettingsDto.Failed) ;
        }

        var json = await File.ReadAllTextAsync ( fullname ) ;

        SettingsDto dto = JsonSerializer.Deserialize<SettingsDto>(json) ?? SettingsDto.Failed;

        return ( dto != SettingsDto.Failed, dto ) ;
    }

    private readonly ILogger < FileStorage > _logger ;
}