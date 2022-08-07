using System ;
using Idasen.BluetoothLE.Core ;
using Idasen.RestApi.Interfaces ;
using JetBrains.Annotations ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.BackgroundServices.DeskCommands ;

public class CommandFactory : ICommandFactory
{
    public CommandFactory ( [ NotNull ] ILogger < CommandFactory > logger ,
                            Func < Up >                            upFactory ,
                            Func < Down >                          downFactory ,
                            Func < Stop >                          stopFactory ,
                            Func < uint , ToHeight >               toHeightFactory )
    {
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;

        _logger          = logger ;
        _upFactory       = upFactory ;
        _downFactory     = downFactory ;
        _stopFactory     = stopFactory ;
        _toHeightFactory = toHeightFactory ;
    }

    public ICommand Up ( )
    {
        _logger.LogInformation ( "Creating instance for command 'Up'" ) ;

        return _upFactory ( ) ;
    }

    public ICommand Down ( )
    {
        _logger.LogInformation ( "Creating instance for command 'Down'" ) ;

        return _downFactory ( ) ;
    }

    public ICommand Stop ( )
    {
        _logger.LogInformation ( "Creating instance for command 'Stop'" ) ;

        return _stopFactory ( ) ;
    }

    public ICommand ToHeight ( uint height )
    {
        _logger.LogInformation ( "Creating instance for command 'ToHeight'" ) ;

        return _toHeightFactory ( height ) ;
    }

    private readonly Func < Down > _downFactory ;

    private readonly ILogger < CommandFactory > _logger ;
    private readonly Func < Stop >              _stopFactory ;
    private readonly Func < uint , ToHeight >   _toHeightFactory ;
    private readonly Func < Up >                _upFactory ;
}