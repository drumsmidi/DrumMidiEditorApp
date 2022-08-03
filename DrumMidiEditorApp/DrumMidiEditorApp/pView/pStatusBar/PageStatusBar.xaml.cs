using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pLog;

namespace DrumMidiEditorApp.pView.pStatusBar;

public sealed partial class PageStatusBar : Page, INotifyPropertyChanged
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

		ControlAccess.PageStatusBar = this;

		// ログ出力の通知を受け取る
		Log.LogOutputCallback.Enqueue( SetStatusText );
	}

	#region INotifyPropertyChanged

	/// <summary>
	/// x:Bind OneWay/TwoWay 再読み込み
	/// </summary>
	public void ReloadProgressBar()
	{
		OnPropertyChanged( "ConfigSystem" );
	}

	public event PropertyChangedEventHandler? PropertyChanged = delegate { };

	public void OnPropertyChanged( [CallerMemberName] string? aPropertyName = null )
		=> PropertyChanged?.Invoke( this, new( aPropertyName ) );

	#endregion

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
				case 0: SetStatusText( "Informational"	, aText, InfoBarSeverity.Informational	); break;
				case 1: SetStatusText( "Warning"		, aText, InfoBarSeverity.Warning		); break;
   				case 2: SetStatusText( "Error"			, aText, InfoBarSeverity.Error			); break;
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
