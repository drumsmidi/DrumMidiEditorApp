﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace DrumMidiEditorApp.pView.pConfig;

public sealed partial class PageConfigPlayerSequence : Page, INotifyPropertyChanged
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageConfigPlayerSequence()
    {
        InitializeComponent();

        #region 描画方向リスト作成

        foreach ( var name in Enum.GetNames<ConfigPlayerSequence.DrawDirectionMode>() )
        {
            _DrawDirectionModeList.Add( name );
        }

        #endregion

        #region NumberBox の入力書式設定

        _NoteTermHeightNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _NoteTermWidthNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _NoteHeightNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _NoteWidthNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );

        _Line128NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line064NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line032NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line016NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line008NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line004NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );
        _Line001NumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 1, 0.1 );

        #endregion
    }

    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private ConfigPlayerSequence DrawSet => Config.Player.Sequence;

    /// <summary>
    /// 描画方向リスト
    /// </summary>
    private readonly ObservableCollection<string> _DrawDirectionModeList = [];

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// Config再読み込み
    /// 
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadConfig()
    {
        try
        {
            OnPropertyChanged( "DrawSet" );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion

    /// <summary>
    /// 描画方向変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void DrawDirectionModeComboBox_SelectionChanged( object aSender, SelectionChangedEventArgs aArgs )
    {
        try
        {
            EventManage.Event_Player_UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// 共通：トグル切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ToggleSwitch_Toggled( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            EventManage.Event_Player_UpdateScore();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ノートサイズ変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void SizeNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            EventManage.Event_Player_UpdateScore();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void HeaderGroupToggleSwitch_Toggled( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( aSender is ToggleSwitch item )
            {
                DrawSet.HeaderGroupOn = item.IsOn;

                ReloadConfig();
                EventManage.Event_Player_UpdateScore();
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }
}
