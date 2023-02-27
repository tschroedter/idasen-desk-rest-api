using System.Diagnostics.CodeAnalysis ;
using Autofac ;
using Autofac.Extras.DynamicProxy ;
using Idasen.Aop ;
using Idasen.RestApi.Desks ;
using Idasen.RestApi.Shared.Interfaces ;

namespace Idasen.RestApi ;

// ReSharper disable once InconsistentNaming
[ ExcludeFromCodeCoverage ]
public class IdasenRESTAPIModule
    : Module
{
    protected override void Load ( ContainerBuilder builder )
    {
        builder.RegisterModule < BluetoothLEAop > ( ) ;

        builder.RegisterType < DeskManager > ( )
               .As < IDeskManager > ( )
               .EnableInterfaceInterceptors ( ) ;
    }
}