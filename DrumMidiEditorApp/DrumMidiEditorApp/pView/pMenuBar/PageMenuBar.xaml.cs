using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pModel;
using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pIO;
using DrumMidiEditorApp.pUtil;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage.Pickers;

namespace DrumMidiEditorApp.pView.pMenuBar;

public sealed partial class PageMenuBar : Page, INotifyPropertyChanged
{
    #region Member

    /// <summary>
    /// メディア設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

    /// <summary>
    /// メディア設定
    /// </summary>
    private byte ChannelDrum => MidiNet.ChannelDrum;

    /// <summary>
    /// メディア設定
    /// </summary>
    private List<byte> ChannelNoList => MidiNet.ChannelNoList;

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

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageMenuBar()
    {
        InitializeComponent();

        ControlAccess.PageMenuBar = this;

        #region NumberBox の入力書式設定

        _LoopPlayMeasureStartNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 0, 1 );
        _LoopPlayMeasureEndNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 0, 1 );
        _LoopPlayMeasureConnectNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 0, 1 );
        _LoopPlayMeasureStartNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 0, 1 );
        _LoopPlayMeasureEndNumberBox.NumberFormatter
            = HelperXaml.CreateNumberFormatter( 1, 0, 1 );

        #endregion

        if ( DMS.AppStartDmsPath.IsExistFile )
        {
            LoadSocre( DMS.AppStartDmsPath );
        }
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
        => PropertyChanged?.Invoke( this, new( aPropertyName ) );

    /// <summary>
    /// ConfigPlayer設定再読み込み
    /// 
    /// x:Bind OneWay/TwoWay 再読み込み
    /// </summary>
    public void ReloadConfigPlayer()
    {
        try
        {
            OnPropertyChanged( "ConfigPlayer" );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    /// <summary>
    /// スコアファイルを読込
    /// </summary>
    /// <param name="aFilePath">スコアファイルパス</param>
    private void LoadSocre( GeneralPath aFilePath )
    {
        try
        {
            Log.Info( $"{Log.GetThisMethodName}:{aFilePath.AbsoulteFilePath}" );

            if ( FileIO.LoadScore( aFilePath, out var score ) )
            {
                DMS.SCORE           = score;
                DMS.OpenFilePath    = aFilePath;

                ApplyScore();
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// スコアファイルを保存
    /// </summary>
    /// <param name="aFilePath">スコアファイルパス</param>
    private void SaveSocre( GeneralPath aFilePath )
    {
        try
        {
            Log.Info( $"{Log.GetThisMethodName}:{aFilePath.AbsoulteFilePath}" );

            aFilePath.Extension = ConfigFile.ExtentionDms;

            if ( !FileIO.SaveScore( aFilePath, DMS.SCORE ) )
            {
                return;
            }

            DMS.OpenFilePath = aFilePath;

            SetSubTitle();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// スコアをシステム全体に反映
    /// </summary>
    private void ApplyScore()
    {
        try
        {
            Score.EditChannelNo = _ChannelNoComboBox.SelectedValue != null
                ? Convert.ToByte( _ChannelNoComboBox.SelectedValue.ToString() )
                : MidiNet.ChannelDrum;

            SetSubTitle();

            EventManage.Event_Score_ReloadScore();
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
        => DmsControl.StopPreSequence();

    /// <summary>
    /// タイトルバーに編集中のファイル名を設定
    /// </summary>
    private void SetSubTitle()
        => ControlAccess.MainWindow?.SetSubTitle( $"[{DMS.OpenFilePath.AbsoulteFilePath}]" );

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

            HelperXaml.MessageDialogYesNoAsync
                (
                    Content.XamlRoot,
                    HelperResources.GetString( "DialogMenuItemNew/Title" ),
                    HelperResources.GetString( "DialogMenuItemNew/Content" ),
                    HelperResources.GetString( "Dialog/Yes" ),
                    HelperResources.GetString( "Dialog/No" ),
                    new( () =>
                        {
                            DMS.SCORE           = new();
                            DMS.OpenFilePath    = new();

                            ApplyScore();
                        } )
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

            HelperXaml.OpenDialogAsync
                (
                    ControlAccess.MainWindow,
                    Config.File.SupportDmsOpen,
                    PickerLocationId.DocumentsLibrary,
                    ConfigFile.FolderDms,
                    ( filepath ) => LoadSocre( filepath )
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

            var edit_filepath = new GeneralPath( DMS.OpenFilePath.AbsoulteFilePath );

            if ( !edit_filepath.IsExistFile )
            {
                HelperXaml.SaveDialogAsync
                    (
                        ControlAccess.MainWindow,
                        Config.File.SupportDmsSave,
                        edit_filepath.FileNameWithoutExtension,
                        PickerLocationId.DocumentsLibrary,
                        ConfigFile.FolderDms,
                        ( filepath ) => SaveSocre( filepath )
                    );
            }
            else
            {
                if ( !FileIO.SaveScore( edit_filepath, DMS.SCORE ) )
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

            HelperXaml.SaveDialogAsync
                (
                    ControlAccess.MainWindow,
                    Config.File.SupportDmsSave,
                    DMS.OpenFilePath.FileNameWithoutExtension,
                    PickerLocationId.DocumentsLibrary,
                    ConfigFile.FolderDms,
                    ( filepath ) => SaveSocre( filepath )
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

            HelperXaml.SaveDialogAsync
                (
                    ControlAccess.MainWindow,
                    Config.File.SupportMidi,
                    DMS.OpenFilePath.FileNameWithoutExtension,
                    PickerLocationId.DocumentsLibrary,
                    ConfigFile.FolderExport,
                    ( filepath ) =>
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

            HelperXaml.SaveDialogAsync
                (
                    ControlAccess.MainWindow,
                    Config.File.SupportVideo,
                    DMS.OpenFilePath.FileNameWithoutExtension,
                    PickerLocationId.DocumentsLibrary,
                    ConfigFile.FolderExport,
                    ( filepath ) =>
                    {
                        filepath.Extension = ConfigFile.ExtentionMp4;

                        // TODO: 非同期処理時の操作制限の実装が必要
                        FileIO.SaveVideoAsync( filepath );
                    }
                );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// メニュー：Export - Pdf
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MenuItemExportPdf_Click( object sender, RoutedEventArgs args )
    {
        try
        {
            PlayerStop();

            HelperXaml.SaveDialogAsync
                (
                    ControlAccess.MainWindow,
                    Config.File.SupportPdf,
                    DMS.OpenFilePath.FileNameWithoutExtension,
                    PickerLocationId.DocumentsLibrary,
                    ConfigFile.FolderExport,
                    ( filepath ) =>
                    {
                        filepath.Extension = ConfigFile.ExtentionPdf;

                        FileIO.SavePdf( filepath );
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

            HelperXaml.OpenDialogAsync
                (
                    ControlAccess.MainWindow,
                    Config.File.SupportMidi,
                    PickerLocationId.DocumentsLibrary,
                    ConfigFile.FolderMidi,
                    ( filepath ) =>
                    {
                        var page = new PageImportMidi
                        {
                            BpmZoom = ConfigMedia.MidiImportZoom
                        };

                        HelperXaml.InputDialogOkCancelAsync
                            (
                                Content.XamlRoot,
                                HelperResources.GetString( "LabelImportMidi" ),
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
            Score.EditChannelNo = (byte)_ChannelNoComboBox.SelectedItem;

            EventManage.Event_Score_ChangeChannel();
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
            if ( !HelperXaml.NumberBox_RequiredInputValidation( sender, args ) )
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

        var start   = (int)_LoopPlayMeasureStartNumberBox.Value;
        var end     = (int)_LoopPlayMeasureEndNumberBox.Value;
        var conn    = (int)_LoopPlayMeasureConnectNumberBox.Value;
        var min     = (int)_LoopPlayMeasureStartNumberBox.Minimum;
        var max     = (int)_LoopPlayMeasureEndNumberBox.Maximum;
        var on      = _LoopPlayMeasureConnectToggleSwitch.IsOn;

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

        _LoopPlayMeasureStartNumberBox.Value = start;
        _LoopPlayMeasureEndNumberBox.Value   = end;
    }

    /// <summary>
    /// プレイヤー表示切替
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerDisplayToggleSwitch_Toggled( object sender, RoutedEventArgs args )
    {
        try
        {
            if ( sender is not ToggleSwitch item )
            {
                return;
            }

            EventManage.Event_Player_ChangeDisplay( item?.IsOn ?? false );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
