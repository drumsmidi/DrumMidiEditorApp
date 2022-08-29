using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Threading.Tasks;

using DrumMidiClassLibrary.pLog;
using DrumMidiClassLibrary.pUtil;

using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pEvent;
using DrumMidiPlayerApp.pView.pPlayer.pSurface;

namespace DrumMidiPlayerApp.pView.pPlayer;

public sealed partial class PanelPlayer : UserControl
{
    #region Member

    /// <summary>
    /// Player設定
    /// </summary>
    private static ConfigPlayer DrawSet => ConfigLocal.Player;

    /// <summary>
    /// プレイヤーサーフェイス
    /// </summary>
    private IPlayerSurface? _PlayerSurface = null;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PanelPlayer()
    {
        InitializeComponent();

        ControlAccess.PanelPlayer = this;

        // スワップチェイン作成
        _PlayerCanvas.SwapChain = new CanvasSwapChain
            (
                new CanvasDevice(),
                DrawSet.ResolutionScreenWidth,
                DrawSet.ResolutionScreenHeight,
                96
            );

        DrawTaskStart();
    }

    #region 描画スレッドプール

    /// <summary>
    /// 描画タスク
    /// </summary>
    private Task? _IdleTask = null;

    /// <summary>
    /// .描画タスク停止フラグ
    /// </summary>
    private bool _IdleTaskStop = true;

	/// <summary>
	/// 描画タスク開始
	/// </summary>
	public void DrawTaskStart()
    {
        try
        {
            DrawTaskStop();

            EventManage.EventPlayerUpdateSufaceMode();

            _IdleTaskStop = false;

            // 粒度の細かいシステムではなく、タスクが長時間実行され、
            // 少量の大きなコンポーネントを含む粒度の粗い操作とすることを指定します。
            // これは、TaskScheduler に対し、オーバーサブスクリプションを許可してもよいことを示します。
            // オーバーサブスクリプションを使用すると、使用可能なハードウェア スレッドよりも多くのスレッドを作成できます。
            // これは、タスクの処理に追加のスレッドが必要になる可能性があるというヒントをタスク スケジューラに提供し、
            // 他のスレッドまたはローカル スレッド プール キューの作業項目の進行をスケジューラがブロックするのを防ぎます。
            _IdleTask = Task.Factory.StartNew( DrawTaskAsync, TaskCreationOptions.LongRunning );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// 描画タスク
    /// </summary>
    public void DrawTaskAsync()
    {
        try
        {
            var fps = new Fps();
            fps.Set( 1, 0 );
            fps.Start();

            while ( !_IdleTaskStop )
            {
                // サイズ変更
                if ( DrawSet.UpdateSizeFlag )
                {
                    DrawSet.UpdateSizeFlag = false;
                    UpdatePanelSize();
                }

                // プレイヤー描画モード変更
                if ( DrawSet.UpdateSurfaceModoFlag )
                {
                    DrawSet.UpdateSurfaceModoFlag = false;
                    UpdateSurfaceMode();

                    EventManage.EventPlayerUpdateScore();
                }

                // フレーム更新＆描画処理
                fps.Tick();

                // フレーム更新
                _PlayerSurface?.OnMove( fps.GetFrameTime( 1 ) );

                // 描画処理
                using var drawSession = _PlayerCanvas.SwapChain.CreateDrawingSession( DrawSet.SheetColor.Color );

                _PlayerSurface?.OnDraw( new CanvasDrawEventArgs( drawSession ) );

                _PlayerCanvas.SwapChain.Present();

                // 垂直同期
                _PlayerCanvas.SwapChain.WaitForVerticalBlank();
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

	/// <summary>
	/// 描画タスク停止
	/// </summary>
	public void DrawTaskStop()
    {
        try
        {
            _IdleTaskStop = true;
            _IdleTask?.Wait();
            _IdleTask?.Dispose();
            _IdleTask = null;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// プレイヤー描画モード変更
    /// </summary>
    private void UpdateSurfaceMode()
    {
        switch ( DrawSet.PlayerSurfaceModeSelect )
        {
			case ConfigPlayer.PlayerSurfaceMode.Sequence:
   				_PlayerSurface = new pSurface.pSequence.PlayerSurface();
				break;
        }
    }

    /// <summary>
    /// 描画サイズ変更
    /// </summary>
    private void UpdatePanelSize()
    {
        _PlayerCanvas.SwapChain.ResizeBuffers
            (
                DrawSet.ResolutionScreenWidth,
                DrawSet.ResolutionScreenHeight,
                96
            );
    }

    #endregion

    #region Mouse Event

    /// <summary>
    /// マウスホイール処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerCanvas_PointerWheelChanged( object sender, PointerRoutedEventArgs args )
    {
        try
        {
            _PlayerSurface?.MouseWheel( sender, args );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// マウスダウン処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerCanvas_PointerPressed( object sender, PointerRoutedEventArgs args )
    {
        try
        {
            _PlayerSurface?.MouseDown( sender, args );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// マウス移動処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerCanvas_PointerMoved( object sender, PointerRoutedEventArgs args )
    {
        try
        {
            _PlayerSurface?.MouseMove( sender, args );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// マウスアップ処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerCanvas_PointerReleased( object sender, PointerRoutedEventArgs args )
    {
        try
        {
            _PlayerSurface?.MouseUp( sender, args );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    /// <summary>
    /// Win2D アンロード処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void UserControl_Unloaded( object sender, RoutedEventArgs args )
    {
        try
        {
            // Win2D アンロード
            _PlayerCanvas.RemoveFromVisualTree();
            _PlayerCanvas = null;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }
}
