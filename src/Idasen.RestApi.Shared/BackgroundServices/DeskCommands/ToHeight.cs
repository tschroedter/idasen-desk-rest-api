using Idasen.RestApi.Shared.Interfaces ;
using Microsoft.Extensions.Logging ;

namespace Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;

public class ToHeight : CommandBase
{
    public ToHeight ( ILogger < Up >  logger ,
                      IDeskManager manager ,
                      uint            targetHeight )
        : base ( logger ,
                 manager )
    {
        TargetHeight = targetHeight ;
    }

    public override string CommandName => "ToHeight" ;

    public uint TargetHeight { get ; }

    protected override Task < bool > ExecuteDeskCommand ( )
    {
        return ! IsDeskValid ( )
                   ? Task.FromResult ( false )
                   : Manager.Desk!.MoveToAsync ( TargetHeight ) ;
    }
}