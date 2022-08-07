using System.Threading.Tasks ;
using Idasen.RESTAPI.Interfaces ;
using JetBrains.Annotations ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.BackgroundServices.DeskCommands ;

public class Down : CommandBase
{
    public Down ( [ NotNull ] ILogger < Up > logger ,
                  [ NotNull ] IDeskManager   manager )
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