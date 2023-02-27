using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;

public abstract class CommandBase : ICommand
{
    protected CommandBase ( ILogger         logger ,
                            IDeskManager manager )
    {
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;
        Guard.ArgumentNotNull ( manager ,
                                nameof ( manager ) ) ;

        _logger = logger ;
        Manager = manager ;
    }

    public abstract string CommandName { get ; }

    public async Task < bool > Execute ( )
    {
        _logger.LogInformation ( $"Execute command '{CommandName}'..." ) ;

        try
        {
            if ( ! Manager.IsReady )
            {
                _logger.LogError ( $"Failed to execute command '{CommandName}' " +
                                   "because DeskManger isn't ready" ) ;

                return false ;
            }

            var status = await ExecuteDeskCommand ( ) ;

            LogStatus ( status ) ;

            return status ;
        }
        catch ( OperationCanceledException )
        {
            _logger.LogInformation ( $"Executing command '{CommandName}' " +
                                     "was cancelled" ) ;

            return false ;
        }
        catch ( Exception e )
        {
            _logger.LogError ( e ,
                               $"Failed to execute command '{CommandName}'" ) ;

            return false ;
        }
    }

    private void LogStatus ( bool status )
    {
        if ( status )
            _logger.LogInformation ( $"Executing command '{CommandName}' " +
                                     "was successful" ) ;
        else
            _logger.LogError ( $"Failed to execute command '{CommandName}'" ) ;
    }

    protected abstract Task < bool > ExecuteDeskCommand ( ) ;
    private readonly   ILogger       _logger ;
    protected readonly IDeskManager  Manager ;
}