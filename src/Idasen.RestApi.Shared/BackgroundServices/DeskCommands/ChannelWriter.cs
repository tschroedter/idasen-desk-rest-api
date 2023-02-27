using System.Threading.Channels ;
using Idasen.RestApi.Shared.Interfaces ;

namespace Idasen.RestApi.Shared.BackgroundServices.DeskCommands ;

public class ChannelWriter : IChannelWriter
{
    public ChannelWriter ( Channel < ICommand > channel )
    {
        Guard.ArgumentNotNull ( channel ,
                                nameof ( channel ) ) ;

        _channel = channel ;
    }

    public ValueTask WriteAsync ( ICommand          message ,
                                  CancellationToken cancellationToken )
    {
        return _channel.Writer.WriteAsync ( message ,
                                            cancellationToken ) ;
    }

    private readonly Channel < ICommand > _channel ;
}