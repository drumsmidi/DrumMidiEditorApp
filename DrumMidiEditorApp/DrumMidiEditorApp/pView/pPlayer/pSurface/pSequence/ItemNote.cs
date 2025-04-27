using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface.pSequence;

/// <summary>
/// プレイヤー描画アイテム：ノート
/// </summary>
/// <param name="aNotePosX">１小節内のノート描画位置X座標</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
/// <param name="aFormatRect">描画書式</param>
/// <param name="aDmsItemMidiMap">MidiMap描画アイテム</param>
internal partial class ItemNote( float aNotePosX, float aWidth, float aHeight, FormatRect aFormatRect, ItemMidiMap aDmsItemMidiMap ) 
    : ItemBase( 0, aNotePosX, 0, aWidth, aHeight )
{
    protected override void Dispose( bool aDisposing )
    {
        if ( _Disposed )
        {
            return;
        }

        // マネージドリソースの解放
        if ( aDisposing )
        {
            _FormatRect     = null;
            _DmsItemMidiMap = null;
        }

        // アンマネージドリソースの解放
        {
        }

        _Disposed = true;
        base.Dispose( aDisposing );
    }
    private bool _Disposed = false;

    #region member

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
    private ItemMidiMap? _DmsItemMidiMap = aDmsItemMidiMap;

    #endregion

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

        var rect = DrawRect;
        rect.X = _DmsItemMidiMap.DrawRect.Right + aDiffX + _NotePosX;
        rect.Y = _DmsItemMidiMap.DrawRect.Top   + aDiffY + ( ( _DmsItemMidiMap.DrawRect.Height - rect.Height ) / 2.0F );

        // 背景色
        aGraphics.FillRoundedRectangle
            (
                rect,
                _FormatRect.RadiusX,
                _FormatRect.RadiusY,
                _FormatRect.Background.Color
            );
    }
}
