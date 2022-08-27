using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Threading.Tasks;
using Windows.Graphics.DirectX;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pView.pPlayer.pSurface;

namespace DrumMidiEditorApp.pView.pPlayer;

public sealed partial class UserControlPlayerPanel : UserControl
{
    #region Member

    /// <summary>
    /// Player設定
    /// </summary>
    private static ConfigPlayer DrawSet => Config.Player;

    /// <summary>
    /// プレイヤーサーフェイス
    /// </summary>
    private IPlayerSurface? _PlayerSurface = null;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlPlayerPanel()
    {
        InitializeComponent();

        ControlAccess.UCPlayerPanel = this;

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
            // _IdleTask = Task.Run( () => DrawTaskAsync() );
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
            //fps.Set( 2, DrawSet.Fps );
            fps.Start();

            while ( !_IdleTaskStop )
            {
                // サイズ変更
                if (DrawSet.UpdateSizeFlag )
                {
                    DrawSet.UpdateSizeFlag = false;
                    UpdatePanelSize();
                }

                // プレイヤー描画モード変更
                if (DrawSet.UpdateSurfaceModoFlag )
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
                using var drawSession = _PlayerCanvas.SwapChain.CreateDrawingSession(DrawSet.SheetColor.Color );

                _PlayerSurface?.OnDraw( new CanvasDrawEventArgs( drawSession ) );

                _PlayerCanvas.SwapChain.Present();

                // 垂直同期
                _PlayerCanvas.SwapChain.WaitForVerticalBlank();

                //await Task.Delay( 1 );
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

    #region 画面ハードコピー

    /// <summary>
    /// 画面ハードコピー用オフスクリーン
    /// </summary>
    private CanvasRenderTarget? _Offscreen = null;

    /// <summary>
    /// 画面ハードコピー開始準備
    /// </summary>
    public void GetFrameStart()
    {
        try
        {
            DrawTaskStop();

            DmsControl.RecordPreSequence();
            DmsControl.WaitRecorder();

            // 更新処理
            UpdateSurfaceMode();
            UpdatePanelSize();

            // オフスクリーン作成
            _Offscreen = new CanvasRenderTarget
                (
                    CanvasDevice.GetSharedDevice(),
                    DrawSet.ResolutionScreenWidth,
                    DrawSet.ResolutionScreenHeight,
                    96
                );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// 画面コピー取得
    /// </summary>
    /// <param name="aFrameTime"></param>
	public CanvasBitmap? GetFrame( double aFrameTime )
    {
        if ( _Offscreen == null )
        {
            return null;
        }

        try
        {
            // フレーム処理
            _PlayerSurface?.OnMove( aFrameTime );

            // 描画処理
            using var drawSession = _Offscreen.CreateDrawingSession();

            drawSession.Clear(DrawSet.SheetColor.Color );

            _PlayerSurface?.OnDraw( new CanvasDrawEventArgs( drawSession ) );

            // Bitmap作成
            return CanvasBitmap.CreateFromBytes
                (
                    drawSession,
                    _Offscreen.GetPixelBytes( 0, 0, (int)_Offscreen.SizeInPixels.Width, (int)_Offscreen.SizeInPixels.Height ),
                    (int)_Offscreen.SizeInPixels.Width,
                    (int)_Offscreen.SizeInPixels.Height,
                    DirectXPixelFormat.R8G8B8A8UIntNormalized,
                    96,
                    CanvasAlphaMode.Premultiplied
                );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
            return null;
        }
    }

    /// <summary>
    /// 画面ハードコピー終了準備
    /// </summary>
    public void GetFrameEnd()
    {
        try
        {
            DmsControl.StopPreSequence();

            _Offscreen?.Dispose();
            _Offscreen = null;

            DrawTaskStart();
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
        switch (DrawSet.PlayerSurfaceModeSelect )
        {
			case ConfigPlayer.PlayerSurfaceMode.Sequence:
   				_PlayerSurface = new pSurface.pSequence.PlayerSurface();
				break;
			case ConfigPlayer.PlayerSurfaceMode.Score:
   				_PlayerSurface = new pSurface.pScore.PlayerSurface();
				break;
			case ConfigPlayer.PlayerSurfaceMode.Simuration:
   				_PlayerSurface = new pSurface.pSimuration.PlayerSurface();
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
