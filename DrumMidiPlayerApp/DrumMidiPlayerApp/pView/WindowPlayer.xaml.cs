using DrumMidiLibrary.pAudio;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pIO;
using DrumMidiPlayerApp.pModel;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;

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
        SetSubTitle( $"[{DMS.OpenFilePath.AbsoulteFilePath}]" );

        // ウィンドウ初期サイズ変更
        UpdateWindowsSize();

        // 通常ウィンドウのプレゼンター設定
        HelperAppWindow.SetPresenterFixedWindow( _AppWindow );

        ControlAccess.MainWindow = this;

        // 再生コントロール開始
        DmsControl.Start();
    }

    /// <summary>
    /// アクティブ状態更新
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Window_Activated( object sender, WindowActivatedEventArgs args )
    {
        try
        {
            // タイトルバーが非アクティブ状態による前景色の変更
            var key =  args.WindowActivationState == WindowActivationState.Deactivated
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
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Window_Closed( object sender, WindowEventArgs args )
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
        catch ( Exception )
        {
            //Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
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
            _AppTitleTextBlock.Text = $"{Config.Window.AppName} - {aSubTitle}";
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
            if ( Config.Window.WindowSizeWidth > 0 && Config.Window.WindowSizeHeight > 0 )
            {
                HelperAppWindow.ResizeWindow
                    (
                        _AppWindow,
                        Config.Window.WindowSizeWidth,
                        Config.Window.WindowSizeHeight
                    );
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
