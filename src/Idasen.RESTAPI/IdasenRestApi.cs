using System ;
using System.Threading ;
using System.Threading.Tasks ;
using Microsoft.AspNetCore.Hosting ;
using Microsoft.Extensions.Configuration ;
using Microsoft.Extensions.Logging ;
using Serilog ;

namespace Idasen.RESTAPI
{
    public class IdasenRestApi
    {
        public IdasenRestApi ( )
        {
            _canceler = new CancellationTokenSource ( ) ;
        }

        private static Action < WebHostBuilderContext , IConfigurationBuilder > AddAppSettingsJson ( )
        {
            return ( context ,
                     builder ) =>
                   {
                       builder.AddJsonFile ( "appsettings.json" ,
                                             true ) ;
                   } ;
        }

        public void Start ( )
        {
            _serviceTask = ExecuteAsync ( ) ;
        }

        public void Stop ( )
        {
            _canceler.Cancel ( ) ;
            _serviceTask.Wait ( ) ;
        }

        public async Task ExecuteAsync ( )
        {
            try
            {
                _host = CreateWebHostBuilder ( ) ;

                await _host.RunAsync ( ) ;

                while ( ! _canceler.Token.IsCancellationRequested )
                {
                    Log.Logger.Information ( "Received request to stop service..." ) ;

                    Log.Logger.Information ( "Stopping web server..." ) ;
                    await _host.StopAsync ( TimeSpan.FromSeconds ( 5 ) ) ;

                    Log.Logger.Information ( "...stopped web server." ) ;
                }
            }
            catch ( Exception e )
            {
                Log.Logger.Error ( e, "Failed to stop service!" ) ;
            }
        }

        private IWebHost CreateWebHostBuilder ( )
        {
            return new WebHostBuilder ( ).UseKestrel ( )
                                         .UseUrls ( "http://*:5000" )
                                         .ConfigureLogging ( logging =>
                                                             {
                                                                 logging.ClearProviders ( ) ;
                                                                 logging.AddConsole ( ) ;
                                                                 logging.AddDebug ( ) ;
                                                             } )
                                         .UseStartup < Startup > ( )
                                         .ConfigureAppConfiguration ( AddAppSettingsJson ( ) )
                                         .Build ( ) ;
        }

        private readonly CancellationTokenSource _canceler ;
        private          IWebHost                _host ;
        private          Task                    _serviceTask ;
    }
}