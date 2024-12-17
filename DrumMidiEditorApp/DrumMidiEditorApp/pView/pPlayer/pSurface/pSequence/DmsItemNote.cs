using System;
using DrumMidiEditorApp.pUtil;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface.pSequence;

/// <summary>
/// プレイヤー描画アイテム：ノート
/// </summary>
internal partial class DmsItemNote : DisposeBaseClass, IComparable, IComparable<DmsItemNote>
{
    /// <summary>
    /// １小節内のノート描画位置X座標
    /// </summary>
    private readonly float _NotePosX = 0;

    /// <summary>
    /// ノート描画範囲
    /// </summary>
    private Size _NoteSize = new();

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = null;

    /// <summary>
    /// MidiMapヘッダアイテム
    /// </summary>
    private DmsItemMidiMap? _DmsItemMidiMap = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aNotePosX">１小節内のノート描画位置X座標</param>
    /// <param name="aWidth">横幅</param>
    /// <param name="aHeight">高さ</param>
    /// <param name="aFormatRect">描画書式</param>
    /// <param name="aDmsItemMidiMap">MidiMap描画アイテム</param>
    public DmsItemNote( float aNotePosX, float aWidth, float aHeight, FormatRect aFormatRect, DmsItemMidiMap aDmsItemMidiMap )
    {
        _NotePosX           = aNotePosX;
        _NoteSize.Width     = aWidth;
        _NoteSize.Height    = aHeight;
        _FormatRect         = aFormatRect;
        _DmsItemMidiMap     = aDmsItemMidiMap;
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _FormatRect     = null;
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
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        if ( _DmsItemMidiMap == null || _FormatRect == null )
        {
            return;
        }

        var rect = new Rect
            (
                _DmsItemMidiMap.DrawRect.Right,
                _DmsItemMidiMap.DrawRect.Top,
                _NoteSize._width,
                _NoteSize._height
            );

        rect.X += aDiffX + _NotePosX;
        rect.Y += ( ( _DmsItemMidiMap.DrawRect.Height - rect.Height ) / 2.0F ) + aDiffY;

        // 背景色
        aGraphics.FillRoundedRectangle
            (
                rect,
                _FormatRect.RadiusX,
                _FormatRect.RadiusY,
                _FormatRect.Background.Color
            );
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
