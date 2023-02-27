using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;

public class Down : CommandBase
{
    public Down ( ILogger < Up >  logger ,
                  IDeskManager manager )
        : base ( logger ,
                 manager )
    {
    }

    public override string CommandName => "Down" ;

    protected override Task < bool > ExecuteDeskCommand ( )
    {
        return Manager.Desk.MoveDownAsync ( ) ;
    }
}