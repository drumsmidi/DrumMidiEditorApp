using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pAudio;
using DrumMidiEditorApp.pIO;

namespace DrumMidiEditorApp.pView;

public sealed partial class WindowEditer : Window
{
    #region Member

    /// <summary>
    /// システム設定
    /// </summary>
    private ConfigSystem ConfigSystem => Config.System;

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
		// Configファイル読込
		FileIO.LoadConfig();

		// Midiデバイス初期化
		MidiNet.InitDevice();

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
	}

	/// <summary>
	/// ウィンドウ有効化
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	private void Window_Activated( object sender, WindowActivatedEventArgs args )
    {
		try
		{
			// 再生コントロール開始
			DmsControl.Start();
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
}
