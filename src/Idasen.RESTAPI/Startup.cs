﻿using System.Reflection ;
using FluentValidation.AspNetCore ;
using Idasen.RestApi.BackgroundServices ;
using Idasen.RESTAPI.Desks ;
using Idasen.RESTAPI.Filters ;
using Idasen.RESTAPI.Interfaces ;
using Idasen.RESTAPI.Repositories ;
using Microsoft.AspNetCore.Builder ;
using Microsoft.Extensions.Configuration ;
using Microsoft.Extensions.DependencyInjection ;
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

            services.AddHostedService<DeskManagerInitializerService>();
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
}