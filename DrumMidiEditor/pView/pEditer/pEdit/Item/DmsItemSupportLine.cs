using System;
using System.Drawing;

using DrumMidiEditor.pConfig;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pEditer.pEdit;

/// <summary>
/// エディター描画アイテム：サポート線
/// </summary>
public class DmsItemSupportLine : DisposeBaseClass
{
    /// <summary>
    /// サポート線表示フラグ
    /// </summary>
    private bool _Display = false;

    /// <summary>
    /// サポート線開始位置
    /// </summary>
    private int _NotePosXStart = 0;

    /// <summary>
    /// サポート線終了位置
    /// </summary>
    private int _NotePosXEnd = 0;

    /// <summary>
    /// サポート線範囲
    /// </summary>
    private Rectangle _NoteRect = new();

    /// <summary>
    /// サポート線範囲 左端
    /// </summary>
    public int NotePosX => Math.Min( _NotePosXStart, _NotePosXEnd );

    /// <summary>
    /// サポート線の間隔
    /// </summary>
    public int IntervalNotePosX => Math.Abs( _NotePosXEnd - _NotePosXStart );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aX">シート左上小節番号X座標</param>
    /// <param name="aY">シート左上小節番号Y座標</param>
    /// <param name="aWidth">１ノート横幅</param>
    /// <param name="aHeight">サポート線の高さ</param>
    public void SetNoteRect( int aX, int aY, int aWidth, int aHeight )
    {
        _NoteRect.X		    = aX;
        _NoteRect.Y		    = aY;
        _NoteRect.Width	    = aWidth;
        _NoteRect.Height	= aHeight;
    }

    /// <summary>
    /// サポート線開始位置
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）</param>
    public void SetStartPos( int aNotePosX )
    {
        _NotePosXStart     = aNotePosX;
        _NotePosXEnd       = aNotePosX;

        _Display = true;
    }

    /// <summary>
    /// サポート線終了位置
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）</param>
    public void SetEndPos( int aNotePosX )
    {
        _NotePosXEnd = aNotePosX;
    }

    /// <summary>
    /// サポート線表示OFF
    /// </summary>
    public void ClearPos() => _Display = false;

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aMeasureNoStart">開始小節番号</param>
    /// <param name="aMeasureNoEnd">終了小節番号</param>
    /// <param name="aNotePosX">ノート位置（絶対値）</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( Graphics aGraphics, int aMeasureNoStart, int aMeasureNoEnd, int aNotePosX, int aDiffY )
    {
        if ( !_Display )
        {
            return;
        }

        var interval_x = IntervalNotePosX;

        if ( interval_x == 0 )
        {
            int x = _NoteRect.X + ( _NotePosXStart - aNotePosX ) * _NoteRect.Width;

            aGraphics.DrawLine
                ( 
                    Config.Editer.SheetSupportPen, 
                    x, 
                    _NoteRect.Top, 
                    x, 
                    _NoteRect.Bottom + aDiffY
                );
        }
        else
        {
            var s_note = Config.System.MeasureNoteNumber * aMeasureNoStart;
            var e_note = Config.System.MeasureNoteNumber * ( aMeasureNoEnd + 1 );

            s_note += ( NotePosX - s_note ) % interval_x;

            int x;

            for ( int i = s_note; i <= e_note; i += interval_x )
            {
                x = _NoteRect.X + ( i - aNotePosX ) * _NoteRect.Width;

                aGraphics.DrawLine
                    ( 
                        Config.Editer.SheetSupportPen, 
                        x, 
                        _NoteRect.Top, 
                        x, 
                        _NoteRect.Bottom + aDiffY
                    );
            }
        }
    }
} 
