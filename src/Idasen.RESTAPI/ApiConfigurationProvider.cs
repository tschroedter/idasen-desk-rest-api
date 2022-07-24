using Microsoft.Extensions.Configuration ;
using Idasen.RestApi.Interfaces;

namespace Idasen.RESTAPI ;

public class ApiConfigurationProvider : IApiConfigurationProvider
{
    public IConfigurationRoot GetConfigurationRoot()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json",
                                                             true,
                                                             true);

        var configuration = builder.Build();

        return configuration;
    }
}