using System ;
using Serilog ;
using Topshelf ;

namespace Idasen.RESTAPI
{
    internal class Program
    {
        private static void Main ( )
        {
            var rc = HostFactory.Run ( x =>
                                       {
                                           x.Service < IdasenRestApi > ( s =>
                                                                         {
                                                                             s.ConstructUsing ( name => new IdasenRestApi ( ) ) ;
                                                                             s.WhenStarted ( tc => tc.Start ( ) ) ;
                                                                             s.WhenStopped ( tc => tc.Stop ( ) ) ;
                                                                         } ) ;
                                           x.RunAsLocalSystem ( ) ;

                                           x.SetDescription ( "Idasen REST API Host" ) ;
                                           x.SetDisplayName ( "Idasen REST API" ) ;
                                           x.SetServiceName ( "Idasen REST API" ) ;
                                           x.StartAutomatically ( ) ;
                                           x.UseSerilog ( CreateLoggerConfiguration ( ) );
                                           //x.RunAs("username", "password");
                                       } ) ;

            var exitCode = ( int )Convert.ChangeType ( rc ,
                                                       rc.GetTypeCode ( ) ) ;

            Environment.ExitCode = exitCode ;
        }

        private static LoggerConfiguration CreateLoggerConfiguration ( )
        {
            return new LoggerConfiguration ( )
                  .WriteTo.File ( AppDomain.CurrentDomain.BaseDirectory + "\\logs\\app-{Date}.log" )
                  .WriteTo.Console ( )
                  .MinimumLevel.Debug ( ) ;
        }
    }
}