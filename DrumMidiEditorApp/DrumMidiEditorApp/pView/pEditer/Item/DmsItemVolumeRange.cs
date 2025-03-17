using System;
using System.Collections.Generic;
using DrumMidiEditorApp.pAudio;
using DrumMidiEditorApp.pUtil;
using DrumMidiEditorApp.pConfig;
using Windows.Foundation;
using Microsoft.Graphics.Canvas;
using DrumMidiEditorApp.pUtil.pHelper;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：音量選択範囲
/// </summary>
public partial class DmsItemVolumeRange : DisposeBaseClass
{
    /// <summary>
    /// 表示フラグ
    /// </summary>
    private bool _Display = false;

    /// <summary>
    /// 入力方向（True:左から右、False:右から左）
    /// </summary>
    private bool _Direct = false;

    /// <summary>
    /// 音量入力開始ノート位置（絶対値）
    /// </summary>
    private int _StartNotePosX = 0;

    /// <summary>
    /// 音量入力終了ノート位置（絶対値）
    /// </summary>
    private int _EndNotePosX = 0;

    /// <summary>
    /// 音量入力 終了音量
    /// </summary>
    private int _EndVolume = 0;

    /// <summary>
    /// 音量入力リスト
    /// </summary>
    private readonly List<int> _VolumeList = [];

    /// <summary>
    /// シート左下表示位置
    /// </summary>
    private Point _BottomPoint = new();

    /// <summary>
    /// 音量入力タイプ
    /// </summary>
    public ConfigEditer.VolumeEditType EditType
    {
        get; private set;
    } = ConfigEditer.VolumeEditType.FreeHand;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _VolumeList.Clear();
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// 音量入力 開始音量
    /// </summary>
    public int StartVolume { get; private set; } = 0;

    /// <summary>
    /// 左から右順の入力音量リスト。
    /// 音量増減系の入力の場合、音量増減値を出力します。
    /// </summary>
    public List<int> VolumeList
    {
        get
        {
            var lst = new List<int>();

            switch ( EditType )
            {
                case ConfigEditer.VolumeEditType.UpDown:
                case ConfigEditer.VolumeEditType.IntonationHL:
                case ConfigEditer.VolumeEditType.IntonationH:
                case ConfigEditer.VolumeEditType.IntonationL:
                    {
                        if ( _Direct )
                        {
                            foreach ( var v in _VolumeList )
                            {
                                lst.Add( _EndVolume - v );
                            }
                        }
                        else
                        {
                            for ( var i = _VolumeList.Count - 1; i >= 0; i-- )
                            {
                                lst.Add( _EndVolume - _VolumeList [ i ] );
                            }
                        }
                    }
                    break;
                default:
                    {
                        if ( _Direct )
                        {
                            foreach ( var v in _VolumeList )
                            {
                                lst.Add( v );
                            }
                        }
                        else
                        {
                            for ( var i = _VolumeList.Count - 1; i >= 0; i-- )
                            {
                                lst.Add( _VolumeList [ i ] );
                            }
                        }
                    }
                    break;
            }
            return lst;
        }
    }

    /// <summary>
    /// 音量入力開始から終了間のノート数
    /// </summary>
    public int TermNotePosX
        => Math.Abs( _StartNotePosX - _EndNotePosX );

    /// <summary>
    /// 音量入力左端ノート位置（絶対値）
    /// </summary>
    public int StartNotePosX
        => Math.Min( _StartNotePosX, _EndNotePosX );

    /// <summary>
    /// 音量入力右端ノート位置（絶対値）
    /// </summary>
    public int EndNotePosX
        => Math.Max( _StartNotePosX, _EndNotePosX );

    /// <summary>
    /// シート左下位置設定
    /// </summary>
    /// <param name="aX">表示位置X座標</param>
    /// <param name="aY">表示位置Y座標</param>
    public void SetBottomPosition( float aX, float aY )
    {
        _BottomPoint.X = aX;
        _BottomPoint.Y = aY;
    }

    /// <summary>
    /// 音量入力開始位置設定
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）</param>
    /// <param name="aVolume">音量</param>
    /// <param name="aType">音量入力タイプ</param>
    public void SetStartPos( int aNotePosX, int aVolume, ConfigEditer.VolumeEditType aType )
    {
        _StartNotePosX  = aNotePosX;
        _EndNotePosX    = aNotePosX;
        StartVolume     = MidiNet.CheckMidiVolume( aVolume );
        EditType        = aType;

        _VolumeList.Clear();
        _VolumeList.Add( aVolume );

        _Display    = true;
        _Direct     = true;
    }

    /// <summary>
    /// 音量入力終了位置設定
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）</param>
    /// <param name="aVolume">音量</param>
    public void SetEndPos( int aNotePosX, int aVolume )
    {
        _EndNotePosX    = aNotePosX;
        _EndVolume      = MidiNet.CheckMidiVolume( aVolume );

        CalcRange();
    }

