using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pControl;
using DrumMidiEditor.pGeneralFunction.pLog;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pEditer.pMusic;

/// <summary>
/// イコライザパネル
/// </summary>
public class EqualizerPanel : Panel
{
    /// <summary>
    /// マウスアクション
    /// </summary>
    private enum EActionState
    {
        None = 0
    ,   MovePoint
    ,   AddPoint
    ,   RemovePoint
    }

    /// <summary>
    /// マウスアクション
    /// </summary>
    private EActionState _ActionState = EActionState.None;

    /// <summary>
    /// パネルサイズ
    /// </summary>
    private Size _ScreenSize = new();

    /// <summary>
    /// イコライザ入力エリア
    /// </summary>
    private Rectangle _EqulizerBodyRange = new();

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
    private readonly Timer _Timer = new();

    /// <summary>
    /// イコライザ設定
    /// </summary>
    private static ConfigEqualizer DrawSet => Config.Equalizer;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EqualizerPanel()
    {
        SetStyle( ControlStyles.ResizeRedraw,           true );
        SetStyle( ControlStyles.OptimizedDoubleBuffer,  true );
        SetStyle( ControlStyles.UserPaint,              true );
        SetStyle( ControlStyles.AllPaintingInWmPaint,   true );
        SetStyle( ControlStyles.DoubleBuffer,           true ); 
        SetStyle( ControlStyles.Opaque,                 true );

		SuspendLayout();
		{
			Margin  = new Padding( 0 );
			Padding = new Padding( 0 );
		}
		ResumeLayout( false );

        UpdateRange();

        MouseDown	+= new( SheetMouseDown );
		MouseMove	+= new( SheetMouseMove );
		MouseUp	    += new( SheetMouseUp );

        _Timer.Interval     = DrawSet.WaveFormDrawInterval;
        _Timer.Enabled      = DrawSet.WaveFormOn;
        _Timer.Tick        += ( sender, ev ) => { Refresh(); };
        _Timer.Start();
    }

    #region Mouse Event

