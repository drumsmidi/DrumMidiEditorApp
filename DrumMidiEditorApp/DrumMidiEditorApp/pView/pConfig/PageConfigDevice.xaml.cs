using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

using DrumMidiClassLibrary.pAudio;
using DrumMidiClassLibrary.pConfig;
using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pWinUI;

using DrumMidiEditorApp.pEvent;

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
	private readonly ObservableCollection<string> _MidiDeviceList = new();

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
			= XamlHelper.CreateNumberFormatter( 1, 3, 0.001 );
	}

	#region Midi device

	/// <summary>
	/// Midiデバイスリスト読込
	/// </summary>
	private void LoadMidiDeviceList()
    {
		_MidiDeviceList.Clear();

		var list = MidiNet.GetDeviceList();

		list.ForEach( item => _MidiDeviceList.Add( item ) );
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
			if ( !MidiNet.InitDevice( ConfigMedia.MidiOutDeviceName ) )
			{
				Log.Error( ResourcesHelper.GetString( "ErrMsgInitMidiDevice", ConfigMedia.MidiOutDeviceName ) );
				return;
			}
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
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
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
