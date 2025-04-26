using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage.Pickers;

namespace DrumMidiEditorApp.pView.pMusic;

public sealed partial class PageMusic : Page, INotifyPropertyChanged
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PageMusic()
    {
        InitializeComponent();

        ControlAccess.PageMusic = this;
    }

    #region Member

    /// <summary>
    /// スコア情報
    /// </summary>
    private Score MusicInfo => DMS.SCORE;

    /// <summary>
    /// 設定
    /// </summary>
    private ConfigMedia ConfigMedia => Config.Media;

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

            _MusicInfoBpmNumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 2, 0.01 );
            _MusicInfoBgmPlaybackStartPositionNumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 3, 0.001 );
            _MusicInfoBgmVolumeNumberBox.NumberFormatter
                = HelperXaml.CreateNumberFormatter( 1, 0, 1 );

            #endregion
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
            Log.Error( e );
        }
    }

    #endregion

    #region MusicInfo

    /// <summary>
    /// BGMファイルの選択
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MusicInfoBgmFilePathButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        if ( ControlAccess.MainWindow == null )
        {
            return;
        }

        try
        {
            HelperXaml.OpenDialogAsync
                (
                    ControlAccess.MainWindow,
                    Config.File.SupportBgm,
                    PickerLocationId.MusicLibrary,
                    ConfigFile.FolderBgm,
                    ( filepath ) =>
                    {
                        MusicInfo.BgmFilePath.AbsoluteFilePath = filepath.AbsoluteFilePath;

                        ReloadMusicInfo();
                    }
                );
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// BGMファイルパスの変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MusicInfoBgmFilePathTextBox_TextChanged( object aSender, TextChangedEventArgs aArgs )
    {
        try
        {
            EventManage.Event_Score_ReloadBgm();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// BPMの変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MusicInfoBpmNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            EventManage.Event_Score_EditBaseBpm();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// BGM再生開始位置変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MusicInfoBgmPlaybackStartPositionNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            EventManage.Event_Score_EditBgmPlaybackStartPosition();
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    /// <summary>
    /// BGM音量変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void MusicInfoBgmVolumeNumberBox_ValueChanged( NumberBox aSender, NumberBoxValueChangedEventArgs aArgs )
    {
        try
        {
            // 必須入力チェック
            if ( !HelperXaml.NumberBox_RequiredInputValidation( aSender, aArgs ) )
            {
                return;
            }

            Config.Media.BgmVolume = (int)aSender.Value;
        }
        catch ( Exception e )
        {
            Log.Error( e );
        }
    }

    #endregion
}
