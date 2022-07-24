using Microsoft.Extensions.Configuration;

namespace Idasen.RestApi.Interfaces;

public interface IApiConfigurationProvider
{
    IConfigurationRoot GetConfigurationRoot();
}