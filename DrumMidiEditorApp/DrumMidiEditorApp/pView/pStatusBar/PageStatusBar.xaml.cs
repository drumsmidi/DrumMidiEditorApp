using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using System;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pView.pStatusBar;

public sealed partial class PageStatusBar : Page
{
	/// <summary>
	/// システム設定
	/// </summary>
	private ConfigSystem ConfigSystem => Config.System;

	/// <summary>
	/// コンストラクタ
	/// </summary>
    public PageStatusBar()
    {
        InitializeComponent();

		// ログ出力の通知を受け取る
		Log.LogOutputCallback.Enqueue( SetStatusText );
	}

	#region InfoBar

	/// <summary>
	/// ステータスバーへのログ出力
	/// </summary>
	/// <param name="aLevel">0:Info, 1:Warning, 2:Error</param>
	/// <param name="aText">出力内容</param>
	private void SetStatusText( int aLevel, string aText )
    {
		try
		{
			switch ( aLevel )
			{
				case 0: SetStatusText( "Message", aText, InfoBarSeverity.Informational	); break;
				case 1: SetStatusText( "Message", aText, InfoBarSeverity.Warning		); break;
   				case 2: SetStatusText( "Message", aText, InfoBarSeverity.Error			); break;
			}
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

	/// <summary>
	/// ステータスバーテキスト出力
	/// </summary>
	/// <param name="aTitle">タイトル</param>
	/// <param name="aContent">出力内容</param>
	/// <param name="aSeverity"></param>
	private void SetStatusText( string aTitle, string aContent, InfoBarSeverity aSeverity )
    {
		try
		{
			if ( !DispatcherQueue.HasThreadAccess )
            {
                DispatcherQueue.TryEnqueue
                    (
                        DispatcherQueuePriority.Normal,
                        () => SetStatusText( aTitle, aContent, aSeverity )
                    );

				return;
            }

			_InfoBar.Title		= aTitle;
			_InfoBar.Content	= aContent;
			_InfoBar.Severity	= aSeverity;
			_InfoBar.IsOpen		= true;
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    #endregion
}
