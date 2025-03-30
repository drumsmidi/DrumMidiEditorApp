using System;
using System.Collections.Generic;
using System.Threading;
using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pControl;
using DrumMidiLibrary.pLog;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pMusic;

public sealed partial class UserControlEqualizer : UserControl
{
    #region Member

    /// <summary>
    /// イコライザ設定
    /// </summary>
    private ConfigEqualizer DrawSet => Config.Equalizer;

    /// <summary>
    /// イコライザ入力エリア
    /// </summary>
    private Rect _EqulizerBodyRange = new();

    /// <summary>
    /// 移動中のイコライザ入力インデックス
    /// </summary>
    private int _GainMoveIndex = -1;

    /// <summary>
    /// イコライザ入力（座標）
    /// </summary>
    private readonly List<Point> _GainCenters = [];

    /// <summary>
    /// 波形描画用のタイマー
    /// </summary>
    private PeriodicTimer? _Timer = null;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlEqualizer()
    {
        InitializeComponent();

        ControlAccess.UCEqualizer = this;

        // イコライザ描画範囲初期化
        UpdateRange();

        // イコライザ反映処理設定
        DmsControl.ApplyEqualizerCallback = ApplyEqulizer;
    }

    /// <summary>
    /// BGMへイコライザ入力内容を適用
    /// </summary>
    public void ApplyEqulizer()
    {
        if ( !HelperXaml.DispatcherQueueHasThreadAccess( this, ApplyEqulizer ) )
        {
            return;
        }

        UpdateEqualizerAudio();
    }

    /// <summary>
    /// イコライザの入力およびBGMのイコライザ設定をリセット
    /// </summary>
    public void ResetEqualizer()
    {
        if ( !HelperXaml.DispatcherQueueHasThreadAccess( this, ResetEqualizer ) )
        {
            return;
        }

        ResetEqualizerAudio();
        Refresh();
    }

    #region CommanBar Event

