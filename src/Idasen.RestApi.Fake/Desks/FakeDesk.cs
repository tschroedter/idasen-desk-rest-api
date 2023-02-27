

// ReSharper disable UnusedMember.Global

using System.Reactive.Subjects ;
using Idasen.RestApi.Shared.Interfaces ;

namespace Idasen.RestApi.Fake.Desks ;

public class FakeDesk : IRestDesk
{
    public FakeDesk ( )
    {
        Name                 = "Fake Desk" ;
        BluetoothAddress     = 1u ;
        BluetoothAddressType = "Fake Address Type" ;
        DeviceName           = "Fake Desk Device" ;
    }

    public Task < bool > MoveToAsync ( uint targetHeight )
    {
        return DoAction ( ( ) => DoMoveTo ( targetHeight ) ) ;
    }

    public Task < bool > MoveUpAsync ( )
    {
        return DoAction ( DoMoveUp ) ;
    }

    public Task < bool > MoveDownAsync ( )
    {
        return DoAction ( DoMoveDown ) ;
    }

    public Task < bool > MoveStopAsync ( )
    {
        DoMoveStop ( ) ; // execute immediately

        return Task.FromResult ( true ) ;
    }

    public uint Height { get ; private set ; } = 6000u ;

    public int Speed { get ; private set ; }

    public IObservable < IEnumerable < byte > > DeviceNameChanged     => _deviceNameChanged ;
    public IObservable < uint >                 HeightChanged         => _heightChanged ;
    public IObservable < int >                  SpeedChanged          => _speedChanged ;
    public IObservable < HeightSpeedDetails >   HeightAndSpeedChanged => _heightAndSpeedChanged ;
    public IObservable < uint >                 FinishedChanged       => _finishedChanged ;
    public IObservable < bool >                 RefreshedChanged      => _refreshedChanged ;
    public string                               Name                  { get ; }
    public ulong                                BluetoothAddress      { get ; }
    public string                               BluetoothAddressType  { get ; }
    public string                               DeviceName            { get ; }

    public uint Step { get ; set ; } = 100u ;

    public int StepSpeed { get ; set ; } = 25 ;

    public TimeSpan DefaultStepSleep { get ; set ; } = TimeSpan.FromSeconds ( 0.5 ) ;

    public bool IsInUse { get ; private set ; }

    public bool IsStopRequested { get ; private set ; }

    public bool IsLocked { get ; private set ; }

    public void Connect ( )
    {
    }

    public void MoveTo ( uint targetHeight )
    {
        DoMoveTo ( targetHeight ) ;
    }

    public void MoveUp ( )
    {
        DoMoveUp ( ) ;
    }

    public void MoveDown ( )
    {
        DoMoveDown ( ) ;
    }

    public void MoveStop ( )
    {
        DoMoveStop ( ) ;
    }

    public void MoveLock ( )
    {
        DoMoveLock ( ) ;
    }

    public void MoveUnlock ( )
    {
        DoMoveUnlock ( ) ;
    }

    public Task < bool > MoveLockAsync ( )
    {
        return DoAction ( DoMoveLock ) ;
    }

    public Task < bool > MoveUnlockAsync ( )
    {
        return DoAction ( DoMoveUnlock ) ;
    }

    private void CreateNewSourceAndToken ( )
    {
        if ( _source != null )
        {
            _source.Cancel ( ) ;
            _source.Dispose ( ) ;
        }

        _source = new CancellationTokenSource ( ) ;
        _source.CancelAfter ( _defaultTimeout ) ;
        _token = _source.Token ;
    }

    private void DoMoveTo ( uint targetHeight )
    {
        if ( IsLocked ||
             CheckIfIsInUse ( ) )
            return ;

        DoMoveToSteps ( targetHeight ) ;
    }

