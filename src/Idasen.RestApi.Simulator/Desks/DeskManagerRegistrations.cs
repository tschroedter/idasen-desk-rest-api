using Idasen.RestApi.Shared.Interfaces ;
using JetBrains.Annotations ;

namespace Idasen.RestApi.Simulator.Desks ;

public static class DeskManagerRegistrations
{
    [ UsedImplicitly ] public static Task < bool >? DeskManager ;

    public static IDeskManager CreateFakeDeskManager ( )
    {
        IDeskManager manager = new FakeDeskManager ( ) ;

        return manager ;
    }
}