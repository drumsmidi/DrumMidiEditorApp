using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage.Pickers;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pMusic;

public sealed partial class PageMusic : Page, INotifyPropertyChanged
{
    #region Member

    /// <summary>
    /// スコア情報
    /// </summary>
    private Score MusicInfo => DMS.SCORE;

	/// <summary>
	/// 設定
	/// </summary>
	private ConfigMedia ConfigMedia => Config.Media;

	/// <summary>
	/// 設定
	/// </summary>
	private ConfigSystem ConfigSystem => Config.System;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageMusic()
    {
        InitializeComponent();

		ControlAccess.PageMusic = this;

		#region NumberBox の入力書式設定

		_MusicInfoBpmNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 2, 1 );
		_MusicInfoBgmPlaybackStartPositionNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 3, 0.01 );
		_MusicInfoBgmVolumeNumberBox.NumberFormatter 
			= XamlHelper.CreateNumberFormatter( 1, 0, 10 );

        #endregion
	}

	#region INotifyPropertyChanged

	/// <summary>
	/// MusicInfo再読み込み
	/// 
	/// x:Bind OneWay/TwoWay 再読み込み
	/// </summary>
	public void ReloadMusicInfo()
    {
		try
		{
			OnPropertyChanged( "MusicInfo" );
		}
        catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	public event PropertyChangedEventHandler? PropertyChanged = delegate { };

	public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
		=> PropertyChanged?.Invoke( this, new( aPropertyName ) );

	#endregion

	#region MusicInfo

	/// <summary>
	/// BGM再読込
	/// </summary>
	private static void UpdateMusicInfo() => Config.EventReloadBgm();

	/// <summary>
	/// BGMファイルの選択
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void MusicInfoBgmFilePathButton_Click( object sender, RoutedEventArgs args )
	{
		try
		{
			XamlHelper.OpenDialogAsync
				(
					ControlAccess.MainWindow,
					ConfigSystem.SupportBgm,
					PickerLocationId.MusicLibrary,
					ConfigSystem.FolderBgm,
					( filepath ) =>
                    {
						MusicInfo.BgmFilePath.AbsoulteFilePath = filepath.AbsoulteFilePath;

						ReloadMusicInfo();
					}
				);
		}
        catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

	/// <summary>
	/// BGMファイルパスの変更
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void MusicInfoBgmFilePathTextBox_TextChanged( object sender, TextChangedEventArgs args )
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
    private void MusicInfoBpmNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
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
	private void MusicInfoBgmPlaybackStartPositionNumberBox_ValueChanged( NumberBox sender, NumberBoxValueChangedEventArgs args )
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
    private void MusicInfoBgmVolumeNumberBox_ValueChanged(　NumberBox sender, NumberBoxValueChangedEventArgs args )
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
