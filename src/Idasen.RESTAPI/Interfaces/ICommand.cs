using System.Threading.Tasks ;

namespace Idasen.RestApi.Interfaces ;

public interface ICommand
{
    string        CommandName { get ; }
    Task < bool > Execute ( ) ;
}