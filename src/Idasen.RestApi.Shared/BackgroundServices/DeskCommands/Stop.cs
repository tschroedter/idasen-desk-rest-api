using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;

public class Stop : CommandBase
{
    public Stop ( ILogger < Up > logger ,
                  IDeskManager   manager )
        : base ( logger ,
                 manager )
    {
    }

    public override string CommandName => "Stop" ;

    protected override Task < bool > ExecuteDeskCommand ( )
    {
        return ! IsDeskValid ( )
                   ? Task.FromResult ( false )
                   : Manager.Desk!.MoveStopAsync ( ) ;
    }
}