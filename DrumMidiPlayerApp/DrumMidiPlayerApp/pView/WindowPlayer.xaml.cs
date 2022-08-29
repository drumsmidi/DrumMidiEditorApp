using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Windowing;
using System;

using DrumMidiClassLibrary.pAudio;
using DrumMidiClassLibrary.pConfig;
using DrumMidiClassLibrary.pControl;
using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pWinUI;

using DrumMidiPlayerApp.pIO;
using DrumMidiPlayerApp.pConfig;
using DrumMidiClassLibrary.pModel;
using DrumMidiPlayerApp.pEvent;
using DrumMidiClassLibrary.pUtil;

namespace DrumMidiPlayerApp.pView;

public sealed partial class WindowPlayer : Window
{
    #region Member

    /// <summary>
    /// 描画設定
    /// </summary>
    private static ConfigPlayer DrawSet => ConfigLocal.Player;

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
    public WindowPlayer()
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
        UpdatePanelSize();

		// 通常ウィンドウのプレゼンター設定
		//AppWindowHelper.SetPresenterNormalWindow( _AppWindow );

		ControlAccess.MainWindow = this;

		// 再生コントロール開始
		DmsControl.Start();

        // プレイヤーリクエスト通知
        DmsControl.SetPlayerRequestCallback = ( request ) => ConfigLocal.Player.PlayReq = request;



#if DEBUG
        var filepath = new GeneralPath( "D:/CreateGame/DrumMidiEditor/build/net6.0-windows10.0.19041.0/Dms/test.dms" );

		FileIO.LoadScore( filepath, out var score );
		//FileIO.SaveScore( filepath, score );

		DMS.SCORE			= score;
		DMS.OpenFilePath	= filepath;

        DMS.SCORE.EditChannelNo = MidiNet.ChannelDrum;

        EventManage.EventReloadScore();

        DmsControl.PlayPreSequence();

#endif

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

            _AppTitleTextBlock.Foreground = (SolidColorBrush)App.Current.Resources[ key ];
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
			ControlAccess.PanelPlayer?.DrawTaskStop();

			// 設定ファイル保存
			FileIO.SaveConfig();
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

    /// <summary>
    /// パネルサイズ更新
    /// </summary>
    public void UpdatePanelSize()
    {
        try
        {
		    if ( DrawSet.ResolutionScreenWidth > 0 && DrawSet.ResolutionScreenHeight > 0 )
		    { 
			    AppWindowHelper.ResizeWindow
				    (
					    _AppWindow,
                        (int)DrawSet.ResolutionScreenWidth,
                        (int)DrawSet.ResolutionScreenHeight
                    );
		    }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
