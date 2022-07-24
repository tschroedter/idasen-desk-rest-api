using System ;
using System.Reflection;
using FluentValidation.AspNetCore;
using Idasen.BluetoothLE.Core ;
using Idasen.RestApi.BackgroundServices;
using Idasen.RESTAPI.Desks;
using Idasen.RESTAPI.Filters;
using Idasen.RestApi.Interfaces ;
using Idasen.RESTAPI.Interfaces;
using Idasen.RESTAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddTransient < ISettingsRepository , SettingsRepository > ( ) ;

            // ReSharper disable once RedundantArgumentDefaultValue
            services.AddSingleton ( CreateDeskManager ) ;

            services.AddHostedService<DeskManagerInitializerService>();

            services.AddSingleton < IApiConfigurationProvider , ApiConfigurationProvider > ( ) ;
        }

        public void Configure ( IApplicationBuilder builder )
        {
            builder.UseRouting ( ) ;
            builder.UseHealthChecks ( "/health" ) ;
            builder.UseEndpoints ( endpoints => { endpoints.MapControllers ( ) ; } ) ;
        }

        public IDeskManager CreateDeskManager ( IServiceProvider serviceProvider )
        {
            var useFake = GetUseFakeDeskManager ( serviceProvider ) ;

            return useFake
                       ? DeskManagerRegistrations.CreateFakeDeskManager ( )
                       : DeskManagerRegistrations.CreateRealDeskManager ( ) ;
        }

        private static bool GetUseFakeDeskManager ( IServiceProvider serviceProvider )
        {
            var provider      = serviceProvider.GetService < IApiConfigurationProvider > ( ) ;

            Guard.ArgumentNotNull ( provider, nameof(provider) );

            var configuration = provider?.GetConfigurationRoot ( ) ;

            return configuration.GetValue < bool > ( "use-fake-desk-manager" ) ;
        }
    }
}