using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pIO;
using DrumMidiEditorApp.pGeneralFunction.pAudio;

namespace DrumMidiEditorApp.pView.pEditer;

public sealed partial class WindowEditerMain : Window
{
	/// <summary>
	/// 本ウィンドウへのアクセス
	/// </summary>
	private readonly AppWindow _AppWindow;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public WindowEditerMain()
    {
		// Configファイル読込
		FileIO.LoadConfig();

		// Midiデバイス初期化
		MidiNet.InitDevice();

		// ウィンドウ構築
		InitializeComponent();

		// 自身のウィンドウ情報取得
		_AppWindow = AppWindowHelper.GetAppWindow( this );

		// タイトル初期設定
		Title = $"{Config.System.AppName}";

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
		AppWindowHelper.SetPresenterNormalWindow( _AppWindow );

		// アプリ終了処理
		_AppWindow.Closing += ( sender, args ) =>
        {
            DmsControl.StopPreSequence();
            DmsControl.End();

            FileIO.SaveConfig();
        };

		// 再生コントロール開始
		DmsControl.Start();
	}
}
