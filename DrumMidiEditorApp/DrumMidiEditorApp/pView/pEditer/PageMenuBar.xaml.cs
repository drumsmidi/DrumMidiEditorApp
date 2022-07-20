using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pDMS;
using Windows.ApplicationModel.Resources;
using DrumMidiEditorApp.pIO;

namespace DrumMidiEditorApp.pView.pEditer;

public sealed partial class PageMenuBar : Page
{
	private ResourceLoader _Resource = new();

	private ConfigMedia _ConfigMedia => Config.Media;

	private ConfigSystem _ConfigSystem => Config.System;

	private ConfigPlayer _ConfigPlayer => Config.Player;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public PageMenuBar()
    {
        InitializeComponent();

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

			//if ( DMS.EditerForm != null )
			//{
			//	DMS.SCORE.EditChannelNo = (byte)ChannelComboBox.SelectedIndex;

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

		//DMS.PlayerForm?.TemporaryHide();
	}

	/// <summary>
	/// プレイヤーフォーム再表示
	/// </summary>
	private static void PlayerPlay()
	{
		//DMS.PlayerForm?.TemporaryShow();
	}

	/// <summary>
	/// メニュー：DMS新規作成
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void _MenuItemNew_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			XamlHelper.MessageDialogYesNo
				( 
					Content.XamlRoot,
					_Resource.GetString( "MenuItemNewMsgBox/Confirmation" ),
					_Resource.GetString( "MenuItemNewMsgBox/Content" ),
					_Resource.GetString( "MessageDialog/Yes" ),
					_Resource.GetString( "MessageDialog/No" ),
					new( () =>
                    {
						DMS.SCORE = new();

						DMS.OpenFilePath = new();

						ApplyScore();
					} )
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
            PlayerPlay();
		}
    }

	/// <summary>
	/// メニュー：DMS開く
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void _MenuItemOpen_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			XamlHelper.MessageDialogYesNo
				( 
					Content.XamlRoot,
					_Resource.GetString( "MenuItemNewMsgBox/Confirmation" ),
					_Resource.GetString( "MenuItemNewMsgBox/Content" ),
					_Resource.GetString( "MessageDialog/Yes" ),
					_Resource.GetString( "MessageDialog/No" ),
					new( () =>
                    {
						//if ( !FormUtil.OpenShowDialog( Config.System.FolderDms, Config.System.SupportDmsOpen, out var filepath ) )
						//         {
						//	return;
						//         }

						//         if ( !FileIO.LoadScore( filepath, out var score ) )
						//         {
						//	return;
						//         }

						//DMS.SCORE = score;

						//         Config.System.OpenFilePath = filepath;

						ApplyScore();
					})
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
            PlayerPlay();
		}

    }

	/// <summary>
	/// メニュー：DMS上書き保存
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void _MenuItemSave_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			XamlHelper.MessageDialogYesNo
				( 
					Content.XamlRoot,
					_Resource.GetString( "MenuItemNewMsgBox/Confirmation" ),
					_Resource.GetString( "MenuItemNewMsgBox/Content" ),
					_Resource.GetString( "MessageDialog/Yes" ),
					_Resource.GetString( "MessageDialog/No" ),
					new( () =>
                    {
						//var filepath = Config.System.OpenFilePath;

						//if ( !filepath.IsExistFile )
						//{ 
						//	if ( !FormUtil.SaveShowDialog( Config.System.FolderDms, Config.System.OpenFilePath, Config.System.SupportDmsSave, out filepath ) )
						//	{
						//		return;
						//	}
						//}

						//filepath.Extension = Config.System.ExtentionDms;

						//         if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
						//         {
						//             return;
						//         }

						//         Config.System.OpenFilePath = filepath;

						//ApplyScore();
					})
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
            PlayerPlay();
		}

    }

	/// <summary>
	/// メニュー：DMS別名保存
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void _MenuItemSaveAs_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			XamlHelper.MessageDialogYesNo
				( 
					Content.XamlRoot,
					_Resource.GetString( "MenuItemNewMsgBox/Confirmation" ),
					_Resource.GetString( "MenuItemNewMsgBox/Content" ),
					_Resource.GetString( "MessageDialog/Yes" ),
					_Resource.GetString( "MessageDialog/No" ),
					new( () =>
                    {
						//if ( !FormUtil.SaveShowDialog( Config.System.FolderDms, Config.System.OpenFilePath, Config.System.SupportDmsSave, out var filepath ) )
						//{
						//	return;
						//}

						//filepath.Extension = Config.System.ExtentionDms;

						//if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
						//         {
						//             return;
						//         }

						//         Config.System.OpenFilePath = filepath;

						//ApplyScore();
					})
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
            PlayerPlay();
		}

    }

	/// <summary>
	/// メニュー：Export - Midi
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void _MenuItemExportMidi_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			XamlHelper.MessageDialogYesNo
				( 
					Content.XamlRoot,
					_Resource.GetString( "MenuItemNewMsgBox/Confirmation" ),
					_Resource.GetString( "MenuItemNewMsgBox/Content" ),
					_Resource.GetString( "MessageDialog/Yes" ),
					_Resource.GetString( "MessageDialog/No" ),
					new( () =>
                    {
						//if ( !FormUtil.SaveShowDialog( Config.System.FolderMidi, Config.System.OpenFilePath, Config.System.SupportMidi, out var filepath ) )
						//{
						//	return;
						//}

						//if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
						//{
						//	return;
						//}
					})
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
            PlayerPlay();
		}

    }

	/// <summary>
	/// メニュー：Export - Video
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void _MenuItemExportVideo_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			XamlHelper.MessageDialogYesNo
				( 
					Content.XamlRoot,
					_Resource.GetString( "MenuItemNewMsgBox/Confirmation" ),
					_Resource.GetString( "MenuItemNewMsgBox/Content" ),
					_Resource.GetString( "MessageDialog/Yes" ),
					_Resource.GetString( "MessageDialog/No" ),
					new( () =>
                    {
						//if ( !FormUtil.SaveShowDialog( Config.System.FolderVideo, Config.System.OpenFilePath, Config.System.SupportVideo, out var filepath ) )
						//{
						//	return;
						//}

						//if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
						//{
						//	return;
						//}
					})
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
            PlayerPlay();
		}

    }

	/// <summary>
	/// メニュー：Export - TechMania
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void _MenuItemExportTechMidi_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			XamlHelper.MessageDialogYesNo
				( 
					Content.XamlRoot,
					_Resource.GetString( "MenuItemNewMsgBox/Confirmation" ),
					_Resource.GetString( "MenuItemNewMsgBox/Content" ),
					_Resource.GetString( "MessageDialog/Yes" ),
					_Resource.GetString( "MessageDialog/No" ),
					new( () =>
                    {
						//if ( !FormUtil.SaveShowDialog( Config.System.FolderTechMania, Config.System.OpenFilePath, Config.System.SupportTechMania, out var filepath ) )
						//{
						//	return;
						//}

						//if ( !FileIO.SaveScore( filepath, DMS.SCORE ) )
						//{
						//	return;
						//}
					})
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
            PlayerPlay();
		}

    }

	/// <summary>
	/// メニュー：Import - Midi
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void _MenuItemImportMidi_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop(); 

			XamlHelper.MessageDialogYesNo
				( 
					Content.XamlRoot,
					_Resource.GetString( "MenuItemNewMsgBox/Confirmation" ),
					_Resource.GetString( "MenuItemNewMsgBox/Content" ),
					_Resource.GetString( "MessageDialog/Yes" ),
					_Resource.GetString( "MessageDialog/No" ),
					new( () =>
                    {
                        //if (!FormUtil.OpenShowDialog(Config.System.FolderMidi, Config.System.SupportMidi, out var filepath))
                        //{
                        //    return;
                        //}

                        //using var fm = new ImportMidiForm();

                        //if (fm.ShowDialog(Config.Media.MidiImportZoom) != DialogResult.OK)
                        //{
                        //    return;
                        //}

                        //var score = DMS.SCORE;

                        //if (!FileIO.ImportScore(filepath, ref score))
                        //{
                        //    return;
                        //}

                        //DMS.SCORE = score;

                        //ApplyScore();
					} )
				);
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
            PlayerPlay();
		}
    }

	#endregion

	#region Command

	/// <summary>
	/// チャンネルNO切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void _ChannelNoComboBox_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
		try
		{
			DMS.SCORE.EditChannelNo = (byte)Convert.ToInt32( _ChannelNoComboBox.SelectedItem.ToString() ?? "0" );

			Config.EventChangeChannel();

			//if ( DMS.EditerForm != null )
			//{ 
			//	DMS.EditerForm.MidiMapSetCtl.LoadMidiMapSet( DMS.SCORE.EditMidiMapSet );
			//	DMS.EditerForm.Refresh();
			//}
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
	private void _PlayButton_Click( object sender, RoutedEventArgs args )
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
	private void _StopButton_Click( object sender, RoutedEventArgs args )
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
	private void _LoopPlayButton_Click( object sender, RoutedEventArgs args )
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
    private void _LoopPlayMeasureNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			_SettingMeasureNo();
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
    private void _LoopPlayMeasureConnectToggleSwitch_Toggled( object sender, RoutedEventArgs args )
    {
		try
		{
			_SettingMeasureNo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// 小節番号設定
	/// </summary>
	private void _SettingMeasureNo()
    {
		// 初期設定時のエラー回避
		if ( !IsLoaded )
        {
			return;
        }

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

    #endregion
}
