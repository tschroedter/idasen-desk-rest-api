namespace Idasen.RestApi.Shared.Interfaces ;

public interface IChannelWriter
{
    ValueTask WriteAsync ( ICommand          message ,
                           CancellationToken cancellationToken ) ;
}