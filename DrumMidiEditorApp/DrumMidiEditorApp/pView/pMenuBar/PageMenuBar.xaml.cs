using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pIO;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;

namespace DrumMidiEditorApp.pView.pMenuBar;

public sealed partial class PageMenuBar : Page, INotifyPropertyChanged
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageMenuBar()
    {
        InitializeComponent();

        ControlAccess.PageMenuBar = this;
    }

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
    /// ページロード完了後処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Page_Loaded( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
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
        catch ( Exception e )
        {
            Log.Error( e );
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
            Log.Error( e );
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
            Log.Info( $"{Log.GetThisMethodName}:{aFilePath.AbsoluteFilePath}" );

            if ( FileIO.LoadScore( aFilePath, out var score ) )
            {
                DMS.SCORE           = score;
                DMS.OpenFilePath    = aFilePath;

                ApplyScore();
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
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
            Log.Info( $"{Log.GetThisMethodName}:{aFilePath.AbsoluteFilePath}" );

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
            Log.Error( e );
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
            Log.Error( e );
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
        => ControlAccess.MainWindow?.SetSubTitle( $"[{DMS.OpenFilePath.AbsoluteFilePath}]" );

    /// <summary>
    /// メニュー：DMS新規作成
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MenuItemNew_Click( object aSender, RoutedEventArgs aArgs )
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
            Log.Error( e );
        }
    }

    /// <summary>
    /// メニュー：DMS開く
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MenuItemOpen_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

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
            Log.Error( e );
        }
    }

    /// <summary>
    /// メニュー：DMS上書き保存
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MenuItemSave_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

        try
        {
            PlayerStop();

            var edit_filepath = new GeneralPath( DMS.OpenFilePath.AbsoluteFilePath );

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
                FileIO.SaveScore( edit_filepath, DMS.SCORE );
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// メニュー：DMS別名保存
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MenuItemSaveAs_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

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
            Log.Error( e );
        }
    }

    /// <summary>
    /// メニュー：Export - Midi
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MenuItemExportMidi_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

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
                    ( filepath ) => FileIO.SaveMidi( filepath, DMS.SCORE )
                );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// メニュー：Export - Video
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MenuItemExportVideo_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

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

                        var cancel = new CancellationTokenSource ();

                        HelperXaml.ProcDialogCancelAsync
                            (
                                Content.XamlRoot,
                                HelperResources.GetString( "DialogMenuItemExportVideo/Title" ),
                                HelperResources.GetString( "DialogMenuItemExportVideo/Content" ),
                                HelperResources.GetString( "Dialog/Cancel" ),
                                () => FileIO.SaveVideoAsync( filepath, cancel.Token ),
                                () => cancel.Cancel()
                            );
                    }
                );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// メニュー：Export - Pdf
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MenuItemExportPdf_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

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
            Log.Error( e );
        }
    }

    /// <summary>
    /// メニュー：Import - Midi
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MenuItemImportMidi_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

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
            Log.Error( e );
        }
    }

    #endregion

    #region Command

    /// <summary>
    /// チャンネルNO切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void ChannelNoComboBox_SelectionChanged( object aSender, SelectionChangedEventArgs aArgs )
    {
        try
        {
            Score.EditChannelNo = (byte)_ChannelNoComboBox.SelectedItem;

            EventManage.Event_Score_ChangeChannel();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// 通常再生
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void PlayButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            DmsControl.PlayPreSequence();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// 再生停止
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void StopButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            DmsControl.StopPreSequence();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ループ再生
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="aArgs"></param>
    private void LoopPlayButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            DmsControl.PlayPreLoopSequence();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ループ再生 小節番号変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void LoopPlayMeasureNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            SettingMeasureNo();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// ループ再生 小節接続切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void LoopPlayMeasureConnectToggleSwitch_Toggled( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            SettingMeasureNo();
        }
        catch ( Exception e )
        {
            Log.Error( e );
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

        start = Math.Clamp( start, min, max );

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
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void PlayerDisplayToggleSwitch_Toggled( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( aSender is ToggleSwitch item )
            {
                EventManage.Event_Player_ChangeDisplay( item?.IsOn ?? false );
            }
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion
}
