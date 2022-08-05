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

        PlayerIdleAsync();
    }

	/// <summary>
	/// 描画ループ
	/// </summary>
	public async void PlayerIdleAsync()
    {
        try
        {
            Config.EventUpdatePlayerMode();

            // TODO: 描画処理を非同期実行。
            // TODO: 非同期処理を考慮していなかったの落ちる可能性高いです。
            // TODO: そのうち見直し予定
            await Task.Run
                (
                    () => 
                    { 
                        var fps = new Fps();
                        fps.Set( 1, 0 );
                        fps.Set( 2, DrawSet.Fps );
                        fps.Start();

                        while ( true )
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
			case ConfigPlayer.PlayerSurfaceMode.SequenceVertical:
   				_PlayerSurface = new pSequenceVertical.PlayerSurface();
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

            _PlayerSurface?.OnDraw( _PlayerCanvas, args );

            _PlayerCanvas.SwapChain.Present();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
