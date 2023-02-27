namespace Idasen.RestApi.Shared.Interfaces ;

public interface IDeskManager
{
    bool          IsReady { get ; }
    IRestDesk?    Desk    { get ; }
    Task < bool > Initialise ( ) ;
}