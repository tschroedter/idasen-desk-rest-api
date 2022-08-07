using System.Threading.Tasks ;
using Idasen.RESTAPI.Interfaces ;
using JetBrains.Annotations ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.BackgroundServices.DeskCommands ;

public class Stop : CommandBase
{
    public Stop ( [ NotNull ] ILogger < Up > logger ,
                  [ NotNull ] IDeskManager   manager )
        : base ( logger ,
                 manager )
    {
    }

    public override string CommandName => "Stop" ;

    protected override Task < bool > ExecuteDeskCommand ( )
    {
        return Manager.Desk.MoveStopAsync ( ) ;
    }
}