    /// <summary>
    /// イコライザリセット
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>" )]
    private void EqualizerResetAppBarButton_Click( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            ResetEqualizerAudio();
            Refresh();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// イコライザON/OFFの切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>" )]
    private void EqualizerAppBarToggleButton_CheckChanged( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            DrawSet.EqualizerOn = true;
            UpdateEqualizerAudio();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// イコライザON/OFFの切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>" )]
    private void EqualizerAppBarToggleButton_UncheckChanged( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            DrawSet.EqualizerOn = false;
            UpdateEqualizerAudio();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// WaveForm ON/OFFの切替
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>" )]
    private void EqualizerWaveFormAppBarToggleButton_CheckChanged( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            if ( aSender is not AppBarToggleButton item )
            {
                return;
            }

            var ischecked = item?.IsChecked ?? false ;

            // 波形描画用のタイマー設定
            if ( ischecked )
            {
                StartWaveFormAsync();
            }
            else
            {
                StopWaveForm();
            }
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// WaveForm描画開始
    /// </summary>
    private async void StartWaveFormAsync()
    {
        if ( _Timer != null )
        {
            return;
        }

        _Timer = new( TimeSpan.FromSeconds( DrawSet.WaveFormDrawInterval ) );

        while ( await _Timer.WaitForNextTickAsync() )
        {
            Refresh();
        }
    }

    /// <summary>
    /// WaveForm描画停止
    /// </summary>
    private void StopWaveForm()
    {
        _Timer?.Dispose();
        _Timer = null;

        Refresh();
    }

    #endregion

    #region Canvas Mouse Event

    /// <summary>
    /// マウスアクション
    /// </summary>
    private enum EActionState : int
    {
        None = 0,
        MovePoint,
        AddPoint,
        RemovePoint,
    }

    /// <summary>
    /// マウスアクション
    /// </summary>
    private EActionState _ActionState = EActionState.None;

    /// <summary>
    /// マウスダウン
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    private void EqualizerCanvas_PointerPressed( object aSender, PointerRoutedEventArgs aArgs )
    {
        if ( _ActionState != EActionState.None )
        {
            return;
        }

        try
        {
            var p = aArgs.GetCurrentPoint( aSender as FrameworkElement );

            if ( p.Properties.IsLeftButtonPressed )
            {
                if ( CheckEqulizerBodyArea( p.Position ) )
                {
                    _GainMoveIndex = GetGainCentersIndex( p.Position );

                    _ActionState = _GainMoveIndex != -1 ? EActionState.MovePoint : EActionState.AddPoint;

                    Refresh();
                }
            }
            else if ( p.Properties.IsRightButtonPressed )
            {
                if ( CheckEqulizerBodyArea( p.Position ) )
                {
                    _GainMoveIndex = GetGainCentersIndex( p.Position );

                    _ActionState = EActionState.RemovePoint;

                    Refresh();
                }
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
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
	private void EqualizerCanvas_PointerMoved( object aSender, PointerRoutedEventArgs aArgs )
    {
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
            var p = aArgs.GetCurrentPoint( aSender as FrameworkElement );

            switch ( _ActionState )
            {
                case EActionState.MovePoint:
                    {
                        EditPointMove( _GainMoveIndex, p.Position );
                        Refresh();
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
    /// マウスアップ
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
	private void EqualizerCanvas_PointerReleased( object aSender, PointerRoutedEventArgs aArgs )
    {
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
            var p = aArgs.GetCurrentPoint( aSender as FrameworkElement );

            switch ( _ActionState )
            {
                case EActionState.AddPoint:
                    {
                        if ( CheckEqulizerBodyArea( p.Position ) )
                        {
                            EditPoint( p.Position, true );

                            _GainMoveIndex = -1;

                            Refresh();
                        }
                    }
                    break;
                case EActionState.MovePoint:
                    {
                        EditPointMove( _GainMoveIndex, p.Position );

                        _GainMoveIndex = -1;

                        Refresh();
                    }
                    break;
                case EActionState.RemovePoint:
                    {
                        if ( CheckEqulizerBodyArea( p.Position ) )
                        {
                            EditPoint( p.Position, false );

                            _GainMoveIndex = -1;

                            Refresh();
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

    #endregion

    #region Check

    /// <summary>
    /// イコライザ入力範囲チェック
    /// </summary>
    /// <param name="aMousePos"></param>
    /// <returns>True:範囲内、False:範囲外</returns>
    private bool CheckEqulizerBodyArea( Point aMousePos )
        => HelperXaml.CheckRange( aMousePos, _EqulizerBodyRange );

    #endregion

    #region Calc

    /// <summary>
    /// マウス位置のイコライザ入力インデックスを取得
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>取得：0-、未取得：-1</returns>
    private int GetGainCentersIndex( Point aMousePos )
    {
        var pointRect = new Rect( 0, 0, DrawSet.PointSize, DrawSet.PointSize );

        for ( var index = _GainCenters.Count - 1; index >= 0; index-- )
        {
            var p = _GainCenters[ index ];

            pointRect.X = p.X - ( pointRect.Width  / 2 );
            pointRect.Y = p.Y - ( pointRect.Height / 2 );

            if ( HelperXaml.CheckRange( aMousePos, pointRect ) )
            {
                return index;
            }
        }
        return -1;
    }

    /// <summary>
    /// マウス位置の音量増減値取得
    /// </summary>
    /// <param name="aMousePosY">マウス位置</param>
    /// <returns>音量増減値</returns>
    private float CalcGain( double aMousePosY )
    {
        return (float)( DrawSet.DbGainMax -
                ( ( DrawSet.DbGainMax - DrawSet.DbGainMin ) *
                ( aMousePosY - _EqulizerBodyRange.Y ) / _EqulizerBodyRange.Height ) );
    }

    /// <summary>
    /// マウス位置のHz値取得
    /// </summary>
    /// <param name="aMousePosX">マウス位置</param>
    /// <returns>Hz値</returns>
    private float CalcHz( double aMousePosX )
    {
        float hz = 0;

        var x = (float)( aMousePosX - _EqulizerBodyRange.X );

        foreach ( var item in DrawSet.HzList )
        {
            if ( x == item.Width )
            {
                return item.Hz;
            }
            else if ( x < item.Width )
            {
                return hz + ( ( item.Hz - hz ) * ( x / item.Width ) );
            }

            hz = item.Hz;
            x -= item.Width;
        }

        return 0;
    }

    #endregion

    #region Edit

    /// <summary>
    /// イコライザ入力追加／削除
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <param name="aAddFlag">True:追加、False:削除</param>
    private void EditPoint( Point aMousePos, bool aAddFlag )
    {
        var idx = GetGainCentersIndex( aMousePos );

        if ( idx != -1 )
        {
            if ( aAddFlag )
            {
                _GainCenters [ idx ] = aMousePos;
            }
            else
            {
                _GainCenters.RemoveAt( idx );
            }
        }
        else
        {
            if ( aAddFlag )
            {
                _GainCenters.Add( aMousePos );
            }
        }

        if ( aAddFlag )
        {
            SetEqualizerInfo( aMousePos );
        }

        UpdateEqualizerAudio();
    }

    /// <summary>
    /// イコライザ入力移動
    /// </summary>
    /// <param name="aGearIndex">イコライザ入力インデックス</param>
    /// <param name="aMousePos">マウス位置</param>
    private void EditPointMove( int aGearIndex, Point aMousePos )
    {
        if ( aGearIndex == -1 )
        {
            return;
        }

        var p = HelperXaml.AdjustRangeIn( aMousePos, _EqulizerBodyRange );

        _GainCenters [ aGearIndex ] = p;

        SetEqualizerInfo( p );

        UpdateEqualizerAudio();
    }

    /// <summary>
    /// イコライザ入力情報ログ表示
    /// </summary>
    /// <param name="aPoint">イコライザ入力情報</param>
    private void SetEqualizerInfo( Point aGainInfo ) 
        => _EqualizerInfoTextBlock.Text = $"hz={CalcHz( aGainInfo.X )}   db={CalcGain( aGainInfo.Y )}";

    /// <summary>
    /// BGMイコライザ設定反映
    /// </summary>
    public void UpdateEqualizerAudio()
    {
        try
        {
            if ( DrawSet.EqualizerOn )
            {
                for ( var index = 0; index < _GainCenters.Count; index++ )
                {
                    var p = _GainCenters[ index ];

                    DmsControl.AudioData?.SetEqualizerGain( index, CalcHz( p.X ), CalcGain( p.Y ) );
                }
            }
            else
            {
                for ( var index = 0; index < _GainCenters.Count; index++ )
                {
                    var p = _GainCenters[ index ];

                    DmsControl.AudioData?.SetEqualizerGain( index, CalcHz( p.X ), 0 );
                }
            }

            DmsControl.AudioData?.UpdateEqualizer();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// イコライザ設定リセット
    /// </summary>
    public void ResetEqualizerAudio()
    {
        try
        {
            for ( var index = 0; index < _GainCenters.Count; index++ )
            {
                var p = _GainCenters[ index ];

                DmsControl.AudioData?.SetEqualizerGain( index, (float)CalcHz( p.X ), 0 );
            }

            DmsControl.AudioData?.UpdateEqualizer();

            _GainCenters.Clear();
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// フレーム処理
    /// </summary>
    private static void OnMove()
    {
    }

    /// <summary>
    /// 描画範囲更新
    /// </summary>
    private void UpdateRange()
    {
        // Equlizer body
        _EqulizerBodyRange.X        = DrawSet.EqulizerBodyMarginLeftTop.X;
        _EqulizerBodyRange.Y        = DrawSet.EqulizerBodyMarginLeftTop.Y;
        _EqulizerBodyRange.Width    = DrawSet.GetHzTotalWidth();
        _EqulizerBodyRange.Height   = DrawSet.DbTotalHeight;

        // イコライザキャンバスのサイズ調整
        _EqualizerCanvas.Width      = DrawSet.EqulizerBodyMarginLeftTop.X + _EqulizerBodyRange.Width  + DrawSet.EqulizerBodyMarginRightBottom.X;
        _EqualizerCanvas.Height     = DrawSet.EqulizerBodyMarginLeftTop.Y + _EqulizerBodyRange.Height + DrawSet.EqulizerBodyMarginRightBottom.Y;
    }

    #endregion

    #region Draw

    /// <summary>
    /// イコライザキャンバス描画更新
    /// </summary>
    public void Refresh() => _EqualizerCanvas.Invalidate();

    /// <summary>
    /// イコライザキャンバス描画
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>" )]
    private void EqualizerCanvas_Draw( CanvasControl aSender, CanvasDrawEventArgs aArgs )
    {
        try
        {
            OnMove();

            var body = _EqulizerBodyRange;

            #region 背景色
            {
                aArgs.DrawingSession.Clear( DrawSet.BackGround.Color );
            }
            #endregion

            #region x-line (Hz)
            {
                var pos_x = body.X;

                foreach ( var item in DrawSet.HzList )
                {
                    pos_x += item.Width;

                    aArgs.DrawingSession.DrawText
                        (
                            item.LabelName,
                            (float)pos_x,
                            (float)( body.Bottom + DrawSet.XLineTitlePaddingBottom ),
                            DrawSet.TextCenterRect.Text.TextColor.Color,
                            DrawSet.TextCenterRect.Text.TextFormat
                        );

                    aArgs.DrawingSession.DrawLine
                        (
                            (float)pos_x,
                            (float)body.Top,
                            (float)pos_x,
                            (float)( body.Bottom + DrawSet.XLineTitlePaddingBottom ),
                            DrawSet.Line.LineColor.Color,
                            DrawSet.Line.LineSize
                        );
                }
            }
            #endregion

            #region y-line (db)
            {
                var pos_y = body.Y;

                for ( var y = 0; y <= DrawSet.DbGainSeparateHeightCount; y++ )
                {
                    var db = DrawSet.DbGainMax - (DrawSet.DbGainSeparate * y);

                    aArgs.DrawingSession.DrawText
                        (
                            $"{db}db",
                            (float)( body.Left - DrawSet.YLineTitlePaddingRight ),
                            (float)pos_y,
                            DrawSet.TextRightRect.Text.TextColor.Color,
                            DrawSet.TextRightRect.Text.TextFormat
                        );

                    aArgs.DrawingSession.DrawLine
                        (
                            (float)( body.Left - DrawSet.YLineTitlePaddingRight ),
                            (float)pos_y,
                            (float)body.Right,
                            (float)pos_y,
                            DrawSet.Line.LineColor.Color,
                            DrawSet.Line.LineSize
                        );

                    pos_y += DrawSet.DbGainSeparateHeightTermSize;
                }
            }
            #endregion

            #region 周波数解析

            if ( DrawSet.WaveFormOn )
            {
                // TODO: 仮作成。MusicスレッドでBGMの再読み込み時にエラーになる場合があるので使う場合は改良が必要
                try
                {
                    var bgm = DmsControl.AudioData;

                    if ( bgm?.IsEnableFFT() ?? false )
                    {
                        lock ( bgm )
                        {
                            var fft = bgm.GetFFTPlaying();

                            var hzPerOne = (float)bgm.GetSampleRate() / ( fft.Count * bgm.Channels );

                            var pen = new FormatLine[ 2 ]
                                {
                                    DrawSet.WaveLeftLine,
                                    DrawSet.WaveRightLine,
                                };

                            var r = new Rect( 0, 0, 2, 1 );

                            for ( var k = 0; k < fft.Count; k++ )
                            {
                                var format = pen[ k % bgm.Channels ];

                                r.X         = body.X - ( k % bgm.Channels * format.LineSize );
                                r.Height    = fft [ k ] * body.Height;
                                r.Y         = body.Y + body.Height - ( r.Height > 0 ? r.Height : 0 );

                                var hz_b    = 0d;
                                var hz      = hzPerOne * ( k + 1 - (k % bgm.Channels) );

                                foreach ( var item in DrawSet.HzList )
                                {
                                    if ( hz > item.Hz )
                                    {
                                        r.X += item.Width;
                                        hz_b = item.Hz;
                                    }
                                    else
                                    {
                                        r.X += item.Width * ( hz - hz_b ) / ( item.Hz - hz_b );
                                        break;
                                    }
                                }

                                aArgs.DrawingSession.DrawLine
                                    (
                                        r._x,
                                        r._y,
                                        r._x,
                                        (float)r.Bottom,
                                        format.LineColor.Color,
                                        format.LineSize
                                    );
                            }
                        }
                    }
                }
                catch ( Exception )
                {
                    //Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
                }
            }
            #endregion

            #region point
            {
                for ( var index = 0; index < _GainCenters.Count; index++ )
                {
                    var point = _GainCenters[ index ];

                    var format = index == _GainMoveIndex ? DrawSet.PointSelectRect : DrawSet.PointNonSelectRect ;

                    aArgs.DrawingSession.DrawCircle
                        (
                            point._x,
                            point._y,
                            (float)( DrawSet.PointSize / 2d ),
                            format.Line.LineColor.Color,
                            format.Line.LineSize
                        );

                    aArgs.DrawingSession.FillCircle
                        (
                            point._x,
                            point._y,
                            (float)( DrawSet.PointSize / 2d ),
                            format.Background.Color
                        );
                }
            }
            #endregion
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// Win2D アンロード処理
    /// </summary>
    /// <param name="aSender"></param>
    /// <param name="aArgs"></param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Style", "IDE0060:未使用のパラメーターを削除します", Justification = "<保留中>" )]
    private void Page_Unloaded( object aSender, RoutedEventArgs aArgs )
    {
        try
        {
            // Win2D アンロード（ページキャッシュしているので処理しない）
            //_EqualizerCanvas.RemoveFromVisualTree();
            //_EqualizerCanvas = null;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    #endregion
}
