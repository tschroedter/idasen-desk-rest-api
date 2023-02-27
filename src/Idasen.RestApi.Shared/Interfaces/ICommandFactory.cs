using JetBrains.Annotations ;

namespace Idasen.RestApi.Shared.Interfaces ;

public interface ICommandFactory
{
    ICommand Up ( ) ;
    ICommand ToHeight ( uint height ) ;
    ICommand Down ( ) ;

    [ UsedImplicitly ]
    ICommand Stop ( ) ;
}