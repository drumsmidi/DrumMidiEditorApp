﻿using System;
using System.Collections.ObjectModel;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using DrumMidiEditorApp.pUtil;
using DrumMidiEditorApp.pUtil.pHelper;

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
    //private ConfigMedia ConfigMedia => Config.Media;

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

            EventManage.Event_Player_UpdateSufaceMode();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// プレイヤー描画エフェクトモード変更
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerSurfaceEffectModeComboBox_SelectionChanged( object sender, SelectionChangedEventArgs args )
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

            EventManage.Event_Player_UpdateScreenSize();
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
#pragma warning disable IDE0051 // 使用されていないプライベート メンバーを削除する
#pragma warning disable IDE0060 // 未使用のパラメーターを削除します
    private void ColorButton_Click( object sender, RoutedEventArgs args )
#pragma warning restore IDE0060 // 未使用のパラメーターを削除します
#pragma warning restore IDE0051 // 使用されていないプライベート メンバーを削除する
    {
        try
        {
            if ( sender is not Button item )
            {
                return;
            }

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
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
