using Microsoft.Extensions.Configuration ;

namespace Idasen.RestApi.Shared.Interfaces ;

public interface IApiConfigurationProvider
{
    IConfigurationRoot GetConfigurationRoot ( ) ;
}