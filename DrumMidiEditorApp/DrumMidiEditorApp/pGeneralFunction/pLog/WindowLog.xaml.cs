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
    private readonly AppWindow _ThisAppWindow;

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

        _ThisAppWindow = AppWindowHelper.GetAppWindow( this );
    }

	/// <summary>
	/// ログ追加
	/// </summary>
	/// <param name="aText">追加テキスト</param>
    public void AddLog( string aText )
    {
		// 必要かも？
		//if (this.DispatcherQueue.HasThreadAccess)
		//{
		//	StatusBlock.Text = strMessage;
		//}
		//else
		//{
		//	bool isQueued = this.DispatcherQueue.TryEnqueue(
		//	Microsoft.UI.Dispatching.DispatcherQueuePriority.Normal,
		//	() => StatusBlock.Text = strMessage);
		//}

		try
		{
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
			if ( aDisplay && !_ThisAppWindow.IsVisible )
            {
				_ThisAppWindow.Show();
			}
			else if ( !aDisplay && _ThisAppWindow.IsVisible )
            {
				_ThisAppWindow.Hide();
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }
}