    /// <summary>
    /// マウスダウン
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
    private void SheetMouseDown( object? sender, MouseEventArgs ev )
	{
        if ( _ActionState != EActionState.None )
        {
            return;
        }

        try
        {
			switch ( ev.Button )
			{
				case MouseButtons.Left:
					{
                        if ( CheckEqulizerBodyArea( ev.Location ) )
                        {
                            _GainMoveIndex = GetGainCentersIndex( ev.Location );

                            if ( _GainMoveIndex != -1 )
                            {
    			                _ActionState = EActionState.MovePoint;
                            }
                            else
                            {
                                _ActionState = EActionState.AddPoint;
                            }

                            Invalidate( true );
                        }
					}
					break;

				case MouseButtons.Right:
					{
                        if ( CheckEqulizerBodyArea( ev.Location ) )
                        {
                            _GainMoveIndex = GetGainCentersIndex( ev.Location );

                            _ActionState = EActionState.RemovePoint;

                            Invalidate( true );
                        }
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
	private void SheetMouseMove( object? sender, MouseEventArgs ev )
	{
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
			switch ( ev.Button )
			{
				case MouseButtons.Left:
					{
						switch ( _ActionState )
						{
                            case EActionState.MovePoint:
                                {
                                    EditPointMove( _GainMoveIndex, ev.Location );
                                    Invalidate( true );
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

            _ActionState = EActionState.None;
        }
	}

    /// <summary>
    /// マウスアップ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="ev"></param>
	private void SheetMouseUp( object? sender, MouseEventArgs ev )
	{
        if ( _ActionState == EActionState.None )
        {
            return;
        }

        try
        {
			switch ( ev.Button )
			{
				case MouseButtons.Left:
					{
                        switch ( _ActionState )
                        {
                            case EActionState.AddPoint:
                                {
                                    if ( CheckEqulizerBodyArea( ev.Location ) )
                                    {
                                        EditPoint( ev.Location, true );

                                        _GainMoveIndex = -1;

                                        Invalidate( true );
                                    }
                                }
                                break;
                            case EActionState.MovePoint:
                                {
                                    EditPointMove( this._GainMoveIndex, ev.Location );

                                    _GainMoveIndex = -1;

                                    Invalidate( true );
                                }
                                break;
                        }
                    }
                    break;
				case MouseButtons.Right:
					{
                        switch ( _ActionState )
                        {
                            case EActionState.RemovePoint:
                                {
                                    if ( CheckEqulizerBodyArea( ev.Location ) )
                                    {
                                        EditPoint( ev.Location, false );

                                        _GainMoveIndex = -1;

                                        Invalidate( true );
                                    }
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

    #endregion

    #region Check

    /// <summary>
    /// イコライザ入力範囲チェック
    /// </summary>
    /// <param name="aMousePos"></param>
    /// <returns>True:範囲内、False:範囲外</returns>
    private bool CheckEqulizerBodyArea( Point aMousePos )
        => FormUtil.CheckRange( aMousePos, _EqulizerBodyRange );

    #endregion

    #region Calc

    /// <summary>
    /// マウス位置のイコライザ入力インデックスを取得
    /// </summary>
    /// <param name="aMousePos">マウス位置</param>
    /// <returns>取得：0-、未取得：-1</returns>
    private int GetGainCentersIndex( Point aMousePos )
    {
        var r = new Rectangle( 0, 0, DrawSet.PointSize, DrawSet.PointSize );

        for ( int index = _GainCenters.Count - 1; index >= 0; index-- )
        {
            var p = _GainCenters[ index ];

            r.X = p.X - r.Width  / 2;
            r.Y = p.Y - r.Height / 2;

            if ( FormUtil.CheckRange( aMousePos, r ) )
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
    private float CalcGain( int aMousePosY )
    {
        return DrawSet.DbGainMax -
                ( DrawSet.DbGainMax - DrawSet.DbGainMin ) *
                ( aMousePosY - _EqulizerBodyRange.Y ) / _EqulizerBodyRange.Height;
    }

    /// <summary>
    /// マウス位置のHz値取得
    /// </summary>
    /// <param name="aMousePosX">マウス位置</param>
    /// <returns>Hz値</returns>
    private float CalcHz( int aMousePosX )
    {
        float hz = 0;

        float x = aMousePosX - _EqulizerBodyRange.X;

        foreach ( var item in DrawSet.HzList )
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

        _GainCenters[ aGearIndex ] = FormUtil.CheckRangeIn( aMousePos, _EqulizerBodyRange );

        UpdateEqualizer();
    }

    /// <summary>
    /// BGMイコライザ設定反映
    /// </summary>
    public void UpdateEqualizer()
	{
		try
		{
            if ( DrawSet.EqualizerOn )
			{
				for ( int index = 0; index < _GainCenters.Count; index++ )
				{
                    var p = _GainCenters[ index ];

                    DmsControl.AudioData?.SetEqualizerGain( index, CalcHz( p.X ), CalcGain( p.Y ) );
                }
            }
            else
			{
                for ( int index = 0; index < _GainCenters.Count; index++ )
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
    public void ResetEqualizer()
    {
        try
        {
            for ( int index = 0; index < _GainCenters.Count; index++ )
			{
                var p = _GainCenters[ index ];

                DmsControl.AudioData?.SetEqualizerGain( index, CalcHz( p.X ), 0 );
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
        // 波形表示
        if ( _Timer.Enabled != DrawSet.WaveFormOn )
        { 
            if ( DrawSet.WaveFormOn )
            {
                _Timer.Interval = DrawSet.WaveFormDrawInterval;
                _Timer.Start();
            }
            else
            {
                _Timer.Stop();
            }
        }
    }

    /// <summary>
    /// 描画範囲更新
    /// </summary>
    private void UpdateRange()
	{
		// Screen
        _ScreenSize.Height			= Height;
        _ScreenSize.Width			= Width;

        // Equlizer body
        _EqulizerBodyRange.X		= DrawSet.EqulizerPaddingLeftTop.X;
        _EqulizerBodyRange.Y		= DrawSet.EqulizerPaddingLeftTop.Y;
        _EqulizerBodyRange.Width	= DrawSet.GetHzTotalWidth();
        _EqulizerBodyRange.Height	= DrawSet.GetDbTotalHeight();
	}

    #endregion

    #region Draw

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphic"></param>
    private void OnDraw( Graphics aGraphic )
    {
        var body = _EqulizerBodyRange;

        #region x-line (Hz)
        {
            var pos_x = body.X;

            foreach ( var item in DrawSet.HzList )
            {
                pos_x += item.Width;

                aGraphic.DrawString
                    (
                        item.LabelName,
                        DrawSet.StrFont,
                        DrawSet.StrBrush,
                        pos_x,
                        body.Bottom + DrawSet.XLineTitlePaddingBottom,
                        DrawSet.StrFmtCenter
                    );

                aGraphic.DrawLine
                    (
                        DrawSet.LinePen,
                        pos_x,
                        body.Top,
                        pos_x,
                        body.Bottom + DrawSet.XLineTitlePaddingBottom
                    );
            }
        }
        #endregion

        #region y-line (db)
        {
            var pos_y = body.Y;

            for ( int y = 0; y <= DrawSet.DbGainSeparateHeightCount; y++ )
            {
                var db = DrawSet.DbGainMax - DrawSet.DbGainSeparate * y;

                aGraphic.DrawString
                    (
                        db.ToString() + "db",
                        DrawSet.StrFont,
                        DrawSet.StrBrush,
                        body.Left - DrawSet.YLineTitlePaddingRight,
                        pos_y,
                        DrawSet.StrFmtRight
                    );

                aGraphic.DrawLine
                    (
                        DrawSet.LinePen,
                        body.Left - DrawSet.YLineTitlePaddingRight,
                        pos_y,
                        body.Right,
                        pos_y
                    );

                pos_y += DrawSet.DbGainSeparateHeightTermSize;
            }
        }
        #endregion

        #region 周波数解析

        if ( DrawSet.WaveFormOn )
        {
            var bgm = DmsControl.AudioData;

            if ( bgm?.IsEnableFFT() ?? false )
            { 
                var fft = bgm.GetFFTPlaying();

                var hzPerOne = (float)bgm.GetSampleRate() / ( fft.Count * bgm.Channels );

                var pen = new Pen[ 2 ];
                pen[ 0 ] = DrawSet.WaveLeftPen;
                pen[ 1 ] = DrawSet.WaveRightPen;

                var r = new RectangleF( 0, 0, 2, 1 );

                for ( int k = 0; k < fft.Count; k++ )
                {
                    r.X         = body.X - k % bgm.Channels;
                    r.Height    = fft[ k ] * body.Height;
                    r.Y         = body.Y + body.Height - ( r.Height > 0 ? r.Height : 0 ) ;

                    var hz_b    = 0;
                    var hz      = hzPerOne * ( k + 1 - k % bgm.Channels );

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

                    aGraphic.DrawLine( pen[ k % bgm.Channels ], r.X, r.Y, r.X, r.Bottom );
                }
            }
        }

        #endregion

        #region point
        {
            var r = new Rectangle( 0, 0, DrawSet.PointSize, DrawSet.PointSize );

            for ( int index = 0; index < _GainCenters.Count; index++ )
            {
                var p = _GainCenters[ index ];

                r.X = p.X - r.Width  / 2;
                r.Y = p.Y - r.Height / 2;

                aGraphic.DrawEllipse( DrawSet.PointPen, r );

                aGraphic.FillEllipse
                    ( 
                        index == _GainMoveIndex ? DrawSet.PointSelectBrush : DrawSet.PointNonSelectBrush, 
                        r 
                    );
            }
        }
        #endregion
    }

    /// <summary>
    /// 描画イベント
    /// </summary>
    /// <param name="ev"></param>
    protected override void OnPaint( PaintEventArgs ev )
    {
		try
		{
            OnMove();
			OnDraw( ev.Graphics );
		}
		catch ( Exception e )
		{
            Log.Error( $"{Log.GetThisMethodName}:{e.Message}" );
		}
    }

    #endregion
}
