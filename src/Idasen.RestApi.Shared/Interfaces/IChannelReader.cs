namespace Idasen.RestApi.Shared.Interfaces ;

public interface IChannelReader
{
    IAsyncEnumerable < ICommand > ReadAllAsync ( CancellationToken cancellationToken ) ;
}