using System;
using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pEvent;
using DrumMidiPlayerApp.pIO;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Windows.Gaming.Input;

namespace DrumMidiPlayerApp.pView;

public sealed partial class WindowPlayer : Window
{
    /// <summary>
    /// 本ウィンドウへのアクセス
    /// </summary>
    private readonly AppWindow _AppWindow;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowPlayer()
    {
        // Midiデバイス初期化
        MidiNet.MidiOutDeviceWatcher();
        MidiNet.InitDeviceAsync( Config.Media.MidiOutDeviceName );

        // ウィンドウ構築
        InitializeComponent();

        // 自身のウィンドウ情報取得
        _AppWindow = this.GetAppWindow();

        // タイトル初期設定
        Title = $"{Config.Window.AppName}";

        // 独自のタイトルバー設定
        ExtendsContentIntoTitleBar = true;
        SetTitleBar( _AppTitleBar );
        SetSubTitle( string.Empty );
    //  SetSubTitle( $"[{DMS.OpenFilePath.AbsoulteFilePath}]" );

        // ウィンドウ初期サイズ変更
        UpdateWindowsSize();

        // 通常ウィンドウのプレゼンター設定
        HelperAppWindow.SetPresenterFixedWindow( _AppWindow );

        ControlAccess.MainWindow = this;

        // 環境音有効化
        SystemSound.SoundOn();

        // 再生コントロール開始
        DmsControl.Start();

        // キーイベントキャプチャ
        _MainGrid.KeyUp   += InputControl.KeyUp;
        _MainGrid.KeyDown += InputControl.KeyDown;

        // ゲームパッド監視
        InputControl.SetGamePadWatcher();

        InputControl.StartTime();
    }

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
            var key =  aArgs.WindowActivationState == WindowActivationState.Deactivated
                ? "WindowCaptionForegroundDisabled" : "WindowCaptionForeground";

            _AppTitleTextBlock.Foreground = (SolidColorBrush)Application.Current.Resources [ key ];
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ウィンドウ終了処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Window_Closed( object aSender, WindowEventArgs aArgs )
    {
        try
        {
            // キーイベントキャプチャ解放
            _MainGrid.KeyUp   -= InputControl.KeyUp;
            _MainGrid.KeyDown -= InputControl.KeyDown;

            // ゲームパッド監視解放
            InputControl.ReleaseGamePadWatcher();

            // 再生コントロール停止
            DmsControl.StopPreSequence();
            DmsControl.End();

            // プレイヤー停止
            ControlAccess.MainPanel?.DrawTaskStop();

            // 設定ファイル保存
            _ = FileIO.SaveConfig();
        }
        catch ( Exception )
        {
            //Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ウィンドウサイズ変更
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void Window_SizeChanged( object aSender, WindowSizeChangedEventArgs aArgs )
    {
        try
        {
            EventManage.Event_Window_ResizeWindow();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// ウィンドウサイズ更新
    /// </summary>
    public void UpdateWindowsSize()
    {
        try
        {
            // NOTE: DPIスケール取得できない？WindowsAPI仕様が必要？
            // DPIスケール取得
            Config.Window.DpiScale = 1.5; // Content.RasterizationScale;

            // パネルの解像度にに合わせてウィンドウサイズを変更する
            Config.Window.WindowSizeWidthDpiNoScale     = (int)Config.Panel.ResolutionScreenWidth;
            Config.Window.WindowSizeHeightDpiNoScale    = (int)Config.Panel.ResolutionScreenHeight;

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
                : $"{Config.Window.AppName}" ;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
