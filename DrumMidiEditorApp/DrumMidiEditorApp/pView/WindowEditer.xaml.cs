using System;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pIO;
using DrumMidiEditorApp.pModel;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace DrumMidiEditorApp.pView;

public sealed partial class WindowEditer : Window
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowEditer()
    {
        // Midiデバイス初期化
        MidiNet.MidiOutDeviceWatcher();
        _ = MidiNet.InitDeviceAsync( Config.Media.MidiOutDeviceName );

        // ウィンドウ構築
        InitializeComponent();

        // 自身のウィンドウ情報取得
        _AppWindow = HelperAppWindow.GetAppWindow( this );

        // タイトル初期設定
        Title = $"{Config.Window.AppName}";

        // 独自のタイトルバー設定
        ExtendsContentIntoTitleBar = true;
        SetTitleBar( _AppTitleBar );
        SetSubTitle( $"[{DMS.OpenFilePath.AbsoluteFilePath}]" );

        // ウィンドウ初期サイズ変更
        UpdateWindowsSize();

        // 通常ウィンドウのプレゼンター設定
        //HelperAppWindow.SetPresenterNormalWindow( _AppWindow );

        ControlAccess.MainWindow = this;

        // 再生コントロール開始
        DmsControl.Start();
    }

    #region member

    /// <summary>
    /// 本ウィンドウへのアクセス
    /// </summary>
    private readonly AppWindow _AppWindow;

    #endregion

    /// <summary>
    /// アクティブ状態更新
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Window_Activated( object aSender, WindowActivatedEventArgs aArgs )
    {
        try
        {
            // タイトルバーが非アクティブ状態による前景色の変更
            var key = ( aArgs.WindowActivationState == WindowActivationState.Deactivated )
                ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

            _AppTitleTextBlock.Foreground = (SolidColorBrush)App.Current.Resources [ key ];
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ウィンドウ終了処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="aArgs"></param>
    private void Window_Closed( object aSender, WindowEventArgs aArgs )
    {
        try
        {
            // 再生コントロール停止
            DmsControl.StopPreSequence();
            DmsControl.End();

            // プレイヤー停止
            ControlAccess.UCPlayerPanel?.DrawTaskStop();

            // 設定ファイル保存
            _ = FileIO.SaveConfig();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
        finally
        {
            // トレースログファイルを開く
            Log.OpenLogFile();
        }
    }

    /// <summary>
    /// ウィンドウサイズ更新
    /// </summary>
    public void UpdateWindowsSize()
    {
        try
        {
            // TASK: DPIスケール取得できない？WindowsAPI仕様が必要？
            // DPIスケール取得
            Config.Window.SetDpiScale( 1.5 ); // Content.RasterizationScale;

            var width  = Config.Window.WindowSizeWidthDpiScale;
            var height = Config.Window.WindowSizeHeightDpiScale;

            if ( width > 0 && height > 0 )
            {
                HelperAppWindow.ResizeWindowClient( _AppWindow, width, height );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// タイトルバーのサブタイトルを設定
    /// </summary>
    /// <param name="aSubTitle">サブタイトル</param>
    public void SetSubTitle( string aSubTitle )
    {
        try
        {
            _AppTitleTextBlock.Text = aSubTitle.Length != 0
                ? $"{Config.Window.AppName} - {aSubTitle}"
                : $"{Config.Window.AppName}";
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
