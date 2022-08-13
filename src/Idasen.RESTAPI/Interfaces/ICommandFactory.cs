using JetBrains.Annotations ;

namespace Idasen.RestApi.Interfaces ;

public interface ICommandFactory
{
    ICommand Up ( ) ;
    ICommand ToHeight ( uint height ) ;
    ICommand Down ( ) ;

    [ UsedImplicitly ]
    ICommand Stop ( ) ;
}