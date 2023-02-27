using System.Threading.Channels;
using System.Reflection ;
using FluentValidation.AspNetCore ;
using Idasen.RestApi.Fake.Desks ;
using Idasen.RestApi.Shared ;
using Idasen.RestApi.Shared.BackgroundServices ;
using Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;
using Idasen.RestApi.Shared.Interfaces ;
using Idasen.RestApi.Shared.Repositories ;
using Serilog ;

var configurationProvider = new ApiConfigurationProvider();

var root = configurationProvider.GetConfigurationRoot();

// todo check if we need root
Log.Logger = new LoggerConfiguration().ReadFrom
                                      .Configuration(root)
                                      .CreateLogger();


var builder = WebApplication.CreateBuilder(args);

// Use Serilog
builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
builder.Services
       .AddAutoMapper ( Assembly.GetExecutingAssembly ( ) )
       .AddAutoMapper ( Assembly.GetAssembly ( typeof ( FakeDesk ) ) ) ;

builder.Services
       .AddHealthChecks()
       .AddCheck<DeskManagerHealthCheck>("Desk Manager");

builder.Services.AddSingleton(DeskManagerRegistrations.CreateFakeDeskManager());

builder.Services
       .AddTransient < ISettingsRepository , SettingsRepository > ( )
       .AddTransient < ISettingsStorage , FileStorage > ( ) ;

builder.Services
       .AddFluentValidationAutoValidation ( )
       .AddFluentValidationClientsideAdapters ( ) ;

builder.Services
       .AddHostedService<DeskManagerInitializerService>()
       .AddHostedService<DeskManagerCommandService>();

var channel = Channel.CreateBounded<ICommand>(3);

IChannelWriter writer = new ChannelWriter(channel);
IChannelReader reader = new ChannelReader(channel);

builder.Services
       .AddSingleton ( channel )
       .AddSingleton ( writer )
       .AddSingleton ( reader ) ;

builder.Services
       .AddSingleton < ICommandFactory , CommandFactory > ( )
       .AddTransient < Up > ( )
       .AddTransient < Down > ( )
       .AddTransient < Stop > ( ) ;
        // ToHeight requires a uint (see Func<uint, ToHeight> ) ;

builder.Services
       .AddSingleton ( provider => new Func < Up > ( provider.GetService < Up > ) )
       .AddSingleton ( provider => new Func < Down > ( provider.GetService < Down > ) )
       .AddSingleton ( provider => new Func < Stop > ( provider.GetService < Stop > ) )
       .AddSingleton ( ToHeightFactory ) ;

builder.Services
       .AddSingleton<IApiConfigurationProvider, ApiConfigurationProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();
app.MapControllers();
app.UseSerilogRequestLogging();

app.Run();


Func<uint, ToHeight> ToHeightFactory(IServiceProvider provider)
{
    return toHeight =>
               new ToHeight(provider.GetRequiredService<ILogger<Up>>(),
                            provider.GetRequiredService<IDeskManager>(),
                            toHeight);
}