namespace Idasen.RestApi.Shared.Interfaces ;

public interface IRestDesk
{
    uint          Height { get ; }
    int           Speed  { get ; }
    Task < bool > MoveToAsync ( uint targetHeight ) ;
    Task < bool > MoveUpAsync ( ) ;
    Task < bool > MoveDownAsync ( ) ;
    Task < bool > MoveStopAsync ( ) ;
}