namespace Idasen.RestApi.Shared.Interfaces ;

public interface ICommand
{
    string        CommandName { get ; }
    Task < bool > Execute ( ) ;
}