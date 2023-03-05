using Idasen.RestApi.Service ;
using Microsoft.Extensions.Logging.Configuration ;
using Microsoft.Extensions.Logging.EventLog ;

if ( args is { Length: 1 } )
{
    var installer = new ServiceInstaller ( ) ;

    switch ( args [ 0 ] )
    {
        case "/Install" :
            await installer.Install ( ) ;
            break ;

        case "/Uninstall" :
            await installer.Uninstall ( ) ;
            break ;

        default :
            Console.WriteLine ( $"Unknown argument '{args [ 0 ]}'" ) ;
            break ;
    }

    return ;
}

using var host = Host.CreateDefaultBuilder ( args )
                     .UseWindowsService ( options => { options.ServiceName = "Idasen Rest Api" ; } )
                     .ConfigureServices ( services =>
                                          {
                                              LoggerProviderOptions.RegisterProviderOptions <
                                                  EventLogSettings , EventLogLoggerProvider > ( services ) ;

                                              services.AddSingleton < JokeService > ( ) ;
                                              services.AddHostedService < WindowsBackgroundService > ( ) ;
                                          } )
                     .ConfigureLogging ( ( context ,
                                           logging ) =>
                                         {
                                             // See: https://github.com/dotnet/runtime/issues/47303
                                             logging.AddConfiguration (
                                                                       context.Configuration
                                                                              .GetSection ( "Logging" ) ) ;
                                         } )
                     .Build ( ) ;

await host.RunAsync ( ) ;