    private void DoMoveToSteps ( uint targetHeight )
    {
        IsStopRequested = false ;

        var steps      = CalculateSteps ( targetHeight ) ;
        var isMoveDown = targetHeight <= Height ;

        for ( var i = 1 ; i < steps ; i ++ )
        {
            if ( IsStopRequested )
                return ;

            if ( isMoveDown )
                DoMoveDownOneStep ( ) ;
            else
                DoMoveUpOneStep ( ) ;
        }

        Height = targetHeight ;
        Speed  = 0 ;

        FinishedMove ( ) ;
    }

    private int CalculateSteps ( uint targetHeight )
    {
        return Math.Abs ( ( ( int )targetHeight - ( int )Height ) / ( int )Step ) ;
    }

    private bool CheckIfIsInUse ( )
    {
        lock ( _padlock )
        {
            if ( IsInUse )
                return false ;

            IsInUse = true ;
        }

        return true ;
    }

    private void DoMoveUp ( )
    {
        if ( IsLocked ||
             CheckIfIsInUse ( ) )
            return ;

        DoMoveUpOneStep ( ) ;
    }

    private void DoMoveUpOneStep ( )
    {
        Height = ( uint )( ( int )Height + ( int )Step ) ;
        Speed  = StepSpeed ;

        PublishHeightAndSpeed ( ) ;

        Thread.Sleep ( DefaultStepSleep ) ;

        FinishedMove ( ) ;
    }

    private void DoMoveDown ( )
    {
        if ( IsLocked ||
             CheckIfIsInUse ( ) )
            return ;

        DoMoveDownOneStep ( ) ;
    }

    private void DoMoveDownOneStep ( )
    {
        Height = ( uint )( ( int )Height - ( int )Step ) ;
        Speed  = - StepSpeed ;

        PublishHeightAndSpeed ( ) ;

        Thread.Sleep ( DefaultStepSleep ) ;

        FinishedMove ( ) ;
    }

    private void DoMoveStop ( )
    {
        Thread.Sleep ( DefaultStepSleep ) ;

        IsStopRequested = true ;

        FinishedMove ( ) ;
    }

    private void DoMoveLock ( )
    {
        IsLocked = true ;
    }

    private void DoMoveUnlock ( )
    {
        IsLocked = false ;
    }

    private async Task < bool > DoAction ( Action action )
    {
        lock ( _padlock )
        {
            if ( IsInUse )
                return false ;

            IsInUse = true ;
        }

        CreateNewSourceAndToken ( ) ;

        await Task.Run ( action ,
                         _token ) ;

        lock ( _padlock )
        {
            IsInUse = false ;
        }

        return true ;
    }

    private void PublishHeightAndSpeed ( )
    {
        _heightChanged.OnNext ( Height ) ;
        _speedChanged.OnNext ( Speed ) ;

        var details = new HeightSpeedDetails ( DateTimeOffset.Now ,
                                               Height ,
                                               Speed ) ;

        _heightAndSpeedChanged.OnNext ( details ) ;
    }

    private void FinishedMove ( )
    {
        Speed = 0 ;

        _heightChanged.OnNext ( Height ) ;
        _speedChanged.OnNext ( Speed ) ;

        var details = new HeightSpeedDetails ( DateTimeOffset.Now ,
                                               Height ,
                                               0 ) ;

        _heightAndSpeedChanged.OnNext ( details ) ;
    }

    private readonly TimeSpan _defaultTimeout = TimeSpan.FromMinutes ( 1 ) ;
    private readonly Subject < IEnumerable < byte > > _deviceNameChanged     = new( ) ;
    private readonly Subject < uint >                 _finishedChanged       = new( ) ;
    private readonly Subject < HeightSpeedDetails >   _heightAndSpeedChanged = new( ) ;
    private readonly Subject < uint >                 _heightChanged         = new( ) ;
    private readonly object                           _padlock               = new( ) ;
    private readonly Subject < bool >                 _refreshedChanged      = new( ) ;
    private readonly Subject < int >                  _speedChanged          = new( ) ;
    private          CancellationTokenSource?         _source ;
    private          CancellationToken                _token ;
}