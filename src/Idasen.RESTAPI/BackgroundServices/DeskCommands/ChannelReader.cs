using System.Collections.Generic ;
using System.Threading ;
using System.Threading.Channels ;
using Idasen.BluetoothLE.Core ;
using Idasen.RestApi.Interfaces ;
using JetBrains.Annotations ;

namespace Idasen.RestApi.BackgroundServices.DeskCommands ;

public class ChannelReader : IChannelReader
{
    public ChannelReader ( [ NotNull ] Channel < ICommand > channel )
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