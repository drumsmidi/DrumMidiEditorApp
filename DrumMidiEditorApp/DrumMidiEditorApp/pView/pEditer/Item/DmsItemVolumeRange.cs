﻿using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using Windows.Foundation;

using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pGeneralFunction.pUtil;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：音量選択範囲
/// </summary>
public class DmsItemVolumeRange : DisposeBaseClass
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
    /// 音量入力 開始音量
    /// </summary>
    private int _StartVolume = 0;

    /// <summary>
    /// 音量入力 終了音量
    /// </summary>
    private int _EndVolume = 0;

    /// <summary>
    /// 音量入力リスト
    /// </summary>
    private readonly List<int> _VolumeList = new();

    /// <summary>
    /// シート左下表示位置
    /// </summary>
    private Point _BottomPoint = new();

    /// <summary>
    /// 音量入力タイプ
    /// </summary>
    public ConfigEditer.VolumeEditType EditType 
        { get; private set; } = ConfigEditer.VolumeEditType.FreeHand;

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
    public int StartVolume => _StartVolume;

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
                            foreach ( int v in _VolumeList )
                            {
                                lst.Add( _EndVolume - v );
                            }
                        }
                        else
                        {
                            for ( int i = _VolumeList.Count - 1; i >= 0; i-- )
                            {
                                lst.Add( _EndVolume - _VolumeList[ i ] );
                            }
                        }
                    }
                    break;
                default:
                    {
                        if ( _Direct )
                        {
                            foreach ( int v in _VolumeList )
                            {
                                lst.Add( v );
                            }
                        }
                        else
                        {
                            for ( int i = _VolumeList.Count - 1; i >= 0; i-- )
                            {
                                lst.Add( _VolumeList[ i ] );
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
        _BottomPoint.X	= aX;
        _BottomPoint.Y	= aY;
    }

    /// <summary>
    /// 音量入力開始位置設定
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）</param>
    /// <param name="aVolume">音量</param>
    /// <param name="aType">音量入力タイプ</param>
    public void SetStartPos( int aNotePosX, int aVolume, ConfigEditer.VolumeEditType aType )
    {
        _StartNotePosX = aNotePosX;
        _EndNotePosX   = aNotePosX;
        _StartVolume   = Config.Media.CheckMidiVolume( aVolume );
        EditType       = aType;

        _VolumeList.Clear();
        _VolumeList.Add( aVolume );

        _Display   = true;
        _Direct    = true;
    }

    /// <summary>
    /// 音量入力終了位置設定
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）</param>
    /// <param name="aVolume">音量</param>
    public void SetEndPos( int aNotePosX, int aVolume )
    {
        _EndNotePosX  = aNotePosX;
        _EndVolume    = Config.Media.CheckMidiVolume( aVolume );

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
        int s  = _StartNotePosX;
        int e  = _EndNotePosX;
        int sa = TermNotePosX;

        bool dir = ( e - s > 0 );

        if ( sa == 0 )
        {
            _VolumeList.Clear();
            _VolumeList.Add( _StartVolume );
        }
        else if ( dir == _Direct && EditType == ConfigEditer.VolumeEditType.FreeHand )
        {
            int cnt = _VolumeList.Count;

            if ( cnt >= sa )
            {
                _VolumeList.RemoveRange( sa,  cnt - sa );
                _VolumeList.Add( _EndVolume );
            }
            else
            {
                int volume = ( cnt == 0 ? StartVolume : _VolumeList[ cnt - 1 ] );

                for ( int i = cnt; i <= sa; i++ )
                {
                    _VolumeList.Add( (int)( volume + ( _EndVolume - volume ) * ( i - cnt ) / ( sa - cnt ) ) );
                }
            }
        }
        else if (   EditType == ConfigEditer.VolumeEditType.UpDown
                ||  EditType == ConfigEditer.VolumeEditType.IntonationHL
                ||  EditType == ConfigEditer.VolumeEditType.IntonationH
                ||  EditType == ConfigEditer.VolumeEditType.IntonationL )
        {
            _VolumeList.Clear();

            for ( int i = 0; i <= sa; i++ )
            {
                _VolumeList.Add( (int)_StartVolume );
            }
        }
        else if (   EditType == ConfigEditer.VolumeEditType.FreeHand
                ||  EditType == ConfigEditer.VolumeEditType.StraightLine )
        {
            _VolumeList.Clear();

            for ( int i = 0; i <= sa; i++ )
            {
                _VolumeList.Add( (int)( _StartVolume + ( _EndVolume - _StartVolume ) * i / sa ) );
            }
        }
        else
        {
            // ベジェ曲線
            float c1 = _StartVolume;
            float c2 = _EndVolume;

            switch ( EditType )
            {
                case ConfigEditer.VolumeEditType.Curve1:
                    c1 = _StartVolume;
                    c2 = _EndVolume;
                    break;
                case ConfigEditer.VolumeEditType.Curve2:
                    c1 = _StartVolume;
                    c2 = _StartVolume;
                    break;
                case ConfigEditer.VolumeEditType.Curve3:
                    c1 = _EndVolume;
                    c2 = _EndVolume;
                    break;
                case ConfigEditer.VolumeEditType.Curve4:
                    c1 = _StartVolume + ( _EndVolume - _StartVolume ) / 3.0F;
                    c2 = _EndVolume   + ( _EndVolume - _StartVolume ) / 3.0F * 2.0F;
                    break;
            }

            _VolumeList.Clear();

            for ( int i = 0; i <= sa; i++ )
            {
                var vol = MathUtil.GetBezierCurvePosition
                    ( 
                        _StartVolume,
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
        p2.X  = p1.X;

        int cnt = _VolumeList.Count - 1;

        if ( cnt <= 0 )
        {
            return;
        }

        var format = Config.Editer.VolumeInputLine;

        var term = Config.Editer.NoteWidthSize * ( _Direct ? 1 : -1 );

        for ( int i = 0; i < cnt; i++ )
        {
            p1.X  = p2.X;
            p2.X  = p1.X + term;

            p1.Y  = _BottomPoint.Y - (int)_VolumeList[ i ];
            p2.Y  = _BottomPoint.Y - (int)_VolumeList[ i + 1 ];

            aGraphics.DrawLine
                ( 
                    p1._x,
                    p1._y,
                    p2._x,
                    p2._y,
                    format.LineColor,
                    format.LineSize
                );

            switch ( EditType )
            {
                case ConfigEditer.VolumeEditType.UpDown:
                case ConfigEditer.VolumeEditType.IntonationHL:
                case ConfigEditer.VolumeEditType.IntonationH:
                case ConfigEditer.VolumeEditType.IntonationL:
                    {
                        // 開始／終了間の差分表示の場合
                        p1.Y -= _EndVolume - _StartVolume;
                        p2.Y -= _EndVolume - _StartVolume;

                        aGraphics.DrawLine
                            ( 
                                p1._x,
                                p1._y,
                                p2._x,
                                p2._y,
                                format.LineColor,
                                format.LineSize
                            );
                    }
                    break;
            }
        }
    }
} 
