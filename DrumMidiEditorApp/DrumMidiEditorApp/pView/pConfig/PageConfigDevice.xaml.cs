using System;
using System.Collections.ObjectModel;
using DrumMidiEditorApp.pAudio;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pEvent;
using Microsoft.UI.Xaml.Controls;
using DrumMidiEditorApp.pUtil.pHelper;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigDevice : Page
{
    /// <summary>
    /// Media設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// Midiデバイスリスト
    /// </summary>
    private readonly ObservableCollection<string> _MidiDeviceList = [];

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigDevice()
    {
        // 初期化
        InitializeComponent();

        // Midiデバイス読込
        LoadMidiDeviceList();

        // NumberBox の入力書式設定
        _MidiOutLatencyNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 3, 0.001 );
    }

    #region Midi device

    /// <summary>
    /// Midiデバイスリスト読込
    /// </summary>
    private void LoadMidiDeviceList()
    {
        _MidiDeviceList.Clear();

        var list = MidiNet.GetDeviceList();

        list.ForEach( _MidiDeviceList.Add );
    }

    /// <summary>
    /// MIDI-OUTデバイス選択
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MidiOutDeviceListView_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
        try
        {
            MidiNet.InitDeviceAsync( ConfigMedia.MidiOutDeviceName );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// MIDI-OUT遅延時間変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MidiOutLatencyNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( sender, args ) )
            {
                return;
            }

            EventManage.EventUpdateMidiOutLatency();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
