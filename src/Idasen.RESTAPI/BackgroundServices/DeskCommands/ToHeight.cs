using System.Threading.Tasks ;
using Idasen.RestApi.Shared.Interfaces ;
using JetBrains.Annotations ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.BackgroundServices.DeskCommands ;

public class ToHeight : CommandBase
{
    public ToHeight ( [ NotNull ] ILogger < Up > logger ,
                      [ NotNull ] IDeskManager   manager ,
                      uint                       targetHeight )
        : base ( logger ,
                 manager )
    {
        TargetHeight = targetHeight ;
    }

    public override string CommandName => "ToHeight" ;

    public uint TargetHeight { get ; }

    protected override Task < bool > ExecuteDeskCommand ( )
    {
        return Manager.Desk.MoveToAsync ( TargetHeight ) ;
    }
}