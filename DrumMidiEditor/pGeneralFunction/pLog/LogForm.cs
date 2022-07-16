using System;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;

namespace DrumMidiEditor.pGeneralFunction.pLog;

/// <summary>
/// ログ出力フォーム
/// </summary>
internal partial class LogForm : Form
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public LogForm()
    {
        InitializeComponent();

		#region SetStyle

		// true の場合、コントロールはウィンドウ メッセージ WM_ERASEBKGND を無視することによって、ちらつきを抑えます。
		// このスタイルは、UserPaint ビットが true に設定されている場合だけ適用されます。 
        this.SetStyle( ControlStyles.AllPaintingInWmPaint, true );

		// true の場合、コントロールは、必要に応じて毎回 Handle からテキストのコピーを取得するのではなく
		// テキストのコピーを保持します。このスタイルは、既定では false に設定されます。
		// この動作によってパフォーマンスは向上しますが、テキストを同期させておくことが難しくなります。 
        this.SetStyle( ControlStyles.CacheText, true );

		// true の場合、コントロールは、コンテナのような機能を果たします。 
		this.SetStyle( ControlStyles.ContainerControl, false );

		// true の場合、描画はバッファで実行され、完了後に、結果が画面に出力されます。
		// ダブル バッファリングは、コントロールの描画によるちらつきを防ぎます。
		// DoubleBuffer を true に設定した場合は、UserPaint および AllPaintingInWmPaint も true に設定する必要があります。 
        this.SetStyle( ControlStyles.DoubleBuffer, true );

		// true の場合、コントロールの WndProc に送信されたすべてのメッセージに対して OnNotifyMessage メソッドが呼び出されます。
		// このスタイルは、既定で false に設定されます。部分信頼では、EnableNotifyMessage は正常に機能しません。
		this.SetStyle( ControlStyles.EnableNotifyMessage, false );

		// true の場合は、自動スケールされるとき、コントロールの高さが固定されます。
		this.SetStyle( ControlStyles.FixedHeight, false );

		// true の場合、自動スケールされるとき、コントロールの幅が固定されます。
		this.SetStyle( ControlStyles.FixedWidth, false );

		// true の場合は、コントロールが不透明に描画され、背景は描画されません。 
		this.SetStyle( ControlStyles.Opaque, true );

		// true の場合、コントロールは、画面に直接ではなく、まずバッファに描画されます。
		// これにより、ちらつきを抑えることができます。このプロパティを true に設定した場合は、
		// AllPaintingInWmPaint も true に設定する必要があります。 
		this.SetStyle( ControlStyles.OptimizedDoubleBuffer, true );

		// true の場合は、コントロールのサイズが変更されると、そのコントロールが再描画されます。 
		this.SetStyle( ControlStyles.ResizeRedraw, false );

		// true の場合、コントロールはフォーカスを受け取ることができます。 
		this.SetStyle( ControlStyles.Selectable, false );

		// true の場合、コントロールは、標準の Click 動作を実装します。 
		this.SetStyle( ControlStyles.StandardClick, false );

		// true の場合、コントロールは、標準の DoubleClick 動作を実装します。
		// StandardClick ビットが true に設定されている場合、このスタイルは無視されます。 
		this.SetStyle( ControlStyles.StandardDoubleClick, false );

		// true の場合、コントロールは、アルファ値が 255 未満の BackColor を受け入れ、透明度をシミュレートします。
		// この透明度は、UserPaint ビットが true に設定され、親コントロールが Control から派生している場合だけシミュレートされます。 
		this.SetStyle( ControlStyles.SupportsTransparentBackColor, false );

		// true の場合は、コントロールがマウスの操作を独自に処理し、オペレーティング システムではマウス イベントが処理されません。 
		this.SetStyle( ControlStyles.UserMouse, true );

		// true の場合は、コントロールは、オペレーティング システムによってではなく、独自に描画されます。
		// false の場合、Paint イベントは発生しません。このスタイルは、Control から派生したクラスだけに適用されます。 
		this.SetStyle( ControlStyles.UserPaint, false );

		// コントロールの Text プロパティの値を指定します。設定した場合、
		// コントロールの既定の Active Accessibility 名とショートカット キーが決定されます。
		this.SetStyle( ControlStyles.UseTextForAccessibility, false );

		#endregion
    }

    /// <summary>
    /// ログフォーム閉じるボタン押下時に終了せずに、非表示とする。
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void LogForm_FormClosing( object sender, FormClosingEventArgs ev )
    {
        ev.Cancel   = true;
        Visible     = false;
    }

    /// <summary>
    /// ログ出力を追加
    /// </summary>
    /// <param name="aText"></param>
    public void AddLog( string aText )
    {
        try
        {
            if ( InvokeRequired )
            {
                Invoke( new Action( () => { AddLog( aText ); } ) );
                return;
            }

            int cnt  = Config.System.LogOutputLineCount;
            var text = ( aText ?? String.Empty ) + Environment.NewLine;

            foreach ( var line in ConsoleRichTextBox.Lines )
            {
                text += line + Environment.NewLine;

                if ( cnt-- == 0 )
                {
                    break;
                }
            }

            ConsoleRichTextBox.Text = text;
            ConsoleRichTextBox.Invalidate();
        }
        catch ( Exception )
        {
        }
    }

    /// <summary>
    /// ログフォーム表示切替
    /// </summary>
    /// <param name="aDisplay">True:表示、False:非表示</param>
    public void SetDisplay( bool aDisplay )
    {
        try
        {
            if ( InvokeRequired )
            {
                Invoke( new Action( () => { SetDisplay( aDisplay ); } ) );
                return;
            }

            Visible = aDisplay;

            if ( Visible )
            {
                Refresh();
                Focus();
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
