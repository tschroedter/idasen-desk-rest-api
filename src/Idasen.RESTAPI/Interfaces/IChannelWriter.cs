using System.Threading ;
using System.Threading.Tasks ;

namespace Idasen.RestApi.Interfaces ;

public interface IChannelWriter
{
    ValueTask WriteAsync ( ICommand          message ,
                           CancellationToken cancellationToken ) ;
}