    /// <summary>
    /// 音量入力解除
    /// </summary>
    public void ClearPos() => _Display = false;

    /// <summary>
    /// VolumeList更新
    /// </summary>
    public void CalcRange()
    {
        var s  = _StartNotePosX;
        var e  = _EndNotePosX;
        var sa = TermNotePosX;

        var dir =  e - s > 0 ;

        if ( sa == 0 )
        {
            _VolumeList.Clear();
            _VolumeList.Add( StartVolume );
        }
        else if ( dir == _Direct && EditType == ConfigEditer.VolumeEditType.FreeHand )
        {
            var cnt = _VolumeList.Count;

            if ( cnt >= sa )
            {
                _VolumeList.RemoveRange( sa, cnt - sa );
                _VolumeList.Add( _EndVolume );
            }
            else
            {
                var volume =  cnt == 0 ? StartVolume : _VolumeList[ cnt - 1 ] ;

                for ( var i = cnt; i <= sa; i++ )
                {
                    _VolumeList.Add( volume + ( ( _EndVolume - volume ) * ( i - cnt ) / ( sa - cnt ) ) );
                }
            }
        }
        else if ( EditType  is ConfigEditer.VolumeEditType.UpDown
                            or ConfigEditer.VolumeEditType.IntonationHL
                            or ConfigEditer.VolumeEditType.IntonationH
                            or ConfigEditer.VolumeEditType.IntonationL )
        {
            _VolumeList.Clear();

            for ( var i = 0; i <= sa; i++ )
            {
                _VolumeList.Add( StartVolume );
            }
        }
        else if ( EditType  is ConfigEditer.VolumeEditType.FreeHand
                            or ConfigEditer.VolumeEditType.StraightLine )
        {
            _VolumeList.Clear();

            for ( var i = 0; i <= sa; i++ )
            {
                _VolumeList.Add( StartVolume + ( ( _EndVolume - StartVolume ) * i / sa ) );
            }
        }
        else
        {
            // ベジェ曲線
            float c1 = StartVolume;
            float c2 = _EndVolume;

            switch ( EditType )
            {
                case ConfigEditer.VolumeEditType.Curve1:
                    c1 = StartVolume;
                    c2 = _EndVolume;
                    break;
                case ConfigEditer.VolumeEditType.Curve2:
                    c1 = StartVolume;
                    c2 = StartVolume;
                    break;
                case ConfigEditer.VolumeEditType.Curve3:
                    c1 = _EndVolume;
                    c2 = _EndVolume;
                    break;
                case ConfigEditer.VolumeEditType.Curve4:
                    c1 = StartVolume + ( ( _EndVolume - StartVolume ) / 3.0F );
                    c2 = _EndVolume + ( ( _EndVolume - StartVolume ) / 3.0F * 2.0F );
                    break;
            }

            _VolumeList.Clear();

            for ( var i = 0; i <= sa; i++ )
            {
                var vol = HelperMath.GetBezierCurvePosition
                    (
                        StartVolume,
                        c1,
                        c2,
                        _EndVolume,
                        (float)i / sa
                    );

                _VolumeList.Add( (int)vol );
            }
        }

        _Direct = dir;
    }

    /// <summary>
    /// 音量入力線描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aStartNotePosX">ノート位置（絶対値）</param>
    public void DrawRange( CanvasDrawingSession aGraphics, int aStartNotePosX )
    {
        if ( !_Display )
        {
            return;
        }

        var p1 = _BottomPoint;
        var p2 = _BottomPoint;

        p1.X += ( _StartNotePosX - aStartNotePosX ) * Config.Editer.NoteWidthSize;
        p2.X = p1.X;

        var cnt = _VolumeList.Count - 1;

        if ( cnt <= 0 )
        {
            return;
        }

        var format = Config.Editer.VolumeInputLine;

        var term = Config.Editer.NoteWidthSize * ( _Direct ? 1 : -1 );

        for ( var i = 0; i < cnt; i++ )
        {
            p1.X = p2.X;
            p2.X = p1.X + term;

            p1.Y = _BottomPoint.Y - _VolumeList [ i ];
            p2.Y = _BottomPoint.Y - _VolumeList [ i + 1 ];

            HelperWin2D.DrawFormatLine( aGraphics, p1, p2, format );

            switch ( EditType )
            {
                case ConfigEditer.VolumeEditType.UpDown:
                case ConfigEditer.VolumeEditType.IntonationHL:
                case ConfigEditer.VolumeEditType.IntonationH:
                case ConfigEditer.VolumeEditType.IntonationL:
                    {
                        // 開始／終了間の差分表示の場合
                        p1.Y -= _EndVolume - StartVolume;
                        p2.Y -= _EndVolume - StartVolume;

                        HelperWin2D.DrawFormatLine( aGraphics, p1, p2, format );
                    }
                    break;
            }
        }
    }
}
