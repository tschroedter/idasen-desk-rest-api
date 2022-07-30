using Idasen.RestApi.Interfaces ;
using Microsoft.Extensions.Configuration ;

namespace Idasen.RESTAPI ;

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