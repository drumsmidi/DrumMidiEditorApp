using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using DrumMidiEditorApp.pConfig;
using Microsoft.UI.Windowing;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pView.pPlayer;

public sealed partial class WindowPlayer : Window
{
	/// <summary>
	/// 描画設定
	/// </summary>
	private ConfigPlayer DrawSet => Config.Player;

	/// <summary>
	/// 本ウィンドウへのアクセス
	/// </summary>
	private readonly AppWindow _AppWindow;

	public WindowPlayer()
    {
        InitializeComponent();

		// 自身のウィンドウ情報取得
		_AppWindow = AppWindowHelper.GetAppWindow(this);

		// 独自のタイトルバー設定
		ExtendsContentIntoTitleBar = true;
		
		SetTitleBar( _AppTitleBar );
		_AppTitleTextBlock.Text = $"Player";

		// ウィンドウ初期サイズ変更
		if ( (int)DrawSet.ResolutionScreenWidth > 0 && (int)DrawSet.ResolutionScreenHeight > 0 )
		{
			AppWindowHelper.ResizeWindow
				(
					_AppWindow,
					(int)DrawSet.ResolutionScreenWidth,
					(int)DrawSet.ResolutionScreenHeight
				);
		}

		// ウィンドウを閉じれないように設定
		_AppWindow.Closing += ( sender, args ) =>
        {
			args.Cancel = true;

			Hide();
		};

		// 通常ウィンドウのプレゼンター設定
		AppWindowHelper.SetPresenterPlayerWindow( _AppWindow );
	}

	public void Exit()
    {
        try
        {
			_AppWindow.Destroy();
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

	public void Show()
    {
        try
        {
			if ( !_AppWindow.IsVisible )
			{
				_AppWindow.Show();
				DrawSet.DisplayPlayer = true;
			}
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	public void Hide()
    {
        try
        {
			if ( _AppWindow.IsVisible )
			{
				_AppWindow.Hide();
				DrawSet.DisplayPlayer = false;
			}
		}
		catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

    #region Playerの一時非表示

    /// <summary>
    /// プレイヤー一時非表示前 表示状態
    /// </summary>
    private bool _PlayerStopBefore = false;

	/// <summary>
	/// プレイヤー一時非表示前 表示状態
	/// </summary>
	private bool _PlayerConfigStopBefore = false;

	/// <summary>
	/// プレイヤーフォーム再表示
	/// </summary>
	public void TemporaryShow()
	{
		Config.Player.DisplayPlayer			= _PlayerStopBefore;
		Config.Player.DisplayPlayerConfig	= _PlayerConfigStopBefore;
	}

	/// <summary>
	/// プレイヤーフォーム一時非表示
	/// </summary>
	public void TemporaryHide()
	{
		_PlayerStopBefore		= Config.Player.DisplayPlayer;
		_PlayerConfigStopBefore = Config.Player.DisplayPlayerConfig;

		Config.Player.DisplayPlayer			= false;
		Config.Player.DisplayPlayerConfig	= false;
	}

    #endregion
}
