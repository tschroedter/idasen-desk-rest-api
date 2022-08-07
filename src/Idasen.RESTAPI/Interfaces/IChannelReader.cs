using System.Collections.Generic ;
using System.Threading ;

namespace Idasen.RestApi.Interfaces ;

public interface IChannelReader
{
    IAsyncEnumerable < ICommand > ReadAllAsync ( CancellationToken cancellationToken ) ;
}