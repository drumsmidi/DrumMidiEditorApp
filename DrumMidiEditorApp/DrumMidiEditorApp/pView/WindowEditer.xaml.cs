using System;
using DrumMidiEditorApp.pAudio;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pLog;
using DrumMidiEditorApp.pIO;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using DrumMidiEditorApp.pUtil;

namespace DrumMidiEditorApp.pView;

public sealed partial class WindowEditer : Window
{
    #region Member

    /// <summary>
    /// システム設定
    /// </summary>
    private static ConfigSystem ConfigSystem => Config.System;

    /// <summary>
    /// 本ウィンドウへのアクセス
    /// </summary>
    private readonly AppWindow _AppWindow;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowEditer()
    {
#if !DEBUG_DEFAULT
        // Configファイル読込
        FileIO.LoadConfig();
#endif

        // Midiデバイス初期化
        MidiNet.InitDevice( Config.Media.MidiOutDeviceName );

        // ウィンドウ構築
        InitializeComponent();

        // 自身のウィンドウ情報取得
        _AppWindow = AppWindowHelper.GetAppWindow( this );

        // タイトル初期設定
        Title = $"{ConfigSystem.AppName}";

        // 独自のタイトルバー設定
        ExtendsContentIntoTitleBar = true;
        SetTitleBar( _AppTitleBar );
        SetSubTitle( "[]" );

        // ウィンドウ初期サイズ変更
        if ( ConfigSystem.WindowSizeWidth > 0 && ConfigSystem.WindowSizeHeight > 0 )
        {
            AppWindowHelper.ResizeWindow
                (
                    _AppWindow,
                    ConfigSystem.WindowSizeWidth,
                    ConfigSystem.WindowSizeHeight
                );
        }

        // 通常ウィンドウのプレゼンター設定
        //AppWindowHelper.SetPresenterNormalWindow( _AppWindow );

        ControlAccess.MainWindow = this;

        // 再生コントロール開始
        DmsControl.Start();

        // プレイヤーリクエスト通知
        DmsControl.SetPlayerRequestCallback = ( request ) => ConfigLocal.Player.PlayReq = request;
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
            var key = ( args.WindowActivationState == WindowActivationState.Deactivated )
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
            _AppTitleTextBlock.Text = $"{ConfigSystem.AppName} - {aSubTitle}";
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
