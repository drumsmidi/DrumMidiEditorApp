using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;
using Microsoft.UI.Xaml.Documents;

namespace DrumMidiEditorApp.pGeneralFunction.pLog;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class WindowLog : Window
{
	/// <summary>
	/// 本ウィンドウへのアクセス
	/// </summary>
    private readonly AppWindow _AppWindow;

	/// <summary>
	/// ログテキストリスト
	/// </summary>
	private readonly List<string> _LogTextLines = new();

	/// <summary>
	/// コンストラクタ
	/// </summary>
    public WindowLog()
    {
        InitializeComponent();

		_AppWindow = AppWindowHelper.GetAppWindow( this );

		// 独自のタイトルバー設定
		// ExtendsContentIntoTitleBar = true;
		
		// SetTitleBar( _AppTitleBar );
		// _AppTitleTextBlock.Text = $"Player";

		//// ウィンドウ初期サイズ変更
		//if ( (int)DrawSet.ResolutionScreenWidth > 0 && (int)DrawSet.ResolutionScreenHeight > 0 )
		//{
		//	AppWindowHelper.ResizeWindow
		//		(
		//			_AppWindow,
		//			(int)DrawSet.ResolutionScreenWidth,
		//			(int)DrawSet.ResolutionScreenHeight
		//		);
		//}

		// ウィンドウを閉じれないように設定
		_AppWindow.Closing += ( sender, args ) =>
        {
			args.Cancel = true;

			SetDisplay( false );
		};
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

	/// <summary>
	/// ログ追加
	/// </summary>
	/// <param name="aText">追加テキスト</param>
    public void AddLog( string aText )
    {
		try
		{
			if ( !DispatcherQueue.HasThreadAccess )
            {
                var isQueued = DispatcherQueue.TryEnqueue
					(
						Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
						() => AddLog( aText )
					);

				return;
            }

			_LogTextLines.Add( aText ?? String.Empty );

			// ログ出力行数上限チェック
			int lineLimitCnt	= Config.System.LogOutputLineCount;
			int lineCnt			= _LogTextLines.Count;

			if ( lineCnt > lineLimitCnt )
			{ 
				_LogTextLines.RemoveRange( 0, lineCnt - lineLimitCnt );
			}

			// ログ出力
			var outputText = "";

			foreach ( var line in _LogTextLines )
            {
				outputText += line + Environment.NewLine;
            }

			_LogTextBox.Text = outputText;
			_LogTextBox.Select( _LogTextBox.ContentEnd, _LogTextBox.ContentEnd );
		}
		catch ( Exception )
		{
		}
    }

	/// <summary>
	/// ウィンドウの表示／非表示切替
	/// </summary>
	/// <param name="aDisplay">True:表示、False:非表示</param>
    public void SetDisplay( bool aDisplay )
    {
		try
		{
			if ( !DispatcherQueue.HasThreadAccess )
            {
                var isQueued = DispatcherQueue.TryEnqueue
					(
						Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
						() => SetDisplay( aDisplay )
					);

				return;
            }

            if ( aDisplay && !_AppWindow.IsVisible )
            {
				_AppWindow.Show();
			}
			else if ( !aDisplay && _AppWindow.IsVisible )
            {
				_AppWindow.Hide();
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }
}
