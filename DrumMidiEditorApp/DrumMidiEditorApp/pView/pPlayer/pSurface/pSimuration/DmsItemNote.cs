using System;
using DrumMidiClassLibrary.pUtil;
using DrumMidiClassLibrary.pWinUI;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface.pSimuration;

/// <summary>
/// プレイヤー描画アイテム：ノート
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aNotePosX">１小節内のノート描画位置X座標</param>
/// <param name="aFormatRect">描画書式</param>
/// <param name="aDmsItemMidiMap">ヘッダアイテム</param>
internal class DmsItemNote( float aNotePosX, FormatRect aFormatRect, DmsItemMidiMap aDmsItemMidiMap ) : DisposeBaseClass, IComparable, IComparable<DmsItemNote>
{
    /// <summary>
    /// １小節内のノート描画位置X座標
    /// </summary>
    private readonly float _NotePosX = aNotePosX;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = aFormatRect;

    /// <summary>
    /// MidiMapヘッダアイテム
    /// </summary>
    private DmsItemMidiMap? _DmsItemMidiMap = aDmsItemMidiMap;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _FormatRect = null;
                _DmsItemMidiMap = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX )
    {
        if ( _DmsItemMidiMap == null || _FormatRect == null )
        {
            return;
        }

        var distance = _NotePosX + aDiffX;

        if ( distance is not < 0F or not > 9F )
        {
            // 1:0, 10:1
            var sa = 1F + Math.Log10( distance + 1 );

            if ( sa <= 0 )
            {
                return;
            }

            var rect = _DmsItemMidiMap.DrawRect;
            rect.Width *= sa;
            rect.Height *= sa;
            rect.X -= ( rect.Width - _DmsItemMidiMap.DrawRect.Width ) / 2.0F;
            rect.Y -= ( rect.Height - _DmsItemMidiMap.DrawRect.Height ) / 2.0F;

            // 背景色
            aGraphics.DrawEllipse
                (
                    rect._x + ( rect._width / 2 ),
                    rect._y + ( rect._height / 2 ),
                    rect._width / 2,
                    rect._height / 2,
                    _FormatRect.Line.LineColor.Color,
                    _FormatRect.Line.LineSize
                );
        }
    }

    /// <summary>
    /// ノート描画順 並替用
    /// </summary>
    /// <param name="aOther"></param>
    /// <returns></returns>
    public int CompareTo( DmsItemNote? aOther )
    {
        if ( aOther == null )
        {
            return 1;
        }
        else if ( _NotePosX > aOther._NotePosX )
        {
            return 1;
        }
        else if ( _NotePosX == aOther._NotePosX )
        {
            return 0;
        }
        return -1;
    }

    /// <summary>
    /// ノート描画順 並替用
    /// </summary>
    /// <param name="aOther"></param>
    /// <returns></returns>
    public int CompareTo( object? aOther )
    {
        return aOther == null
            ? 1
            : GetType() != aOther.GetType()
            ? throw new ArgumentException( "Invalid aOther", nameof( aOther ) )
            : CompareTo( aOther as DmsItemNote );
    }
}
