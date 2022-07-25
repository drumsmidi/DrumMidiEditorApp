using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pIO;
using DrumMidiEditorApp.pGeneralFunction.pAudio;
using DrumMidiEditorApp.pView.pPlayer;
using Windows.ApplicationModel;

namespace DrumMidiEditorApp.pView;

public sealed partial class WindowEditer : Window
{
	/// <summary>
	/// 本ウィンドウへのアクセス
	/// </summary>
	private readonly AppWindow _AppWindow;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public WindowEditer()
    {
		// Configファイル読込
		//FileIO.LoadConfig();

		// Midiデバイス初期化
		MidiNet.InitDevice();

		// ウィンドウ構築
		InitializeComponent();

		// 自身のウィンドウ情報取得
		_AppWindow = AppWindowHelper.GetAppWindow( this );

		// タイトル初期設定
		Title = $"{Config.System.AppName}";

		// 独自のタイトルバー設定
		ExtendsContentIntoTitleBar = true;
		SetTitleBar( _AppTitleBar );
		_AppTitleTextBlock.Text = $"{Config.System.AppName}";

		// ウィンドウ初期サイズ変更
		if ( Config.System.WindowSizeWidth > 0 && Config.System.WindowSizeHeight > 0 )
		{ 
			AppWindowHelper.ResizeWindow
				(
					_AppWindow, 
					Config.System.WindowSizeWidth, 
					Config.System.WindowSizeHeight 
				);
		}

		// 通常ウィンドウのプレゼンター設定
		//AppWindowHelper.SetPresenterNormalWindow( _AppWindow );
	}

    private void Window_Activated( object sender, WindowActivatedEventArgs args )
    {
		// 再生コントロール開始
		DmsControl.Start();
    }

	/// <summary>
	/// アプリ終了処理
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
    private void Window_Closed( object sender, WindowEventArgs args )
    {
		// 再生コントロール停止
		DmsControl.StopPreSequence();
		DmsControl.End();

		// 設定ファイル保存
		FileIO.SaveConfig();
	}
}
