using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pDMS;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pGeneralFunction.pWinApi;
using DrumMidiEditor.pView.pEditer.pMidiMapSet;
using DrumMidiEditor.pView.pEditer.pEdit;
using DrumMidiEditor.pView.pEditer.pMusic;
using DrumMidiEditor.pView.pEditer.pPlay;
using DrumMidiEditor.pView.pEditer.pScore;

namespace DrumMidiEditor.pView.pEditer;

/// <summary>
/// メインフォーム
/// </summary>
public partial class EditerForm : Form
{
	/// <summary>
	/// プレイヤーコントロール
	/// </summary>
	public PlayerControl PlayerCtl => PlayerControl;

	/// <summary>
	/// Musicタブコントロール
	/// </summary>
	public MusicControl MusicCtl => MusicControl;

	/// <summary>
	/// MidiMapSetタブコントロール
	/// </summary>
	public MidiMapSetControl MidiMapSetCtl => MidiMapSetControl;

	/// <summary>
	/// Editerタブコントロール
	/// </summary>
	public EditerControl EditerCtl => EditerControl;

	/// <summary>
	/// Scoreタブコントロール
	/// </summary>
	public ScoreControl ScoreCtl => ScoreControl;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public EditerForm()
    {
        InitializeComponent();

		#region SetStyle

		// true の場合、コントロールはウィンドウ メッセージ WM_ERASEBKGND を無視することによって、ちらつきを抑えます。
		// このスタイルは、UserPaint ビットが true に設定されている場合だけ適用されます。 
        SetStyle( ControlStyles.AllPaintingInWmPaint, true );

		// true の場合、コントロールは、必要に応じて毎回 Handle からテキストのコピーを取得するのではなく
		// テキストのコピーを保持します。このスタイルは、既定では false に設定されます。
		// この動作によってパフォーマンスは向上しますが、テキストを同期させておくことが難しくなります。 
        SetStyle( ControlStyles.CacheText, true );

		// true の場合、コントロールは、コンテナのような機能を果たします。 
		SetStyle( ControlStyles.ContainerControl, false );

		// true の場合、描画はバッファで実行され、完了後に、結果が画面に出力されます。
		// ダブル バッファリングは、コントロールの描画によるちらつきを防ぎます。
		// DoubleBuffer を true に設定した場合は、UserPaint および AllPaintingInWmPaint も true に設定する必要があります。 
        SetStyle( ControlStyles.DoubleBuffer, true );

		// true の場合、コントロールの WndProc に送信されたすべてのメッセージに対して OnNotifyMessage メソッドが呼び出されます。
		// このスタイルは、既定で false に設定されます。部分信頼では、EnableNotifyMessage は正常に機能しません。
		SetStyle( ControlStyles.EnableNotifyMessage, false );

		// true の場合は、自動スケールされるとき、コントロールの高さが固定されます。
		SetStyle( ControlStyles.FixedHeight, false );

		// true の場合、自動スケールされるとき、コントロールの幅が固定されます。
		SetStyle( ControlStyles.FixedWidth, false );

		// true の場合は、コントロールが不透明に描画され、背景は描画されません。 
		SetStyle( ControlStyles.Opaque, true );

		// true の場合、コントロールは、画面に直接ではなく、まずバッファに描画されます。
		// これにより、ちらつきを抑えることができます。このプロパティを true に設定した場合は、
		// AllPaintingInWmPaint も true に設定する必要があります。 
		SetStyle( ControlStyles.OptimizedDoubleBuffer, true );

		// true の場合は、コントロールのサイズが変更されると、そのコントロールが再描画されます。 
		SetStyle( ControlStyles.ResizeRedraw, true );

		// true の場合、コントロールはフォーカスを受け取ることができます。 
		SetStyle( ControlStyles.Selectable, true );

		// true の場合、コントロールは、標準の Click 動作を実装します。 
		SetStyle( ControlStyles.StandardClick, true );

		// true の場合、コントロールは、標準の DoubleClick 動作を実装します。
		// StandardClick ビットが true に設定されている場合、このスタイルは無視されます。 
		SetStyle( ControlStyles.StandardDoubleClick, true );

		// true の場合、コントロールは、アルファ値が 255 未満の BackColor を受け入れ、透明度をシミュレートします。
		// この透明度は、UserPaint ビットが true に設定され、親コントロールが Control から派生している場合だけシミュレートされます。 
		SetStyle( ControlStyles.SupportsTransparentBackColor, false );

		// true の場合は、コントロールがマウスの操作を独自に処理し、オペレーティング システムではマウス イベントが処理されません。 
		SetStyle( ControlStyles.UserMouse, true );

		// true の場合は、コントロールは、オペレーティング システムによってではなく、独自に描画されます。
		// false の場合、Paint イベントは発生しません。このスタイルは、Control から派生したクラスだけに適用されます。 
		SetStyle( ControlStyles.UserPaint, true );

		// コントロールの Text プロパティの値を指定します。設定した場合、
		// コントロールの既定の Active Accessibility 名とショートカット キーが決定されます。
		SetStyle( ControlStyles.UseTextForAccessibility, false );

		#endregion

		Text = $"{Config.System.AppName}";

		Log.LogOutputCallback.Enqueue( SetStatusText );

		Application.Idle += new EventHandler( OnApplicationIdle );
	}

	/// <summary>
	/// アイドル処理
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void OnApplicationIdle( object? sender, EventArgs ev )
	{
        while ( !WinApi.PeekMessage( out _, IntPtr.Zero, 0, 0, 0 ) )
		{
			DMS.PlayerForm?.DrawFrame();
			Thread.Sleep( 1 );
		}
	}

	#region StatusLabel

	/// <summary>
	/// ステータスバーへのログ出力
	/// </summary>
	/// <param name="aLevel">0:Info, 1:Warning, 2:Error</param>
	/// <param name="aText">出力内容</param>
	public void SetStatusText( int aLevel, string aText )
    {
		switch ( aLevel )
        {
			case 0: SetStatusText( aText, Color.Blue	); break;
			case 1: SetStatusText( aText, Color.Yellow	); break;
   			case 2: SetStatusText( aText, Color.Red		); break;
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
            if ( InvokeRequired )
            {
                Invoke( new Action( () => { SetStatusText( aText, aColor ); } ) );
                return;
            }

			MainToolStripStatusLabel.Text		= aText ?? String.Empty ;
			MainToolStripStatusLabel.ForeColor	= aColor;
			Refresh();
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
			Log.ChangeDisplayLogForm();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
	}

    #endregion
}

