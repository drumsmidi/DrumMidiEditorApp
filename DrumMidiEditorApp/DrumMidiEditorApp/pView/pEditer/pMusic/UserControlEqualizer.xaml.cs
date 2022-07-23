using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using Windows.Foundation;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pControl;
using DrumMidiEditorApp.pGeneralFunction.pLog;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using Windows.UI;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pEditer;

public sealed partial class UserControlEqualizer : UserControl
{
    /// <summary>
    /// イコライザ設定
    /// </summary>
    private ConfigEqualizer _DrawSet => Config.Equalizer;

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
    private readonly List<Point> _GainCenters = new();

    /// <summary>
    /// 波形描画用のタイマー
    /// </summary>
    private readonly DispatcherTimer _Timer = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public UserControlEqualizer()
    {
        InitializeComponent();

        // イコライザ描画範囲初期化
        UpdateRange();

        // 波形描画更新用のタイマー設定
        _Timer.Interval  = TimeSpan.FromSeconds( _DrawSet.WaveFormDrawInterval );
        _Timer.Tick     += ( sender, ev ) => { RefreshEqualizerCanvas(); };

        if ( _DrawSet.WaveFormOn )
        { 
            _Timer.Start();
        }
    }

    /// <summary>
    /// BGMへイコライザ入力内容を適用
    /// </summary>
    public void ApplyEqulizer() => UpdateEqualizer();

	/// <summary>
	/// イコライザの入力およびBGMのイコライザ設定をリセット
	/// </summary>
	public void ResetEqulizer() 
	{
		ResetEqualizer();
		RefreshEqualizerCanvas();
	}

    #region CommanBar Event

