using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Configuration ;

namespace Idasen.RestApi.Shared ;

public class ApiConfigurationProvider : IApiConfigurationProvider
{
    public IConfigurationRoot GetConfigurationRoot ( )
    {
        var builder = new ConfigurationBuilder ( ).AddJsonFile ( "appsettings.json" ,
                                                                 true ,
                                                                 true ) ;

        var configuration = builder.Build ( ) ;

        return configuration ;
    }
}