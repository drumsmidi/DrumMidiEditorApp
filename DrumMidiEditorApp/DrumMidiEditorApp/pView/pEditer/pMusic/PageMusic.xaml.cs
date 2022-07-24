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

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pEditer.pMusic;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PageMusic : Page
{
	/// <summary>
	/// スコア情報
	/// </summary>
	private Score _MusicInfo => DMS.SCORE;

	/// <summary>
	/// 設定
	/// </summary>
	private ConfigMedia _ConfigMedia => Config.Media;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public PageMusic()
    {
        InitializeComponent();

		// NumberBox の入力書式設定
		_MusicInfoBpmNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 2, 1 );
		_MusicInfoBgmPlaybackStartPositionNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 0, 100 );
		_MusicInfoBgmVolumeNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 0, 10 );
	}

    #region MusicInfo

	/// <summary>
	/// BGM再読込
	/// </summary>
	private static void UpdateMusicInfo() { } // => Config.EventReloadBgm();

	/// <summary>
	/// BGMファイルの選択
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void _MusicInfoBgmFilePathButton_Click( object sender, RoutedEventArgs args )
	{
		try
		{
            //DMS.PlayerForm?.TemporaryHide();

			//var filepath = new GeneralPath();

   //         XamlUtil.OpenShowDialog( Config.System.SupportBgm, filepath );

			//if ( !filepath.IsExistFile )
			//{
			//	return;
			//}

			//_MusicInfoBgmFilePathTextBox.Text = filepath.RelativeFilePath ?? String.Empty ;

			//_MusicInfo.BgmFilePath = new( filepath );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
			//DMS.PlayerForm?.TemporaryShow();
		}
	}

	/// <summary>
	/// BGMファイルパスの変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void _MusicInfoBgmFilePathTextBox_TextChanged( object sender, TextChangedEventArgs args )
    {
		try
		{
			UpdateMusicInfo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// BPMの変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void _MusicInfoBpmNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			UpdateMusicInfo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}

    }

	/// <summary>
	/// BGM再生開始位置の変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void _MusicInfoBgmPlaybackStartPositionNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
    {
		try
		{
			// 必須入力チェック
			if ( !XamlHelper.NumberBox_RequiredInputValidation( sender, args ) )
            {
				return;
            }

			UpdateMusicInfo();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// BGM音量の変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void _MusicInfoBgmVolumeNumberBox_ValueChanged(　NumberBox sender, NumberBoxValueChangedEventArgs args )
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
