using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;

public class Up : CommandBase
{
    public Up ( ILogger < Up > logger ,
                IDeskManager   manager )
        : base ( logger ,
                 manager )
    {
    }

    public override string CommandName => "Up" ;

    protected override Task < bool > ExecuteDeskCommand ( )
    {
        return ! IsDeskValid ( )
                   ? Task.FromResult ( false )
                   : Manager.Desk!.MoveUpAsync ( ) ;
    }
}