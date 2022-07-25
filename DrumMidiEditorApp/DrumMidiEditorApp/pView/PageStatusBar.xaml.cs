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
using DrumMidiEditorApp.pGeneralFunction.pLog;
using Windows.UI;

namespace DrumMidiEditorApp.pView;

public sealed partial class PageStatusBar : Page
{
	/// <summary>
	/// コンストラクタ
	/// </summary>
    public PageStatusBar()
    {
        InitializeComponent();

		// ログ出力の通知を受け取る
		//Log.LogOutputCallback.Enqueue( SetStatusText );
	}

	#region StatusLabel

	/// <summary>
	/// ステータスバーへのログ出力
	/// </summary>
	/// <param name="aLevel">0:Info, 1:Warning, 2:Error</param>
	/// <param name="aText">出力内容</param>
	private void SetStatusText( int aLevel, string aText )
    {
		switch ( aLevel )
        {
			case 0: SetStatusText( aText, Color.FromArgb( 255,   0,   0, 255 ) ); break;
			case 1: SetStatusText( aText, Color.FromArgb( 255,   0, 255,   0 ) ); break;
   			case 2: SetStatusText( aText, Color.FromArgb( 255, 255,   0,   0 ) ); break;
		}
    }

	/// <summary>
	/// ステータスバーテキスト出力
	/// </summary>
	/// <param name="aText">出力内容</param>
	/// <param name="aColor">文字色</param>
	private void SetStatusText( string aText, Color aColor )
    {
		try
		{
			//MainToolStripStatusLabel.Text		= aText ?? String.Empty ;
			//MainToolStripStatusLabel.ForeColor	= aColor;
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    #endregion
 
    #region LogView
		
	/// <summary>
	/// ログフォームの表示切替
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
    private void LogToolStripSplitButton_ButtonClick( object sender, EventArgs ev )
    {
        try
        {
			//Log.ChangeDisplayLogForm();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

    #endregion
}
