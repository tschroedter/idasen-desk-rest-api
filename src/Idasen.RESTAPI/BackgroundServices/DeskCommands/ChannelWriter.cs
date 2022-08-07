using System.Threading ;
using System.Threading.Channels ;
using System.Threading.Tasks ;
using Idasen.BluetoothLE.Core ;
using Idasen.RestApi.Interfaces ;
using JetBrains.Annotations ;

namespace Idasen.RestApi.BackgroundServices.DeskCommands ;

public class ChannelWriter : IChannelWriter
{
    public ChannelWriter ( [ NotNull ] Channel < ICommand > channel )
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