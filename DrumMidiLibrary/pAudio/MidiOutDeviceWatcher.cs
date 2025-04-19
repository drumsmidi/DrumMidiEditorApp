using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;

namespace DrumMidiLibrary.pAudio;

internal partial class MidiOutDeviceWatcher : DisposeBaseClass
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MidiOutDeviceWatcher()
    {
        _DeviceWatcher = DeviceInformation.CreateWatcher( _MidiSelector );

        _DeviceWatcher.Added                 += DeviceWatcher_Added;
        _DeviceWatcher.Removed               += DeviceWatcher_Removed;
        _DeviceWatcher.Updated               += DeviceWatcher_Updated;
        _DeviceWatcher.EnumerationCompleted  += DeviceWatcher_EnumerationCompleted;
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( _Disposed )
        {
            return;
        }

        // マネージドリソースの解放
        if ( aDisposing )
        {
        }

        // アンマネージドリソースの解放
        {
            _DeviceWatcher.Added                -= DeviceWatcher_Added;
            _DeviceWatcher.Removed              -= DeviceWatcher_Removed;
            _DeviceWatcher.Updated              -= DeviceWatcher_Updated;
            _DeviceWatcher.EnumerationCompleted -= DeviceWatcher_EnumerationCompleted;
        }

        _Disposed = true;

        base.Dispose( aDisposing );
    }
    private bool _Disposed = false;

    #region member

    /// <summary>
    /// ロックオブジェクト
    /// </summary>
    private readonly Lock _LockObj = new();

    /// <summary>
    /// MIDI-OUTデバイス全検索文字列
    /// </summary>
    private readonly string _MidiSelector = MidiOutPort.GetDeviceSelector();

    /// <summary>
    /// MIDI-OUTデバイスウオッチャー
    /// </summary>
    private readonly DeviceWatcher _DeviceWatcher;

    /// <summary>
    /// MIDI-OUTデバイス検索完了フラグ
    /// </summary>
    private volatile bool _EnumerationCompleted = false;

    /// <summary>
    /// MIDI-OUTデバイス検索完了タスク
    /// </summary>
    private readonly TaskCompletionSource<bool> _EnumerationCompletedTask = new();

    /// <summary>
    /// MIDI-OUTデバイス一覧（デバイス名、デバイス情報）
    /// </summary>
    public readonly Dictionary<string, DeviceInformation> MidiOutDeviceDic = [];

    #endregion

    /// <summary>
    /// Start the Device Watcher
    /// </summary>
    public void DeviceStart()
    {
        if ( _DeviceWatcher.Status != DeviceWatcherStatus.Started )
        {
            _DeviceWatcher.Start();
        }
    }

    /// <summary>
    /// Stop the Device Watcher
    /// </summary>
    public void DeviceStop()
    {
        if ( _DeviceWatcher.Status != DeviceWatcherStatus.Stopped )
        {
            _DeviceWatcher.Stop();
        }
    }

    /// <summary>
    /// Update UI on device added
    /// </summary>
    /// <param name="aSender">The active DeviceWatcher instance</param>
    /// <param name="aArgs">Event arguments</param>
    private async void DeviceWatcher_Added( DeviceWatcher aSender, DeviceInformation aArgs )
    {
        try
        {
            if ( _EnumerationCompleted )
            {
                await UpdateDevices();
            }
        }
        catch ( Exception e )
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}", false );
        }
    }

    /// <summary>
    /// Update UI on device removed
    /// </summary>
    /// <param name="aSender">The active DeviceWatcher instance</param>
    /// <param name="aArgs">Event arguments</param>
    private async void DeviceWatcher_Removed( DeviceWatcher aSender, DeviceInformationUpdate aArgs )
    {
        try
        {
            if ( _EnumerationCompleted )
            {
                await UpdateDevices();
            }
        }
        catch ( Exception e )
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}", false );
        }
    }

    /// <summary>
    /// Update UI on device updated
    /// </summary>
    /// <param name="aSender">The active DeviceWatcher instance</param>
    /// <param name="aArgs">Event arguments</param>
    private async void DeviceWatcher_Updated( DeviceWatcher aSender, DeviceInformationUpdate aArgs )
    {
        try
        {
            if ( _EnumerationCompleted )
            {
                await UpdateDevices();
            }
        }
        catch ( Exception e )
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}", false );
        }
    }

    /// <summary>
    /// Update UI on device enumeration completed.
    /// </summary>
    /// <param name="aSender">The active DeviceWatcher instance</param>
    /// <param name="aArgs">Event arguments</param>
    private async void DeviceWatcher_EnumerationCompleted( DeviceWatcher aSender, object aArgs )
    {
        try
        {
            _EnumerationCompleted = true;

            await UpdateDevices();

            _EnumerationCompletedTask.TrySetResult( true );
        }
        catch ( Exception e )
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}", false );
        }
    }

    /// <summary>
    /// Add any connected MIDI devices to the list
    /// </summary>
    private async Task UpdateDevices()
    {
        try
        {
            var deviceInformationCollection = await DeviceInformation.FindAllAsync( _MidiSelector );

            lock ( _LockObj )
            {
                MidiOutDeviceDic.Clear();

                foreach ( var device in deviceInformationCollection )
                {
                    MidiOutDeviceDic.Add( device.Name, device );
                }
            }
        }
        catch ( Exception e )
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}", false );
        }
    }

    /// <summary>
    /// MIDI-OUTポート取得
    /// </summary>
    /// <param name="aMidiOutDeviceName">MIDI-OUTデバイス名</param>
    /// <returns>MIDI-OUTポート</returns>
    public async Task<IMidiOutPort?> GetMidiOutPortAsync( string aMidiOutDeviceName )
    {
        IMidiOutPort? port = null;

        try
        {
            if ( !_EnumerationCompleted )
            {
                await _EnumerationCompletedTask.Task;
            }

            if ( MidiOutDeviceDic.TryGetValue( aMidiOutDeviceName, out var device ) )
            {
                port = (IMidiOutPort?)await MidiOutPort.FromIdAsync( device.Id );
            }
        }
        catch ( Exception e ) 
        {
            Log.Warning( $"{Log.GetThisMethodName}:{e.Message}", false );
        }
        return port;
    }
}
