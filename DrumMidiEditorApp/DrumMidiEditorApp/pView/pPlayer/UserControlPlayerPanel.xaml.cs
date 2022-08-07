using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Threading;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using System.Threading.Tasks;
using Windows.Graphics.DirectX;
using DrumMidiEditorApp.pControl;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface;

public sealed partial class UserControlPlayerPanel : UserControl
{
    #region Member

    /// <summary>
    /// Player設定
    /// </summary>
    private ConfigPlayer DrawSet => Config.Player;

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

        //PlayerIdleAsync();
    }


    private Task? _IdleTask = null;

    private CancellationTokenSource? _IdleTaskCancelToken = null;

	/// <summary>
	/// 描画ループ
	/// </summary>
	public void PlayerIdleAsync()
    {
        try
        {
            Config.EventUpdatePlayerMode();

            _IdleTaskCancelToken?.Dispose();
            _IdleTaskCancelToken = new();

            // TODO: 描画処理を非同期実行。
            // TODO: 非同期処理を考慮していなかったの落ちる可能性高いです。
            // TODO: そのうち見直し予定
            _IdleTask = Task.Run
                (
                    () => 
                    { 
                        var fps = new Fps();
                        fps.Set( 1, 0 );
                        fps.Set( 2, DrawSet.Fps );
                        fps.Start();

                        while ( !_IdleTaskCancelToken.IsCancellationRequested )
                        {
                            // プレイヤー描画モード変更
                            if ( DrawSet.UpdateSurfaceModoFlag )
                            {
                                DrawSet.UpdateSurfaceModoFlag = false;
                                UpdateSurfaceMode();
                            }

                            // サイズ変更
                            if ( DrawSet.UpdateSizeFlag )
                            {
                                DrawSet.UpdateSizeFlag = false;
                                UpdatePanelSize();
                            }

                            // フレーム更新＆描画処理
                            fps.Tick();

			                if ( fps.TickFpsWeight( 1 ) )
			                { 
				                _PlayerSurface?.OnMove( fps.GetFrameTime( 1 ) );
			                }

			                if ( fps.TickFpsWeight( 2 ) )
			                {
                                PlayerCanvasSwapChain_Draw();

                                fps.TickFpsWeight( 2 );
			                }

                            Task.Delay( 1 );
                        }
                    }
                );
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }


    private CanvasRenderTarget? _Offscreen = null;

    public void StartGetFrame()
    {
        _IdleTaskCancelToken?.Cancel();
        _IdleTask?.Wait();

        DmsControl.RecordPreSequence();
        DmsControl.WaitRecorder();

        UpdateSurfaceMode();
        UpdatePanelSize();

        // 描画処理
        _Offscreen = new CanvasRenderTarget
            (
                CanvasDevice.GetSharedDevice(),
                DrawSet.ResolutionScreenWidth,
                DrawSet.ResolutionScreenHeight,
                96
            );
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

            drawSession.Clear( DrawSet.SheetColor.Color );

            var args = new CanvasDrawEventArgs( drawSession );

            _PlayerSurface?.OnDraw( args );

            // Bitmap作成
            return CanvasBitmap.CreateFromBytes
                (
                    drawSession,
                    _Offscreen.GetPixelBytes( 0, 0, (int)_Offscreen.SizeInPixels.Width, (int)_Offscreen.SizeInPixels.Height ),
                    (int)_Offscreen.SizeInPixels.Width,
                    (int)_Offscreen.SizeInPixels.Height,
                    DirectXPixelFormat.R8G8B8A8UIntNormalized,
                //  DirectXPixelFormat.B8G8R8A8UIntNormalized,
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

    public void EndGetFrame()
    {
        DmsControl.StopPreSequence();

        _Offscreen?.Dispose();

        PlayerIdleAsync();
    }

    /// <summary>
    /// プレイヤー描画モード変更
    /// </summary>
    private void UpdateSurfaceMode()
    {
        switch ( DrawSet.PlayerSurfaceModeSelect )
        {
			case ConfigPlayer.PlayerSurfaceMode.Sequence:
   				_PlayerSurface = new pSequence.PlayerSurface();
				break;
			case ConfigPlayer.PlayerSurfaceMode.Score:
   				_PlayerSurface = new pScore.PlayerSurface();
				break;
			case ConfigPlayer.PlayerSurfaceMode.Simuration:
   				_PlayerSurface = new pSimuration.PlayerSurface();
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

    #region Draw

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

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PlayerCanvasSwapChain_Draw()
    {
        try
        {
            using var drawSession = _PlayerCanvas.SwapChain
                .CreateDrawingSession( DrawSet.SheetColor.Color );

            var args = new CanvasDrawEventArgs( drawSession );

            _PlayerSurface?.OnDraw( args );

            _PlayerCanvas.SwapChain.Present();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
