using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pGeneralFunction.pAudio;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pView.pEditer.pConfig;

public sealed partial class PageConfig : Page
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
	public PageConfig()
    {
		// Midiデバイス読込
		LoadMidiDeviceList();

		// 初期化
		InitializeComponent();

		// NumberBox の入力書式設定
		_MidiOutLatencyNumberBox.NumberFormatter
			= XamlHelper.CreateNumberFormatter( 1, 3, 0.001 );
		_VideoFpsNumberBox.NumberFormatter
			= XamlHelper.CreateNumberFormatter( 1, 0, 1 );
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
#if DEBUG
			Log.Info( $"{ConfigMedia.MidiOutDeviceName}={_MidiOutDeviceListView.SelectedValue}" );
#endif

			if ( !MidiNet.InitDevice() )
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
#if DEBUG
			Log.Info( $"{ConfigMedia.MidiOutLatency}={_MidiOutLatencyNumberBox.Value}" );
#endif

			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			Config.EventReloadBgm();
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    #endregion

    #region Video

	/// <summary>
	/// Fps設定
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void VideoFpsNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
		try
		{ 
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    #endregion
}
