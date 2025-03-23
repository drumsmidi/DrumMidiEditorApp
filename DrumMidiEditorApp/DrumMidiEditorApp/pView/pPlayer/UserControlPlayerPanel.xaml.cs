using System;
using System.Threading.Tasks;
using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pEvent;
using DrumMidiEditorApp.pView.pPlayer.pSurface;
using DrumMidiLibrary.pConfig;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Graphics.DirectX;

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

                using var blur = GetEffectImage( cl );
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
    /// エフェクト
    /// </summary>
    /// <param name="aCanvasCommandList"></param>
    /// <returns></returns>
    private static ICanvasImage? GetEffectImage( CanvasCommandList aCanvasCommandList )
    {
        return new AtlasEffect
            {
                Source              = aCanvasCommandList,
            //  PaddingRectangle    = new( 0, 0, 400, 8000 ),
            //  SourceRectangle     = new( 0, 0, 400, 700 ),
            };

#if false // 無効化
        // https://microsoft.github.io/Win2D/WinUI2/html/N_Microsoft_Graphics_Canvas_Effects.htm

        ICanvasImage? blur = null;
        switch ( DrawSet.PlayerSurfaceEffectModeSelect )
        {
            case ConfigPlayer.PlayerSurfaceEffectMode.AlphaMaskEffect:
                {
                    blur = new AlphaMaskEffect
                    {
                        Source      = aCanvasCommandList,
                        AlphaMask   = aCanvasCommandList,
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ArithmeticCompositeEffect:
                {
                    blur = new ArithmeticCompositeEffect
                    {
                        Source1         = aCanvasCommandList,
                        Source2         = aCanvasCommandList,
                        MultiplyAmount  = 0.5f, // 0.5で均等に合成
                        Source1Amount   = 0.5f,  // 1.0で画像1を完全に表示
                        Source2Amount   = 0.5f,   // 1.0で画像2を完全に表示
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.AtlasEffect:
                {
                    blur = new AtlasEffect
                    {
                        Source = aCanvasCommandList,
                        //PaddingRectangle = new( 0, 0, 400, 8000 ),
                        //SourceRectangle = new( 0, 0, 400, 700 ),
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.BlendEffect:
                {
                    blur = new BlendEffect
                    {
                        Background = aCanvasCommandList,
                        Foreground = aCanvasCommandList,
                        Mode       = BlendEffectMode.Screen,
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.BorderEffect:
                {
                    blur = new BorderEffect
                    {
                        Source = aCanvasCommandList,
                        ExtendX = CanvasEdgeBehavior.Wrap,
                        ExtendY = CanvasEdgeBehavior.Clamp,
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.BrightnessEffect:
                {
                    blur = new BrightnessEffect
                    {
                        Source = aCanvasCommandList,
                        BlackPoint = new(0.3f,0.5f),
                    //    WhitePoint = new(0.8f,08f),
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ChromaKeyEffect:
                {
                    blur = new ChromaKeyEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ColorManagementEffect:
                {
                    blur = new ColorManagementEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ColorManagementProfile:
                {
                    //blur = new ColorManagementProfile
                    //{
                    //};
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ColorMatrixEffect:
                {
                    blur = new ColorMatrixEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ColorSourceEffect:
                {
                    blur = new ColorSourceEffect
                    {
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.CompositeEffect:
                {
                    blur = new CompositeEffect
                    {
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ContrastEffect:
                {
                    blur = new ContrastEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ConvolveMatrixEffect:
                {
                    blur = new ConvolveMatrixEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.CropEffect:
                {
                    blur = new CropEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.CrossFadeEffect:
                {
                    blur = new CrossFadeEffect
                    {
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.DirectionalBlurEffect:
                {
                    blur = new DirectionalBlurEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.DiscreteTransferEffect:
                {
                    blur = new DiscreteTransferEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.DisplacementMapEffect:
                {
                    blur = new DisplacementMapEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.DistantDiffuseEffect:
                {
                    blur = new DistantDiffuseEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.DistantSpecularEffect:
                {
                    blur = new DistantSpecularEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.DpiCompensationEffect:
                {
                    blur = new DpiCompensationEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.EdgeDetectionEffect:
                {
                    blur = new EdgeDetectionEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.EffectTransferTable3D:
                {
                    //blur = new EffectTransferTable3D
                    //{
                    //};
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.EmbossEffect:
                {
                    blur = new EmbossEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ExposureEffect:
                {
                    blur = new ExposureEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.GammaTransferEffect:
                {
                    blur = new GammaTransferEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.GaussianBlurEffect:
                {
                    blur = new GaussianBlurEffect
                    {
                        Source = aCanvasCommandList,
                        BlurAmount = 1,
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.GrayscaleEffect:
                {
                    blur = new GrayscaleEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.HighlightsAndShadowsEffect:
                {
                    blur = new HighlightsAndShadowsEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.HueRotationEffect:
                {
                    blur = new HueRotationEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.HueToRgbEffect:
                {
                    blur = new HueToRgbEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.InvertEffect:
                {
                    blur = new InvertEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.LinearTransferEffect:
                {
                    blur = new LinearTransferEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.LuminanceToAlphaEffect:
                {
                    blur = new LuminanceToAlphaEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.MorphologyEffect:
                {
                    blur = new MorphologyEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.OpacityEffect:
                {
                    blur = new OpacityEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.OpacityMetadataEffect:
                {
                    blur = new OpacityMetadataEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.PixelShaderEffect:
                {
                    //blur = new PixelShaderEffect
                    //{
                    //};
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.PointDiffuseEffect:
                {
                    blur = new PointDiffuseEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.PointSpecularEffect:
                {
                    blur = new PointSpecularEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.PosterizeEffect:
                {
                    blur = new PosterizeEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.PremultiplyEffect:
                {
                    blur = new PremultiplyEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.RgbToHueEffect:
                {
                    blur = new RgbToHueEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.SaturationEffect:
                {
                    blur = new SaturationEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ScaleEffect:
                {
                    blur = new ScaleEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.SepiaEffect:
                {
                    blur = new SepiaEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.ShadowEffect:
                {
                    blur = new ShadowEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.SharpenEffect:
                {
                    blur = new SharpenEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.SpotDiffuseEffect:
                {
                    blur = new SpotDiffuseEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.SpotSpecularEffect:
                {
                    blur = new SpotSpecularEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.StraightenEffect:
                {
                    blur = new StraightenEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.TableTransfer3DEffect:
                {
                    blur = new TableTransfer3DEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.TableTransferEffect:
                {
                    blur = new TableTransferEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.TemperatureAndTintEffect:
                {
                    blur = new TemperatureAndTintEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.TileEffect:
                {
                    blur = new TileEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.TintEffect:
                {
                    blur = new TintEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.Transform2DEffect:
                {
                    blur = new Transform2DEffect
                    {
                        Source = aCanvasCommandList,
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.Transform3DEffect:
                {
                    blur = new Transform3DEffect
                    {
                        Source              = aCanvasCommandList,
                        TransformMatrix     = Matrix4x4.CreateRotationX( 45.0f, new Vector3( 0, 1024, 0 ) ),
                        BorderMode          = EffectBorderMode.Soft,
                        InterpolationMode   = CanvasImageInterpolation.MultiSampleLinear,
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.TurbulenceEffect:
                {
                    blur = new TurbulenceEffect
                    {
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.UnPremultiplyEffect:
                {
                    blur = new UnPremultiplyEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
            case ConfigPlayer.PlayerSurfaceEffectMode.VignetteEffect:
                {
                    blur = new VignetteEffect
                    {
                        Source = aCanvasCommandList,
                        Amount = 0.5F,
                        Color  = Windows.UI.Color.FromArgb(255,255,255,255),
                        Curve  = 0.5F,
                    };
                }
                break;
            default:
                {
                    blur = new AtlasEffect
                    {
                        Source = aCanvasCommandList
                    };
                }
                break;
        }
        return blur;            
#endif
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
                    Config.Media.DefaultDpi
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
            _ = ( _PlayerSurface?.OnMove( aFrameTime ) );

            using var cl = new CanvasCommandList( _Offscreen );

            //// 描画処理
            using var drawSessionA = _Offscreen.CreateDrawingSession();
            using var drawSessionB = cl.CreateDrawingSession();

            var args = new CanvasDrawEventArgs( drawSessionB );

            _ = ( _PlayerSurface?.OnDraw( args ) );

            drawSessionA.Clear( DrawSet.SheetColor.Color );

            using var blur = GetEffectImage( cl );
            if ( blur != null )
            {
                drawSessionA.DrawImage( blur );
            }

            // Bitmap作成
            return CanvasBitmap.CreateFromBytes
                (
                    drawSessionA,
                    _Offscreen.GetPixelBytes( 0, 0, (int)_Offscreen.SizeInPixels.Width, (int)_Offscreen.SizeInPixels.Height ),
                    (int)_Offscreen.SizeInPixels.Width,
                    (int)_Offscreen.SizeInPixels.Height,
                    DirectXPixelFormat.R8G8B8A8UIntNormalized,
                    Config.Media.DefaultDpi,
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

    /// <summary>
    /// 1ページ当たりの小節描画数取得
    /// </summary>
    /// <returns>1ページ当たりの小節描画数</returns>
    public int GetNumberOfMeasureNoPerPage()
        => _PlayerSurface?.GetNumberOfMeasureNoPerPage() ?? 0 ;

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
            case ConfigSystem.PlayerSurfaceMode.Simuration:
                _PlayerSurface = new pSurface.pSimuration.PlayerSurface();
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
