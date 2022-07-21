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
using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pDirectX;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pView.pPlayer;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PagePlayer : Page
{
    public PagePlayer()
    {
        this.InitializeComponent();
    }

#if false
	/// <summary>
	/// マウスアクション
	/// </summary>
	private enum EActionState
	{
		None = 0
	,	PlayerMove
	,	PlayerOff
	}

	/// <summary>
	/// マウスアクション状態
	/// </summary>
	private EActionState _ActionState = EActionState.None;

	/// <summary>
	/// DirectX2D
	/// </summary>
	private readonly Graphic2D _Graphic2D;

	/// <summary>
	/// プレイヤーサーフェイス
	/// </summary>
	private IPlayerSurface? _PlayerSurface = null;

	/// <summary>
	/// フォーム移動前の位置
	/// </summary>
	private Point _BeforePos = new();

	/// <summary>
	/// FPS制御
	/// </summary>
	private readonly Fps _Fps = new();

	/// <summary>
	/// プレイヤー設定フォーム
	/// </summary>
	private readonly PlayerConfigForm? _PCForm = null;

	/// <summary>
	/// プレイヤーコントロール
	/// </summary>
	private static PlayerControl? PlayerControl => DMS.EditerForm?.PlayerCtl;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	public PlayerForm()
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
		SetStyle( ControlStyles.ResizeRedraw, false );

		// true の場合、コントロールはフォーカスを受け取ることができます。 
		SetStyle( ControlStyles.Selectable, false );

		// true の場合、コントロールは、標準の Click 動作を実装します。 
		SetStyle( ControlStyles.StandardClick, false );

		// true の場合、コントロールは、標準の DoubleClick 動作を実装します。
		// StandardClick ビットが true に設定されている場合、このスタイルは無視されます。 
		SetStyle( ControlStyles.StandardDoubleClick, false );

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

		SuspendLayout();
		{
			ClientSize = new( Config.Player.ResolutionScreenWidth, Config.Player.ResolutionScreenHeight );
		}
		ResumeLayout();

		_Graphic2D	= new( Handle );
		_Graphic2D.Initialize
			( 
				Config.Player.ResolutionScreenWidth,
				Config.Player.ResolutionScreenHeight
			);

		ChangeDrawMode();

		_PCForm = new();

		_Fps.Set( 1, 0 );
		_Fps.Set( 2, Config.Player.Fps );
		_Fps.Start();

		Show();
	}

	/// <summary>
	/// 描画モード変更
	/// </summary>
	public void ChangeDrawMode()
    {
		switch ( Config.Player.PlayerSurfaceModeSelect )
        {
			case ConfigPlayer.PlayerSurfaceMode.Sequence:
				_PlayerSurface = new pSequence.PlayerSurface( _Graphic2D );
				break;
			case ConfigPlayer.PlayerSurfaceMode.SequenceVertical:
				_PlayerSurface = new pSequenceVertical.PlayerSurface( _Graphic2D );
				break;
			case ConfigPlayer.PlayerSurfaceMode.Score:
				_PlayerSurface = new pScore.PlayerSurface( _Graphic2D );
				break;
			case ConfigPlayer.PlayerSurfaceMode.Simuration:
				_PlayerSurface = new pSimuration.PlayerSurface( _Graphic2D );
				break;
        }

		Config.EventReloadScore();
    }

	/// <summary>
	/// 解像度変更
	/// </summary>
	public void ResetGraphic()
	{
		_Graphic2D?.ResizeBuffer
			(
				Config.Player.ResolutionScreenWidth,
				Config.Player.ResolutionScreenHeight
			);
	}

	/// <summary>
	/// 描画
	/// </summary>
	public void DrawFrame()
	{
		try
		{
			_Fps.Tick();

			if ( _Fps.TickFpsWeight( 1 ) )
			{ 
				_PlayerSurface?.OnMove( _Fps.GetFrameTime( 1 ) );
			}

			if ( !_Fps.TickFpsWeight( 2 ) )
			{
				return;
			}

			_Graphic2D.DrawStart();

			_PlayerSurface?.OnDraw();

			_Graphic2D.DrawEnd();

			SetFormLayout();

			_Fps.TickFpsWeight( 2 );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
			Log.Error( e );

			_Fps.Reset();
		}
	}

	/// <summary>
	/// 画面ハードコピー取得
	/// </summary>
	/// <param name="aFrameTime">再生時間（秒）</param>
	/// <returns>画面ハードコピー</returns>
	public Bitmap? GetFrame( double aFrameTime )
	{
		Bitmap? bmp = null;

		try
		{
			_PlayerSurface?.OnMove( aFrameTime );

			_Graphic2D.DrawStart();

			_PlayerSurface?.OnDraw();

			_Graphic2D.DrawEnd();

			bmp = _Graphic2D.GetScreenShot();
		}
		catch ( Exception e )
		{
			Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		return bmp;
	}

	/// <summary>
	/// マウスダウン
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void PlayerForm_MouseDown( object sender, MouseEventArgs ev )
	{
		try
		{
			if ( Config.Player.EditModeOn )
			{ 
				_PlayerSurface?.MouseDown( sender, ev );
				return;
			}

			if ( _ActionState != EActionState.None )
			{
				return;
			}

			switch ( ev.Button )
			{
				case MouseButtons.Left:
					{
						// フォーム移動
						_BeforePos = ev.Location;
						SetFormLayout();

						_ActionState = EActionState.PlayerMove;
					}
					break;
				case MouseButtons.Right:
					{
						// 非表示
                        PlayerControl?.DoPlayerCheckBox();

						_ActionState = EActionState.None;
					}
					break;
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

			_ActionState = EActionState.None;
		}
	}

	/// <summary>
	/// マウス移動
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void PlayerForm_MouseMove( object sender, MouseEventArgs ev )
	{
		try
		{
			if ( Config.Player.EditModeOn )
			{ 
				_PlayerSurface?.MouseMove( sender, ev );
				return;
			}

			if ( _ActionState == EActionState.None )
			{
				return;
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );

			_ActionState = EActionState.None;
		}
	}

	/// <summary>
	/// マウスアップ
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="ev"></param>
	private void PlayerForm_MouseUp( object sender, MouseEventArgs ev )
	{
		try
		{
			if ( Config.Player.EditModeOn )
			{ 
				_PlayerSurface?.MouseUp( sender, ev );
				return;
			}

			if ( _ActionState == EActionState.None )
			{
				return;
			}

			switch ( ev.Button )
			{
				case MouseButtons.Left:
					{
						var pos = ev.Location;

						switch ( _ActionState )
						{
							case EActionState.PlayerMove:
                                {
									Location = new Point
										(
											Location.X + ev.Location.X - _BeforePos.X
										,	Location.Y + ev.Location.Y - _BeforePos.Y
										);

									SetFormLayout();
								}
								break;
						}
					}
					break;
			}
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
		finally
        {
			_ActionState = EActionState.None;
		}
	}

#endif
}
