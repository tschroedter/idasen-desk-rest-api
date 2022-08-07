namespace Idasen.RestApi.Interfaces ;

public interface ICommandFactory
{
    ICommand Up ( ) ;
    ICommand ToHeight ( uint height ) ;
    ICommand Stop ( ) ;
    ICommand Down ( ) ;
}