using Idasen.RestApi.Shared.Interfaces ;
using JetBrains.Annotations ;

namespace Idasen.RestApi.Fake.Desks ;

public static class DeskManagerRegistrations
{
    [ UsedImplicitly ] public static Task < bool >? DeskManager ;

    [ UsedImplicitly ]
    public static IDeskManager CreateFakeDeskManager ( )
    {
        IDeskManager manager = new FakeDeskManager ( ) ;

        return manager ;
    }
}