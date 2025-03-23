using System;
using System.Threading.Tasks;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pEvent;
using DrumMidiPlayerApp.pView.pPlayer.pSurface;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Graphics.DirectX;

namespace DrumMidiPlayerApp.pView.pPlayer;

public sealed partial class UserControlPlayerPanel2 : UserControl
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
    public UserControlPlayerPanel2()
    {
        InitializeComponent();

        ControlAccess.UCPlayerPanel = this;

        // スワップチェイン作成
        _PlayerCanvas.SwapChain = new CanvasSwapChain
            (
                new CanvasDevice(),
                DrawSet.ResolutionScreenWidth,
                DrawSet.ResolutionScreenHeight,
                Config.Media.DefaultDpi,                    // DisplayInformation.GetForCurrentView().LogicalDpi
                DirectXPixelFormat.R8G8B8A8UIntNormalized,
            //  DirectXPixelFormat.B8G8R8A8UIntNormalized,
                2,
                CanvasAlphaMode.Premultiplied
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
    private bool _FlagIdleTaskStop = true;

    /// <summary>
    /// 描画タスク開始
    /// </summary>
    public void DrawTaskStart()
    {
        try
        {
            DrawTaskStop();

            EventManage.Event_Player_UpdateSufaceMode();

            _FlagIdleTaskStop = false;

            // 粒度の細かいシステムではなく、タスクが長時間実行され、
            // 少量の大きなコンポーネントを含む粒度の粗い操作とすることを指定します。
            // これは、TaskScheduler に対し、オーバーサブスクリプションを許可してもよいことを示します。
            // オーバーサブスクリプションを使用すると、使用可能なハードウェア スレッドよりも多くのスレッドを作成できます。
            // これは、タスクの処理に追加のスレッドが必要になる可能性があるというヒントをタスク スケジューラに提供し、
            // 他のスレッドまたはローカル スレッド プール キューの作業項目の進行をスケジューラがブロックするのを防ぎます。
            _IdleTask = Task.Factory.StartNew( DrawTaskAsync, TaskCreationOptions.LongRunning );
        //  _IdleTask = Task.Run( () => DrawTaskAsync() );
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
        //  fps.Set( 2, DrawSet.Fps );
            fps.Start();

            while ( !_FlagIdleTaskStop )
            {
                // サイズ変更
                if ( DrawSet.FlagUpdateSize )
                {
                    DrawSet.FlagUpdateSize = false;
                    UpdatePanelSize();
                }

                // プレイヤー描画モード変更
                if ( DrawSet.FlagUpdateSurfaceModo )
                {
                    DrawSet.FlagUpdateSurfaceModo = false;
                    UpdateSurfaceMode();

                    EventManage.Event_Player_UpdateScore();
                }

                // フレーム更新
                fps.Tick();

                _ = ( _PlayerSurface?.OnMove( fps.GetFrameTime( 1 ) ) );

                // 描画処理
                using var cl = new CanvasCommandList( _PlayerCanvas.SwapChain );

                using var drawSessionA = _PlayerCanvas.SwapChain.CreateDrawingSession( DrawSet.SheetColor.Color );
                using var drawSessionB = cl.CreateDrawingSession();

                var args = new CanvasDrawEventArgs( drawSessionB );

                _ = ( _PlayerSurface?.OnDraw( args ) );

                using var blur = new AtlasEffect { Source = cl };

                if ( blur != null )
                {
                    drawSessionA.DrawImage( blur );
                }

                _PlayerCanvas.SwapChain.Present();
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
            _FlagIdleTaskStop = true;
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
            case ConfigSystem.PlayerSurfaceMode.Sequence:
                _PlayerSurface = new pSurface.pSequence.PlayerSurface();
                break;
            case ConfigSystem.PlayerSurfaceMode.ScoreType2:
                _PlayerSurface = new pSurface.pScoreType2.PlayerSurface();
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
                Config.Media.DefaultDpi
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
