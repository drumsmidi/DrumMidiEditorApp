using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage.Pickers;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pIO;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pView.pMenuBar;

public sealed partial class PageMenuBar : Page
{
	/// <summary>
	/// メディア設定
	/// </summary>
	private ConfigMedia ConfigMedia => Config.Media;

	/// <summary>
	/// システム設定
	/// </summary>
	private ConfigSystem ConfigSystem => Config.System;

	/// <summary>
	/// プレイヤー設定
	/// </summary>
	private ConfigPlayer ConfigPlayer => Config.Player;

	/// <summary>
	/// スコア
	/// </summary>
	private Score Score => DMS.SCORE;

	/// <summary>
	/// プレイヤー表示設定リスト
	/// </summary>
	private readonly ObservableCollection<string> _PlayerDisplayList = new();

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public PageMenuBar()
    {
        InitializeComponent();

		// プレイヤー表示タイプリスト
		foreach ( var name in Enum.GetNames<ConfigPlayer.PlayerLayoutMode>() )
		{
			if ( name != null )
			{ 
				_PlayerDisplayList.Add( name );
			}
		}

		// NumberBox の入力書式設定
		_LoopPlayMeasureStartNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 0, 1 );
		_LoopPlayMeasureEndNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 0, 1 );
		_LoopPlayMeasureConnectNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 0, 1 );
		_LoopPlayMeasureStartNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 0, 1 );
		_LoopPlayMeasureEndNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 0, 1 );

#if DEBUG
		var filepath = new GeneralPath("D:/CreateGame/DrumMidiEditor/build/net6.0-windows10.0.19041.0/Dms/test.dms");

		FileIO.LoadScore( filepath, out var score );
		//FileIO.SaveScore( filepath, score );

		DMS.SCORE			= score;
		DMS.OpenFilePath	= filepath;

		ApplyScore();
