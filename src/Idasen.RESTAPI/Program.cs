using System ;
using Idasen.RestApi.Shared ;
using Serilog ;
using Topshelf ;

namespace Idasen.RestApi ;

internal class Program
{
    private static void Main ( )
    {
        ConfigureLogger ( ) ;

        var rc = HostFactory.Run ( x =>
                                   {
                                       x.Service < IdasenRestApi > ( s =>
                                                                     {
                                                                         s.ConstructUsing ( _ =>
                                                                                                new
                                                                                                    IdasenRestApi ( ) ) ;
                                                                         s.WhenStarted ( tc => tc.Start ( ) ) ;
                                                                         s.WhenStopped ( tc => tc.Stop ( ) ) ;
                                                                     } ) ;

                                       x.RunAsLocalSystem ( ) ;

                                       x.SetDescription ( "Idasen REST API Host" ) ;
                                       x.SetDisplayName ( "Idasen REST API" ) ;
                                       x.SetServiceName ( "Idasen REST API" ) ;
                                       x.StartAutomatically ( ) ;
                                       x.UseSerilog ( ) ;
                                       //x.RunAs("username", "password");
                                   } ) ;

        var exitCode = ( int )Convert.ChangeType ( rc ,
                                                   rc.GetTypeCode ( ) ) ;

        Environment.ExitCode = exitCode ;
    }

    private static void ConfigureLogger ( )
    {
        var provider = new ApiConfigurationProvider ( ) ;

        var root = provider.GetConfigurationRoot ( ) ;

        Log.Logger = new LoggerConfiguration ( ).ReadFrom
                                                .Configuration ( root )
                                                .CreateLogger ( ) ;
    }
}