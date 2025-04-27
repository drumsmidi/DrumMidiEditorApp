using System;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

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
internal partial class ItemNote( float aNotePosX, FormatRect aFormatRect, ItemMidiMap aDmsItemMidiMap )
    : ItemBase( 0, aNotePosX, 0, 0, 0 )
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
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX )
    {
        if ( _DmsItemMidiMap == null || _FormatRect == null )
        {
            return;
        }

        var distance = _NotePosX + aDiffX;

        //if ( distance is not < 0F or not > 9F )
        {
            // 1:0, 10:1
            var sa = 1F + Math.Log10( distance + 1 );

            if ( sa <= 0 )
            {
                return;
            }

            var rect = _DmsItemMidiMap.DrawRect;
            rect.Width  *= sa;
            rect.Height *= sa;
            rect.X      -= ( rect.Width  - _DmsItemMidiMap.DrawRect.Width  ) / 2.0F;
            rect.Y      -= ( rect.Height - _DmsItemMidiMap.DrawRect.Height ) / 2.0F;

            // 背景色
            aGraphics.DrawEllipse
                (
                    rect._x + ( rect._width  / 2F ),
                    rect._y + ( rect._height / 2F ),
                    rect._width  / 2F,
                    rect._height / 2F,
                    _FormatRect.Line.LineColor.Color,
                    _FormatRect.Line.LineSize
                );
        }
    }
}
