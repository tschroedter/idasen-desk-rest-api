using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;

public abstract class CommandBase : ICommand
{
    protected CommandBase ( ILogger      logger ,
                            IDeskManager manager )
    {
        Guard.ArgumentNotNull ( logger ,
                                nameof ( logger ) ) ;
        Guard.ArgumentNotNull ( manager ,
                                nameof ( manager ) ) ;

        Logger  = logger ;
        Manager = manager ;
    }

    public abstract string CommandName { get ; }

    public async Task < bool > Execute ( )
    {
        Logger.LogInformation ( $"Execute command '{CommandName}'..." ) ;

        try
        {
            if ( ! Manager.IsReady )
            {
                Logger.LogError ( $"Failed to execute command '{CommandName}' " +
                                  "because DeskManger isn't ready" ) ;

                return false ;
            }

            var status = await ExecuteDeskCommand ( ) ;

            LogStatus ( status ) ;

            return status ;
        }
        catch ( OperationCanceledException )
        {
            Logger.LogInformation ( $"Executing command '{CommandName}' " +
                                    "was cancelled" ) ;

            return false ;
        }
        catch ( Exception e )
        {
            Logger.LogError ( e ,
                              $"Failed to execute command '{CommandName}'" ) ;

            return false ;
        }
    }

    protected bool IsDeskValid ( )
    {
        if ( Manager.Desk != null ) return true ;

        Logger.LogWarning ( "Desk is null" ) ;

        return false ;
    }

    private void LogStatus ( bool status )
    {
        if ( status )
            Logger.LogInformation ( $"Executing command '{CommandName}' " +
                                    "was successful" ) ;
        else
            Logger.LogError ( $"Failed to execute command '{CommandName}'" ) ;
    }

    protected abstract Task < bool > ExecuteDeskCommand ( ) ;
    protected readonly ILogger       Logger ;
    protected readonly IDeskManager  Manager ;
}