    /// <summary>
    /// イコライザリセット
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void _EqualizerResetAppBarButton_Click( object sender, RoutedEventArgs args )
    {
		try
		{
			ResetEqulizer();
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

    /// <summary>
    /// イコライザON/OFFの切替
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void _EqualizerAppBarToggleButton_CheckChanged( object sender, RoutedEventArgs args )
    {
		try
		{
            _DrawSet.EqualizerOn = _EqualizerAppBarToggleButton.IsChecked ?? false ;

            ApplyEqulizer();
        }
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
	}

    /// <summary>
    /// WaveForm ON/OFFの切替
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void _EqualizerWaveFormAppBarToggleButton_CheckChanged( object sender, RoutedEventArgs args )
	{
		try
		{
            _DrawSet.WaveFormOn = _EqualizerWaveFormAppBarToggleButton.IsChecked ?? false ;
            
            // 波形描画用のタイマー設定
            if ( _DrawSet.WaveFormOn )
            {
                if ( !_Timer.IsEnabled )
                { 
                    _Timer.Start();
                }
            }
            else if  ( _Timer.IsEnabled )
            {
                _Timer.Stop();
            }
        }
        catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
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
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void _EqualizerCanvas_PointerPressed( object sender, PointerRoutedEventArgs args )
	{
        if ( _ActionState != EActionState.None )
        {
            return;
        }

        try
        {
            var p = args.GetCurrentPoint( _EqualizerCanvas );

            if ( p.Properties.IsLeftButtonPressed )
            {
                if ( CheckEqulizerBodyArea( p.Position ) )
                {
                    _GainMoveIndex = GetGainCentersIndex( p.Position );

                    if ( _GainMoveIndex != -1 )
                    {
                        _ActionState = EActionState.MovePoint;
                    }
                    else
                    {
                        _ActionState = EActionState.AddPoint;
                    }

                    RefreshEqualizerCanvas();
                }
            }
            else if ( p.Properties.IsRightButtonPressed )
            { 
                if ( CheckEqulizerBodyArea( p.Position ) )
                {
                    _GainMoveIndex = GetGainCentersIndex( p.Position );

                    _ActionState = EActionState.RemovePoint;

                    RefreshEqualizerCanvas();
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
    /// <param name="sender"></param>
    /// <param name="ev"></param>
	private void _EqualizerCanvas_PointerMoved( object sender, PointerRoutedEventArgs args )
	{
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
            var p = args.GetCurrentPoint( _EqualizerCanvas );

            switch ( _ActionState )
            {
                case EActionState.MovePoint:
                    {
                        EditPointMove( _GainMoveIndex, p.Position );
                        RefreshEqualizerCanvas();
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
    /// <param name="sender"></param>
    /// <param name="ev"></param>
	private void _EqualizerCanvas_PointerReleased( object sender, PointerRoutedEventArgs args )
	{
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
            var p = args.GetCurrentPoint( _EqualizerCanvas );

            switch ( _ActionState )
            {
                case EActionState.AddPoint:
                    {
                        if ( CheckEqulizerBodyArea( p.Position ) )
                        {
                            EditPoint( p.Position, true );

                            _GainMoveIndex = -1;

                            RefreshEqualizerCanvas();
                        }
                    }
                    break;
                case EActionState.MovePoint:
                    {
                        EditPointMove( _GainMoveIndex, p.Position );

                        _GainMoveIndex = -1;

                        RefreshEqualizerCanvas();
                    }
                    break;
                case EActionState.RemovePoint:
                    {
                        if ( CheckEqulizerBodyArea( p.Position ) )
                        {
                            EditPoint( p.Position, false );

                            _GainMoveIndex = -1;

                            RefreshEqualizerCanvas();
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
        => XamlHelper.CheckRange( aMousePos, _EqulizerBodyRange );

    #endregion

    #region Calc

    /// <summary>
    /// マウス位置のイコライザ入力インデックスを取得
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>取得：0-、未取得：-1</returns>
    private int GetGainCentersIndex( Point aMousePos )
    {
        var pointRect = new Rect( 0, 0, _DrawSet.PointSize, _DrawSet.PointSize );

        for ( int index = _GainCenters.Count - 1; index >= 0; index-- )
        {
            var p = _GainCenters[ index ];

            pointRect.X = p.X - pointRect.Width  / 2;
            pointRect.Y = p.Y - pointRect.Height / 2;

            if ( XamlHelper.CheckRange( aMousePos, pointRect ) )
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
    private double CalcGain( double aMousePosY )
    {
        return _DrawSet.DbGainMax -
                ( _DrawSet.DbGainMax - _DrawSet.DbGainMin ) *
                ( aMousePosY - _EqulizerBodyRange.Y ) / _EqulizerBodyRange.Height;
    }

    /// <summary>
    /// マウス位置のHz値取得
    /// </summary>
    /// <param name="aMousePosX">マウス位置</param>
    /// <returns>Hz値</returns>
    private double CalcHz( double aMousePosX )
    {
        double hz = 0;

        double x = aMousePosX - _EqulizerBodyRange.X;

        foreach ( var item in _DrawSet.HzList )
        {
            if ( x == item.Width )
            {
                return item.Hz;
            }
            else if ( x < item.Width )
            {
                return hz + ( item.Hz - hz ) * ( x / item.Width );
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
                _GainCenters[ idx ] = aMousePos;
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

        UpdateEqualizer();
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

        _GainCenters[ aGearIndex ] = XamlHelper.AdjustRangeIn( aMousePos, _EqulizerBodyRange );

        UpdateEqualizer();
    }

    /// <summary>
    /// BGMイコライザ設定反映
    /// </summary>
    public void UpdateEqualizer()
	{
		try
		{
            if ( _DrawSet.EqualizerOn )
			{
				for ( int index = 0; index < _GainCenters.Count; index++ )
				{
                    var p = _GainCenters[ index ];

                    DmsControl.AudioData?.SetEqualizerGain( index, (float)CalcHz( p.X ), (float)CalcGain( p.Y ) );
                }
            }
            else
			{
                for ( int index = 0; index < _GainCenters.Count; index++ )
			    {
                    var p = _GainCenters[ index ];

                    DmsControl.AudioData?.SetEqualizerGain( index, (float)CalcHz( p.X ), 0 );
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
    public void ResetEqualizer()
    {
        try
        {
            for ( int index = 0; index < _GainCenters.Count; index++ )
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
    private void OnMove()
    {
    }

    /// <summary>
    /// 描画範囲更新
    /// </summary>
    private void UpdateRange()
	{
        // Equlizer body
        _EqulizerBodyRange.X		= _DrawSet.EqulizerBodyMarginLeftTop.X;
        _EqulizerBodyRange.Y		= _DrawSet.EqulizerBodyMarginLeftTop.Y;
        _EqulizerBodyRange.Width	= _DrawSet.GetHzTotalWidth();
        _EqulizerBodyRange.Height	= _DrawSet.GetDbTotalHeight();

        // イコライザキャンバスのサイズ調整
        _EqualizerCanvas.Width      = _DrawSet.EqulizerBodyMarginLeftTop.X + _EqulizerBodyRange.Width  + _DrawSet.EqulizerBodyMarginRightBottom.X;
        _EqualizerCanvas.Height     = _DrawSet.EqulizerBodyMarginLeftTop.Y + _EqulizerBodyRange.Height + _DrawSet.EqulizerBodyMarginRightBottom.Y;
	}

    #endregion

    #region Draw

    /// <summary>
    /// イコライザキャンバス描画更新
    /// </summary>
    public void RefreshEqualizerCanvas() => _EqualizerCanvas.Invalidate();

    /// <summary>
    /// Win2D アンロード処理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Page_Unloaded( object sender, RoutedEventArgs args )
    {
        try
        {
            // Win2D アンロード
            _EqualizerCanvas.RemoveFromVisualTree();
            _EqualizerCanvas = null;
        }
        catch ( Exception e )
        {
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
        }
    }

    /// <summary>
    /// イコライザキャンバス描画
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void _EqualizerCanvas_Draw( CanvasControl sender, CanvasDrawEventArgs args )
    {
        try
        {
            OnMove();

            var body = _EqulizerBodyRange;

            #region x-line (Hz)
            {
                var pos_x = body.X;

                foreach ( var item in _DrawSet.HzList )
                {
                    pos_x += item.Width;

                    args.DrawingSession.DrawText
                        (
                            item.LabelName,
                            (float)( pos_x ),
                            (float)( body.Bottom + _DrawSet.XLineTitlePaddingBottom ),
                            _DrawSet.TextColor,
                            _DrawSet.TextFormatCenter
                        );

                    args.DrawingSession.DrawLine
                        (
                            (float)( pos_x ),
                            (float)( body.Top ),
                            (float)( pos_x ),
                            (float)( body.Bottom + _DrawSet.XLineTitlePaddingBottom ),
                            _DrawSet.LineColor
                        );
                }
            }
            #endregion

            #region y-line (db)
            {
                var pos_y = body.Y;

                for ( int y = 0; y <= _DrawSet.DbGainSeparateHeightCount; y++ )
                {
                    var db = _DrawSet.DbGainMax - _DrawSet.DbGainSeparate * y;

                    args.DrawingSession.DrawText
                        (
                            $"{db}db",
                            (float)( body.Left - _DrawSet.YLineTitlePaddingRight ),
                            (float)( pos_y ),
                            _DrawSet.TextColor,
                            _DrawSet.TextFormatRight
                        );

                    args.DrawingSession.DrawLine
                        (
                            (float)( body.Left - _DrawSet.YLineTitlePaddingRight ),
                            (float)( pos_y ),
                            (float)( body.Right ),
                            (float)( pos_y ),
                            _DrawSet.LineColor
                        );

                    pos_y += _DrawSet.DbGainSeparateHeightTermSize;
                }
            }
            #endregion

            #region 周波数解析

            // TODO: 仮作成。BGMの再読み込み時にエラーになるので使う場合は改良が必要

            if ( _DrawSet.WaveFormOn )
            {
                var bgm = DmsControl.AudioData;

                if ( bgm?.IsEnableFFT() ?? false )
                {
                    var fft = bgm.GetFFTPlaying();

                    var hzPerOne = (float)bgm.GetSampleRate() / ( fft.Count * bgm.Channels );

                    var pen = new Color[ 2 ]
                        {
                            _DrawSet.WaveLeftColor,
                            _DrawSet.WaveRightColor,
                        };

                    var r = new Rect( 0, 0, 2, 1 );

                    for ( int k = 0; k < fft.Count; k++ )
                    {
                        r.X         = body.X - k % bgm.Channels;
                        r.Height    = fft[k] * body.Height;
                        r.Y         = body.Y + body.Height - ( r.Height > 0 ? r.Height : 0 );

                        var hz_b    = 0d;
                        var hz      = hzPerOne * ( k + 1 - k % bgm.Channels );

                        foreach ( var item in _DrawSet.HzList )
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

                        args.DrawingSession.DrawLine
                            (
                                r._x,
                                r._y,
                                r._x,
                                (float)r.Bottom,
                                pen[ k % bgm.Channels ]
                            );
                    }
                }
            }
            #endregion

            #region point
            {
                for ( int index = 0; index < _GainCenters.Count; index++ )
                {
                    var point = _GainCenters[index];

                    args.DrawingSession.DrawCircle
                        (
                            point._x,
                            point._y,
                            (float)( _DrawSet.PointSize / 2d ),
                            _DrawSet.PointColor
                        );

                    args.DrawingSession.FillCircle
                        (
                            point._x,
                            point._y,
                            (float)( _DrawSet.PointSize / 2d ),
                            index == _GainMoveIndex ? _DrawSet.PointSelectColor : _DrawSet.PointNonSelectColor
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

    #endregion
}
