using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;

namespace DrumMidiLibrary.pAudio;

internal partial class MidiOutDeviceWatcher : DisposeBaseClass
{
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
    private bool _EnumerationCompleted = false;

    /// <summary>
    /// MIDI-OUTデバイス一覧（デバイス名、デバイス情報）
    /// </summary>
    public readonly Dictionary<string, DeviceInformation> MidiOutDeviceDic = [];

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
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
            }

            // Dispose unmanaged resources.
            _DeviceWatcher.Added                -= DeviceWatcher_Added;
            _DeviceWatcher.Removed              -= DeviceWatcher_Removed;
            _DeviceWatcher.Updated              -= DeviceWatcher_Updated;
            _DeviceWatcher.EnumerationCompleted -= DeviceWatcher_EnumerationCompleted;

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

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
    /// <param name="sender">The active DeviceWatcher instance</param>
    /// <param name="args">Event arguments</param>
    private void DeviceWatcher_Added( DeviceWatcher sender, DeviceInformation args )
    {
        if ( _EnumerationCompleted )
        {
            UpdateDevices();
        }
    }

    /// <summary>
    /// Update UI on device removed
    /// </summary>
    /// <param name="sender">The active DeviceWatcher instance</param>
    /// <param name="args">Event arguments</param>
    private void DeviceWatcher_Removed( DeviceWatcher sender, DeviceInformationUpdate args )
    {
        if ( _EnumerationCompleted )
        {
            UpdateDevices();
        }
    }

    /// <summary>
    /// Update UI on device updated
    /// </summary>
    /// <param name="sender">The active DeviceWatcher instance</param>
    /// <param name="args">Event arguments</param>
    private void DeviceWatcher_Updated( DeviceWatcher sender, DeviceInformationUpdate args )
    {
        if ( _EnumerationCompleted )
        {
            UpdateDevices();
        }
    }

    /// <summary>
    /// Update UI on device enumeration completed.
    /// </summary>
    /// <param name="sender">The active DeviceWatcher instance</param>
    /// <param name="args">Event arguments</param>
    private void DeviceWatcher_EnumerationCompleted( DeviceWatcher sender, object args )
    {
        _EnumerationCompleted = true;

        UpdateDevices();
    }

    /// <summary>
    /// Add any connected MIDI devices to the list
    /// </summary>
    private async void UpdateDevices()
    {
        var deviceInformationCollection = await DeviceInformation.FindAllAsync( _MidiSelector );

        MidiOutDeviceDic.Clear();

        foreach ( var device in deviceInformationCollection )
        {
            MidiOutDeviceDic.Add( device.Name, device );
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
                await Task.Delay( 3000 );
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
