using System;
using DrumMidiClassLibrary.pConfig;
using DrumMidiClassLibrary.pUtil;
using DrumMidiEditorApp.pConfig;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pEditer;

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
    /// サポート線開始位置：ノート位置（絶対値）
    /// </summary>
    private int _NotePosXStart = 0;

    /// <summary>
    /// サポート線終了位置：ノート位置（絶対値）
    /// </summary>
    private int _NotePosXEnd = 0;

    /// <summary>
    /// サポート線列範囲（シート左上小節番号始点）
    /// </summary>
    private Rect _NoteColumnRect = new();

    /// <summary>
    /// サポート線範囲 左端：ノート位置（絶対値）
    /// </summary>
    public int NotePosX => Math.Min( _NotePosXStart, _NotePosXEnd );

    /// <summary>
    /// サポート線の間隔
    /// </summary>
    public int IntervalNotePosX => Math.Abs( _NotePosXEnd - _NotePosXStart );

    /// <summary>
    /// ノートサイズ設定
    /// </summary>
    /// <param name="aX">シート左上小節番号X座標</param>
    /// <param name="aY">シート左上小節番号Y座標</param>
    /// <param name="aWidth">１ノート横幅</param>
    /// <param name="aHeight">サポート線の高さ</param>
    public void SetNoteRect( float aX, float aY, float aWidth, float aHeight )
    {
        _NoteColumnRect.X       = aX;
        _NoteColumnRect.Y       = aY;
        _NoteColumnRect.Width   = aWidth;
        _NoteColumnRect.Height  = aHeight;
    }

    /// <summary>
    /// サポート線開始位置
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）</param>
    public void SetStartPos( int aNotePosX )
    {
        _NotePosXStart  = aNotePosX;
        _NotePosXEnd    = aNotePosX;

        _Display = true;
    }

    /// <summary>
    /// サポート線終了位置
    /// </summary>
    /// <param name="aNotePosX">ノート位置（絶対値）</param>
    public void SetEndPos( int aNotePosX ) => _NotePosXEnd = aNotePosX;

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
    public void Draw( CanvasDrawingSession aGraphics, int aMeasureNoStart, int aMeasureNoEnd, int aNotePosX, float aDiffY )
    {
        if ( !_Display )
        {
            return;
        }

        var format = ConfigLocal.Editer.SheetSupportLine;

        var interval_x = IntervalNotePosX;

        if ( interval_x == 0 )
        {
            var x = _NoteColumnRect._x + (( _NotePosXStart - aNotePosX ) * _NoteColumnRect._width);

            aGraphics.DrawLine
                (
                    x,
                    (float)_NoteColumnRect.Top,
                    x,
                    (float)_NoteColumnRect.Bottom + aDiffY,
                    format.LineColor.Color,
                    format.LineSize
                );
        }
        else
        {
            var s_note = Config.System.MeasureNoteNumber * aMeasureNoStart;
            var e_note = Config.System.MeasureNoteNumber * ( aMeasureNoEnd + 1 );

            s_note += ( NotePosX - s_note ) % interval_x;

            float x;

            for ( var i = s_note; i <= e_note; i += interval_x )
            {
                x = _NoteColumnRect._x + ( ( i - aNotePosX ) * _NoteColumnRect._width );

                aGraphics.DrawLine
                    (
                        x,
                        (float)_NoteColumnRect.Top,
                        x,
                        (float)_NoteColumnRect.Bottom + aDiffY,
                        format.LineColor.Color,
                        format.LineSize
                    );
            }
        }
    }
}
