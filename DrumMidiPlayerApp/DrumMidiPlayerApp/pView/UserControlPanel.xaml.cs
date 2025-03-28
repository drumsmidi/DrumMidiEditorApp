using System;
using System.Threading.Tasks;
using DrumMidiLibrary.pInput;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using DrumMidiPlayerApp.pEvent;
using DrumMidiPlayerApp.pView.pScreen;
using DrumMidiPlayerApp.pView.pScreen.pSongSelect;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics.DirectX;

namespace DrumMidiPlayerApp.pView.pPlayer;

public sealed partial class UserControlPanel : UserControl
{
    #region Member

    /// <summary>
    /// メインサーフェイス
    /// </summary>
    private IScreen? _CurrentSurface = null;

    /// <summary>
    /// 曲選択サーフェイス
    /// </summary>
    private IScreen? _SongSelectSurface = null;

    /// <summary>
    /// 曲選択サーフェイス
    /// </summary>
    private IScreen? _PlayerSurface = null;

    #endregion

    #region コンストラクタ / アンロード

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlPanel()
    {
        InitializeComponent();

        ControlAccess.MainPanel = this;

        // スワップチェイン作成
        _Canvas.SwapChain = new CanvasSwapChain
            (
                new CanvasDevice(),
                Config.Panel.ResolutionScreenWidth,
                Config.Panel.ResolutionScreenHeight,
                Config.Window.DefaultDpi,
                DirectXPixelFormat.R8G8B8A8UIntNormalized,
                2,
                CanvasAlphaMode.Premultiplied
            );

        // 描画タスク開始
        DrawTaskStart();

        // マウスイベントキャプチャ
        _Canvas.PointerPressed      += InputControl.PointerPressed;
        _Canvas.PointerMoved        += InputControl.PointerMoved;
        _Canvas.PointerExited       += InputControl.PointerReleased;
        _Canvas.PointerCanceled     += InputControl.PointerReleased;
        _Canvas.PointerCaptureLost  += InputControl.PointerReleased;
        _Canvas.PointerReleased     += InputControl.PointerReleased;
    }

    /// <summary>
    /// Win2D アンロード処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void UserControl_Unloaded( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            // マウスイベントキャプチャ解放
            _Canvas.PointerPressed      -= InputControl.PointerPressed;
            _Canvas.PointerMoved        -= InputControl.PointerMoved;
            _Canvas.PointerExited       -= InputControl.PointerReleased;
            _Canvas.PointerCanceled     -= InputControl.PointerReleased;
            _Canvas.PointerCaptureLost  -= InputControl.PointerReleased;
            _Canvas.PointerReleased     -= InputControl.PointerReleased;

            // Win2D アンロード
            _Canvas.RemoveFromVisualTree();
            _Canvas = null;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

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
    public async Task DrawTaskAsync()
    {
        try
        {
            // FPS 制御はしない方がよさそう
            var fps = new Fps();
            fps.Set( 0 );
        //  fps.Set( Config.Panel.Fps );
            fps.Start();

            while ( !_FlagIdleTaskStop )
            {
                // サイズ変更
                if ( Config.Panel.FlagUpdateSize )
                {
                    Config.Panel.FlagUpdateSize = false;
                    UpdatePanelSize();
                }

                // プレイヤー描画モード変更
                if ( Config.Panel.FlagUpdateSurfaceModo )
                {
                    Config.Panel.FlagUpdateSurfaceModo = false;
                    UpdateSurfaceMode();

                    EventManage.Event_Player_UpdateScore();
                }

                // フレーム更新
                if ( fps.TickFpsWeight() )
                {
                    _CurrentSurface?.OnMove( fps.GetFrameTime() );

                    // 描画処理
                    using var cl = new CanvasCommandList( _Canvas.SwapChain );

                    using var drawSessionA = _Canvas.SwapChain.CreateDrawingSession( Config.Panel.SheetColor.Color );
                    using var drawSessionB = cl.CreateDrawingSession();

                    var args = new CanvasDrawEventArgs( drawSessionB );

                    _CurrentSurface?.OnDraw( args );

                    using var blur = new ScaleEffect
                    { 
                        Source  = cl,
                        Scale   = Config.Panel.ScreenMagnification,
                    };

                    if ( blur != null )
                    {
                        drawSessionA.DrawImage( blur );
                    }

                    _Canvas.SwapChain.Present();
                }

                await Task.Delay( 1 );
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
    /// 描画モード変更
    /// </summary>
    private void UpdateSurfaceMode()
    {
        switch ( Config.Panel.SurfaceModeSelect )
        {
            case ConfigPanel.SurfaceMode.SongSelect:
                _CurrentSurface = _SongSelectSurface ??= new ScreenSongSelect();
                break;
            //case ConfigPanel.SurfaceMode.Player_ScoreType2:
            //    _CurrentSurface = _PlayerSurface ??= new pScreen.pPlayer.pScoreType2.ScreenPlayer();
            //    break;
        }
    }

    /// <summary>
    /// 描画サイズ変更
    /// </summary>
    private void UpdatePanelSize()
    {
        _Canvas.SwapChain.ResizeBuffers
            (
                Config.Panel.ResolutionScreenWidth,
                Config.Panel.ResolutionScreenHeight,
                Config.Window.DefaultDpi
            );
    }

    #endregion
}
