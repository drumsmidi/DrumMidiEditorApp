using System;
using System.Collections.ObjectModel;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigDevice : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigDevice()
    {
        InitializeComponent();
    }

    #region member

    /// <summary>
    /// Media設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// Midiデバイスリスト
    /// </summary>
    private readonly ObservableCollection<string> _MidiDeviceList = [];

    #endregion

    /// <summary>
    /// ページロード完了後処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Page_Loaded( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            // Midiデバイス読込
            LoadMidiDeviceList();

            #region NumberBox の入力書式設定

            _MidiOutLatencyNumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 3, 0.001 );

            _BgmDesiredLatencyNumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 3, 0.001 );

            _BgmNumberOfBuffersNumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 0, 1 );

            #endregion
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #region Midi device

    /// <summary>
    /// Midiデバイスリスト読込
    /// </summary>
    private void LoadMidiDeviceList()
    {
        _MidiDeviceList.Clear();

        MidiNet.GetDeviceList().ForEach( _MidiDeviceList.Add );
    }

    /// <summary>
    /// MIDI-OUTデバイス選択
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiOutDeviceListView_SelectionChanged( object aSender, SelectionChangedEventArgs aArgs )
    {
        try
        {
            _ = MidiNet.InitDeviceAsync( ConfigMedia.MidiOutDeviceName );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// MIDI-OUT遅延時間変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MidiOutLatencyNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            EventManage.Event_Device_UpdateMidiOutLatency();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    #region Audio device

    /// <summary>
    /// BGM再生バッファの遅延時間（秒）変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void BgmDesiredLatencyNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            aSender.Value = ConfigMedia.CheckBgmDesiredLatency( aSender.Value );

            EventManage.Event_Device_UpdateBgmLatency();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// BGM再生バッファ数変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void BgmNumberOfBuffersNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            aSender.Value = ConfigMedia.CheckBgmNumberOfBuffers( (int)aSender.Value );

            EventManage.Event_Device_UpdateBgmLatency();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion
}
