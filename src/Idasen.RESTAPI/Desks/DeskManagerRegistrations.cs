using System.Collections.Generic ;
using System.IO ;
using System.Threading.Tasks ;
using Autofac ;
using Autofac.Core ;
using Idasen.Launcher ;
using Idasen.RestApi.Shared.Interfaces ;
using JetBrains.Annotations ;
using Microsoft.Extensions.Configuration ;

namespace Idasen.RestApi.Desks ;

public static class DeskManagerRegistrations
{
    [ UsedImplicitly ] public static Task < bool > DeskManager ;

    public static IDeskManager CreateRealDeskManager ( )
    {
        IEnumerable < IModule > otherModules = new List < IModule > { new IdasenRESTAPIModule ( ) } ;

        var builder = new ConfigurationBuilder ( ).SetBasePath ( Directory.GetCurrentDirectory ( ) )
                                                  .AddJsonFile ( "idasen-desk.json" ) ;

        var container = ContainerProvider.Create ( builder.Build ( ) ,
                                                   otherModules ) ;

        var manager = container.Resolve < IDeskManager > ( ) ;

        return manager ;
    }
}