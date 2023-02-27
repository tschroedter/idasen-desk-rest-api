using System ;
using System.Reflection ;
using System.Threading.Channels ;
using FluentValidation.AspNetCore ;
using Idasen.RestApi.Fake.Desks ;
using Idasen.RestApi.Shared ;
using Idasen.RestApi.Shared.BackgroundServices ;
using Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;
using Idasen.RestApi.Shared.Filters ;
using Idasen.RestApi.Shared.Interfaces ;
using Idasen.RestApi.Shared.Repositories ;
using Microsoft.AspNetCore.Builder ;
using Microsoft.Extensions.Configuration ;
using Microsoft.Extensions.DependencyInjection ;
using Microsoft.Extensions.Logging ;
using DeskManagerRegistrations = Idasen.RestApi.Desks.DeskManagerRegistrations ;
using Guard = Idasen.BluetoothLE.Core.Guard ;

namespace Idasen.RestApi ;

public class Startup
{
    public void ConfigureServices ( IServiceCollection services )
    {
        services.AddAutoMapper ( Assembly.GetExecutingAssembly ( ) ) ;
        services.AddAutoMapper ( Assembly.GetAssembly ( typeof ( FakeDesk ) ) ) ;

        services.AddControllers ( options => { options.Filters.Add ( new ValidationFilter ( ) ) ; } ) ;

        services.AddFluentValidationAutoValidation (  )
                .AddFluentValidationClientsideAdapters ( ) ;

        services.AddHealthChecks ( )
                .AddCheck < DeskManagerHealthCheck > ( "Desk Manager" ) ;

        services.AddTransient < ISettingsRepository , SettingsRepository > ( ) ;
        services.AddTransient < ISettingsStorage , FileStorage > ( ) ;

        // ReSharper disable once RedundantArgumentDefaultValue
        services.AddSingleton ( CreateDeskManager ) ;

        services.AddHostedService < DeskManagerInitializerService > ( ) ;
        services.AddHostedService < DeskManagerCommandService > ( ) ;

        var channel = Channel.CreateBounded < ICommand > ( 3 ) ;
        services.AddSingleton ( channel ) ;

        IChannelWriter writer = new ChannelWriter ( channel ) ;
        services.AddSingleton ( writer ) ;

        IChannelReader reader = new ChannelReader ( channel ) ;
        services.AddSingleton ( reader ) ;

        services.AddSingleton < ICommandFactory , CommandFactory > ( ) ;
        services.AddTransient < Up > ( ) ;
        services.AddTransient < Down > ( ) ;
        services.AddTransient < Stop > ( ) ;
        services.AddTransient < ToHeight > ( ) ;

        services.AddSingleton ( provider => new Func < Up > ( provider.GetService < Up > ) ) ;
        services.AddSingleton ( provider => new Func < Down > ( provider.GetService < Down > ) ) ;
        services.AddSingleton ( provider => new Func < Stop > ( provider.GetService < Stop > ) ) ;
        services.AddSingleton ( ToHeightFactory ) ;

        services.AddSingleton < IApiConfigurationProvider , ApiConfigurationProvider > ( ) ;
    }

    private static Func < uint , ToHeight > ToHeightFactory ( IServiceProvider provider )
    {
        return toHeight =>
                   new ToHeight ( provider.GetRequiredService < ILogger < Up > > ( ) ,
                                  provider.GetRequiredService < IDeskManager > ( ) ,
                                  toHeight ) ;
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
                   ? Fake.Desks.DeskManagerRegistrations.CreateFakeDeskManager ( )
                   : DeskManagerRegistrations.CreateRealDeskManager ( ) ;
    }

    private static bool GetUseFakeDeskManager ( IServiceProvider serviceProvider )
    {
        var provider = serviceProvider.GetService < IApiConfigurationProvider > ( ) ;

        Guard.ArgumentNotNull ( provider ,
                                nameof ( provider ) ) ;

        var configuration = provider?.GetConfigurationRoot ( ) ;

        return configuration.GetValue < bool > ( "use-fake-desk-manager" ) ;
    }
}