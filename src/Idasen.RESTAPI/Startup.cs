using System ;
using System.Reflection ;
using System.Threading ;
using System.Threading.Tasks ;
using FluentValidation.AspNetCore ;
using Idasen.BluetoothLE.Core ;
using Idasen.RESTAPI.Desks ;
using Idasen.RESTAPI.Filters ;
using Idasen.RESTAPI.Interfaces ;
using Idasen.RESTAPI.Repositories ;
using JetBrains.Annotations ;
using Microsoft.AspNetCore.Builder ;
using Microsoft.Extensions.Configuration ;
using Microsoft.Extensions.DependencyInjection ;
using Microsoft.Extensions.Hosting ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RESTAPI
{
    public class Startup
    {
        public void ConfigureServices ( IServiceCollection services )
        {
            services.AddAutoMapper ( Assembly.GetExecutingAssembly ( ) ) ;

            services.AddControllers ( options => { options.Filters.Add ( new ValidationFilter ( ) ) ; } ) ;

            services.AddFluentValidation ( options =>
                                           {
                                               options.RegisterValidatorsFromAssemblyContaining < Startup > ( ) ;
                                           } ) ;

            services.AddHealthChecks ( )
                    .AddCheck < DeskManagerHealthCheck > ( "Desk Manager" ) ;

            services.AddLogging ( config => config.AddConfiguration ( GetLoggingConfiguration ( ) ) ) ;

            services.AddTransient < ISettingsRepository , SettingsRepository > ( ) ;

            // ReSharper disable once RedundantArgumentDefaultValue
            services.AddSingleton ( _ => CreateDeskManager ( ) ) ;

            services.AddHostedService<DeskManagerBackground>();
        }

        public void Configure ( IApplicationBuilder builder )
        {
            builder.UseRouting ( ) ;
            builder.UseHealthChecks ( "/health" ) ;
            builder.UseEndpoints ( endpoints => { endpoints.MapControllers ( ) ; } ) ;
        }

        public IDeskManager CreateDeskManager ( )
        {
            var useFake = GetUseFakeDeskManager ( ) ;

            return useFake
                       ? DeskManagerRegistrations.CreateFakeDeskManager ( )
                       : DeskManagerRegistrations.CreateRealDeskManager ( ) ;
        }

        private static bool GetUseFakeDeskManager ( )
        {
            return GetConfiguration ( ).GetValue < bool > ( "use-fake-desk-manager" ) ;
        }

        private static IConfiguration GetLoggingConfiguration ( )
        {
            return GetConfiguration ( ).GetSection ( "Logging" ) ;
        }

        private static IConfigurationRoot GetConfiguration ( )
        {
            var builder = new ConfigurationBuilder ( ).AddJsonFile ( "appsettings.json" ,
                                                                     true ,
                                                                     true ) ;

            var configuration = builder.Build ( ) ;
            return configuration ;
        }
    }

    public class DeskManagerBackground : BackgroundService
    {
        private readonly ILogger < DeskManagerBackground > _logger ;
        private readonly IDeskManager                      _manager ;

        public DeskManagerBackground ( [ NotNull ] ILogger <DeskManagerBackground> logger,
                                       [ NotNull ] IDeskManager                    manager )
        {
            Guard.ArgumentNotNull ( logger ,
                                    nameof ( logger ) ) ;
            Guard.ArgumentNotNull ( manager ,
                                    nameof ( manager ) ) ;

            _logger  = logger ;
            _manager = manager ;
        }

        protected override async Task ExecuteAsync ( CancellationToken cancellationToken)
        {
            try
            {
                var success = false ;

                while ( ! cancellationToken.IsCancellationRequested &&
                        ! success )
                {
                    _logger.LogInformation("Trying to initializing DeskManager...");

                    success = await _manager.Initialise ( ) ;
                }

                _logger.LogInformation("...DeskManager initialized!");
            }
            catch ( Exception e )
            {
                _logger.LogError ( e ,
                                   "Failed to initialize DeskManager." ) ;
            }
        }
    }
}