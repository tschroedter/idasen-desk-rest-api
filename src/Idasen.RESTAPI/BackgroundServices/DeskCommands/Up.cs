using System.Threading.Tasks ;
using Idasen.RestApi.Shared.Interfaces ;
using JetBrains.Annotations ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.BackgroundServices.DeskCommands ;

public class Up : CommandBase
{
    public Up ( [ NotNull ] ILogger < Up > logger ,
                [ NotNull ] IDeskManager   manager )
        : base ( logger ,
                 manager )
    {
    }

    public override string CommandName => "Up" ;

    protected override Task < bool > ExecuteDeskCommand ( )
    {
        return Manager.Desk.MoveUpAsync ( ) ;
    }
}