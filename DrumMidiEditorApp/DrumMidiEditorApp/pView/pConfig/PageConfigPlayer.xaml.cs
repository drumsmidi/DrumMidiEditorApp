using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.ObjectModel;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigPlayer : Page
{
    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayer DrawSet => Config.Player;

	/// <summary>
	/// Media設定
	/// </summary>
	private ConfigMedia ConfigMedia => Config.Media;

	/// <summary>
	/// プレイヤー描画モードリスト
	/// </summary>
	private readonly ObservableCollection<string> _PlayerSurfaceModeList = new();

	/// <summary>
	/// スクリーンサイズリスト
	/// </summary>
	private readonly ObservableCollection<string> _PlayerScreenSizeList = new();

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigPlayer()
    {
		// 初期化
		InitializeComponent();

		#region プレイヤー描画モードリスト作成

		foreach ( var name in Enum.GetNames<ConfigPlayer.PlayerSurfaceMode>() )
		{
			_PlayerSurfaceModeList.Add( name );
		}

        #endregion

        #region スクリーンサイズ作成

        foreach ( var size in DrawSet.ResolutionScreenList )
		{
			_PlayerScreenSizeList.Add( $"{size.Width} x {size.Height}" );
		}

        #endregion
    }

    /// <summary>
    /// プレイヤー描画モード変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerSurfaceModeComboBox_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
		try
		{
			// 初回表示時は処理しない
			if ( !IsLoaded )
            {
				return;
            }

			Config.EventUpdatePlayerMode();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    /// <summary>
    /// プレイヤースクリーンサイズ変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerScreenSizeComboBox_SelectionChanged( object sender, SelectionChangedEventArgs args )
    {
		try
		{
			// 初回表示時は処理しない
			if ( !IsLoaded )
            {
				return;
            }

			Config.EventUpdatePlayerScreenSize();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    /// <summary>
    /// 色選択
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ColorButton_Click( object sender, RoutedEventArgs args )
    {
		try
		{
            if ( sender is not Button item )
            {
                return;
            }

            XamlHelper.ColorDialogAsync
                (
                    Content.XamlRoot,
                    ( item.Background as SolidColorBrush )?.Color ?? ColorHelper.EmptyColor,
                    ( color ) =>
                    {
                        item.Background = new SolidColorBrush( color );
                    }
                );
        }
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
