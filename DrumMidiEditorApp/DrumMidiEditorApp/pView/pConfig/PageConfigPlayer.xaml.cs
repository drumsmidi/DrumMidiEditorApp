using System;
using System.Collections.ObjectModel;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigPlayer : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigPlayer()
    {
        InitializeComponent();

        #region プレイヤー描画モードリスト作成

        foreach ( var name in Enum.GetNames<ConfigSystem.PlayerSurfaceMode>() )
        {
            _PlayerSurfaceModeList.Add( name );
        }

        #endregion

        #region プレイヤー描画エフェクトモードリスト作成

        foreach ( var name in Enum.GetNames<ConfigPlayer.PlayerSurfaceEffectMode>() )
        {
            _PlayerSurfaceEffectModeList.Add( name );
        }

        #endregion

        #region スクリーンサイズ作成

        foreach ( var size in DrawSet.ResolutionScreenList )
        {
            _PlayerScreenSizeList.Add( $"{size.Width} x {size.Height}" );
        }

        #endregion
    }

    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayer DrawSet => Config.Player;

    /// <summary>
    /// プレイヤー描画モードリスト
    /// </summary>
    private readonly ObservableCollection<string> _PlayerSurfaceModeList = [];

    /// <summary>
    /// プレイヤー描画エフェクトモードリスト
    /// </summary>
    private readonly ObservableCollection<string> _PlayerSurfaceEffectModeList = [];

    /// <summary>
    /// スクリーンサイズリスト
    /// </summary>
    private readonly ObservableCollection<string> _PlayerScreenSizeList = [];

    #endregion

    /// <summary>
    /// プレイヤー描画モード変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void PlayerSurfaceModeComboBox_SelectionChanged( object aSender, SelectionChangedEventArgs aArgs )
    {
        try
        {
            // 初回表示時は処理しない
            if ( !IsLoaded )
            {
                return;
            }

            EventManage.Event_Player_UpdateSufaceMode();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// プレイヤー描画エフェクトモード変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void PlayerSurfaceEffectModeComboBox_SelectionChanged( object aSender, SelectionChangedEventArgs aArgs )
    {
        try
        {
            // 初回表示時は処理しない
            if ( !IsLoaded )
            {
                return;
            }

            EventManage.Event_Player_UpdateSufaceMode();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// プレイヤースクリーンサイズ変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void PlayerScreenSizeComboBox_SelectionChanged( object aSender, SelectionChangedEventArgs aArgs )
    {
        try
        {
            // 初回表示時は処理しない
            if ( !IsLoaded )
            {
                return;
            }

            EventManage.Event_Player_UpdateScreenSize();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// 色選択
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ColorButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( aSender is Button item )
            {
                HelperXaml.ColorDialog
                    (
                        item,
                        ( item.Background as SolidColorBrush )?.Color ?? HelperColor.EmptyColor,
                        ( color ) =>
                        {
                            item.Background = new SolidColorBrush( color );
                        }
                    );
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}
