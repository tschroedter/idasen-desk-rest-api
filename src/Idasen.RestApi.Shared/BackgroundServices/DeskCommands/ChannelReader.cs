using System.Threading.Channels ;
using Idasen.RestApi.Shared.Interfaces ;

namespace Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;

public class ChannelReader : IChannelReader
{
    public ChannelReader ( Channel < ICommand > channel )
    {
        Guard.ArgumentNotNull ( channel ,
                                nameof ( channel ) ) ;

        _channel = channel ;
    }

    public IAsyncEnumerable < ICommand > ReadAllAsync ( CancellationToken cancellationToken )
    {
        return _channel.Reader.ReadAllAsync ( cancellationToken ) ;
    }

    private readonly Channel < ICommand > _channel ;
}