#endif
	}

	/// <summary>
	/// スコアをシステム全体に反映
	/// </summary>
	private void ApplyScore()
    {
        try
        {
			Config.EventReloadScore();

		
			DMS.SCORE.EditChannelNo = _ChannelNoComboBox.SelectedValue != null 
				? Convert.ToByte( _ChannelNoComboBox.SelectedValue.ToString() )
				: ConfigMedia.ChannelDrum;


			//if ( DMS.EditerForm != null )
			//{
			//	//DMS.EditerForm.MusicCtl.ApplyScore();
			//	//DMS.EditerForm.MusicCtl.ApplyEqulizer();
			//	DMS.EditerForm.MidiMapSetCtl.LoadMidiMapSet( DMS.SCORE.EditMidiMapSet );
			//	DMS.EditerForm.Refresh();

			//	DMS.EditerForm.Text = $"{Config.System.AppName} [{Config.System.OpenFilePath.AbsoulteFilePath}]";
			//}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	#region Menu

	/// <summary>
	/// 再生停止＆プレイヤーフォーム一時非表示
	/// </summary>
	private static void PlayerStop()
    {
		DmsControl.StopPreSequence();
	}

	/// <summary>
	/// メニュー：DMS新規作成
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void MenuItemNew_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			XamlHelper.MessageDialogYesNoAsync
				( 
					Content.XamlRoot,
					ResourcesHelper.GetString( "DialogMenuItemNew/Title" ),
					ResourcesHelper.GetString( "DialogMenuItemNew/Content" ),
					ResourcesHelper.GetString( "Dialog/Yes" ),
					ResourcesHelper.GetString( "Dialog/No" ),
					new( () =>
                    {
						DMS.SCORE			= new();
						DMS.OpenFilePath	= new();

						ApplyScore();
					})
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// メニュー：DMS開く
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void MenuItemOpen_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			var filepath = new GeneralPath();

			XamlHelper.OpenDialogAsync
				(
					ControlAccess.MainWindow,
					ConfigSystem.SupportDmsOpen,
					filepath,
					PickerLocationId.DocumentsLibrary,
					ConfigSystem.FolderDms,
					() =>
                    {
				        if ( !FileIO.LoadScore( filepath, out var score ) )
						{
							return;
						}

						DMS.SCORE			= score;
						DMS.OpenFilePath	= filepath;

						ApplyScore();
					}
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// メニュー：DMS上書き保存
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void MenuItemSave_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			var filepath = new GeneralPath( DMS.OpenFilePath.AbsoulteFilePath );

			if ( !filepath.IsExistFile )
			{ 
				XamlHelper.SaveDialogAsync
					(
						ControlAccess.MainWindow,
						ConfigSystem.SupportDmsSave,
						filepath,
						PickerLocationId.DocumentsLibrary,
						ConfigSystem.FolderDms,
						() =>
						{
							filepath.Extension = ConfigSystem.ExtentionDms;

							if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
                            {
                                return;
                            }

                            DMS.OpenFilePath = filepath;
                        }
                    );
			}
			else
            {
                if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
                {
                    return;
                }
            }
        }
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// メニュー：DMS別名保存
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void MenuItemSaveAs_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			var filepath = new GeneralPath( DMS.OpenFilePath.AbsoulteFilePath );

			XamlHelper.SaveDialogAsync
				(
					ControlAccess.MainWindow,
					ConfigSystem.SupportDmsSave,
					filepath,
					PickerLocationId.DocumentsLibrary,
					ConfigSystem.FolderDms,
					() =>
                    {
						filepath.Extension = ConfigSystem.ExtentionDms;

						if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
						{
							return;
						}

						DMS.OpenFilePath = filepath;
					}
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// メニュー：Export - Midi
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void MenuItemExportMidi_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

            var filepath = new GeneralPath( DMS.OpenFilePath.FileNameWithoutExtension );

			XamlHelper.SaveDialogAsync
				(
					ControlAccess.MainWindow,
					ConfigSystem.SupportMidi,
					filepath,
					PickerLocationId.DocumentsLibrary,
					ConfigSystem.FolderExport,
					() =>
                    {
						if ( !FileIO.SaveMidi( filepath, DMS.SCORE ) )
						{
							return;
						}
					}
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// メニュー：Export - Video
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void MenuItemExportVideo_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop();

            var filepath = new GeneralPath( DMS.OpenFilePath.FileNameWithoutExtension );

            XamlHelper.SaveDialogAsync
				(
					ControlAccess.MainWindow,
					ConfigSystem.SupportVideo,
					filepath,
					PickerLocationId.DocumentsLibrary,
					ConfigSystem.FolderExport,
					() =>
                    {
						filepath.Extension = ConfigSystem.ExtentionVideo;

						if ( !FileIO.SaveVideo( filepath, DMS.SCORE ) )
						{
							return;
						}
					}
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// メニュー：Import - Midi
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void MenuItemImportMidi_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop();

			var filepath = new GeneralPath();

			XamlHelper.OpenDialogAsync
				(
					ControlAccess.MainWindow,
					ConfigSystem.SupportMidi,
					filepath,
					PickerLocationId.DocumentsLibrary,
					ConfigSystem.FolderMidi,
					() =>
                    {
						var page = new PageImportMidi
						{
							BpmZoom = ConfigMedia.MidiImportZoom
						};

						XamlHelper.InputDialogOkCancelAsync
							(
								Content.XamlRoot,
								ResourcesHelper.GetString( "LabelImportMidi" ),
								page,
								() =>
								{
									ConfigMedia.MidiImportZoom = page.BpmZoom;

									//var score = DMS.SCORE;

									//if ( !FileIO.ImportScore( filepath, ref score ) )
									//{
									//	return;
									//}

									//DMS.SCORE = score;

									//ApplyScore();
								}
							);
					}
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	#endregion

	#region Command

	/// <summary>
	/// チャンネルNO切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void ChannelNoComboBox_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
		try
		{
			Score.EditChannelNo = (byte)Convert.ToInt32( _ChannelNoComboBox.SelectedItem.ToString() ?? $"{ConfigMedia.ChannelDrum}" );

			Config.EventChangeChannel();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// 通常再生
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void PlayButton_Click( object sender, RoutedEventArgs args )
    {
		try
		{
			DmsControl.PlayPreSequence();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// 再生停止
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void StopButton_Click( object sender, RoutedEventArgs args )
    {
		try
		{
			DmsControl.StopPreSequence();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// ループ再生
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void LoopPlayButton_Click( object sender, RoutedEventArgs args )
    {
		try
		{
			DmsControl.PlayPreLoopSequence();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// ループ再生 小節番号変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void LoopPlayMeasureNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			SettingMeasureNo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// ループ再生 小節接続切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void LoopPlayMeasureConnectToggleSwitch_Toggled( object sender, RoutedEventArgs args )
    {
		try
		{
			SettingMeasureNo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// 小節番号設定
	/// </summary>
	private void SettingMeasureNo()
    {
		// 初期設定時のエラー回避
		if ( !IsLoaded )
        {
			return;
        }

#if DEBUG
		Log.Info( $"-------------------------------------------------------------------" );
		Log.Info( $"1.{ConfigMedia.PlayLoopStart}={_LoopPlayMeasureStartNumberBox.Value}" );
		Log.Info( $"2.{ConfigMedia.PlayLoopEnd}={_LoopPlayMeasureEndNumberBox.Value}" );
		Log.Info( $"3.{ConfigMedia.PlayLoopConnect}={_LoopPlayMeasureConnectNumberBox.Value}" );
		Log.Info( $"4.{ConfigMedia.PlayLoopConnectOn}={_LoopPlayMeasureConnectToggleSwitch.IsOn}" );
		Log.Info( $"-------------------------------------------------------------------" );
#endif

		var start	= (int)_LoopPlayMeasureStartNumberBox.Value;
		var end		= (int)_LoopPlayMeasureEndNumberBox.Value;
		var conn	= (int)_LoopPlayMeasureConnectNumberBox.Value;
		var min		= (int)_LoopPlayMeasureStartNumberBox.Minimum;
		var max		= (int)_LoopPlayMeasureEndNumberBox.Maximum;
		var on		= _LoopPlayMeasureConnectToggleSwitch.IsOn;

		if ( start < min )
		{
			start = min;
		}
		if ( start > max )
		{
			start = max;
		}

		if ( on )
		{
			end = start + conn;
		}
		else if ( start > end )
		{
			end = start;
		}

		if ( end > max )
		{
			end = max;
		}

		_LoopPlayMeasureStartNumberBox.Value	= start;
		_LoopPlayMeasureEndNumberBox.Value		= end;
    }

	/// <summary>
	/// プレイヤー表示切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void PlayerDisplayComboBox_SelectionChanged( object sender, SelectionChangedEventArgs args )
	{
        try
        {
			var tag = args.AddedItems[ 0 ].ToString();

			var value = Enum.GetValues<ConfigPlayer.PlayerLayoutMode>()
							.FirstOrDefault( e => Enum.GetName<ConfigPlayer.PlayerLayoutMode>( e ) == tag );

			ConfigPlayer.PlayerLayoutModeSelect = value;

			ControlAccess.PageEditerMain?.UpdateGridLayout();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

    #endregion
}
