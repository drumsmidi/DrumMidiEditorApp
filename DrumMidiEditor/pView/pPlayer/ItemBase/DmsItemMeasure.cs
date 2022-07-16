using System;
using System.Drawing;

using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer;

/// <summary>
/// プレイヤー描画アイテム：小節ヘッダ
/// </summary>
internal class DmsItemMeasure : DisposeBaseClass
{
    /// <summary>
    /// BPM描画範囲
    /// </summary>
    private RectangleF _Rect = new();

    /// <summary>
    /// TextFormat検索キー（作りが微妙なので必要に応じて作り変えてください）
    /// </summary>
    private readonly int _TextFormatKey = 1;

    /// <summary>
    /// テキストカラー
    /// </summary>
    private readonly Color _TextColor = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aX">描画位置</param>
	/// <param name="aY">描画位置</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aGraphicItem">描画アイテム</param>
    public DmsItemMeasure( Graphic2D aGraphics, float aX, float aY, float aWidth, float aHeight, GraphicItem aGraphicItem )
    {
        _Rect.X        = aX;
        _Rect.Y        = aY;
        _Rect.Width    = aWidth;
        _Rect.Height   = aHeight;
        _TextColor     = aGraphicItem.FontBrush.Color;

		aGraphics.AddBrush( _TextColor );
		aGraphics.AddTextFormat( _TextFormatKey, aGraphicItem.Font, aGraphicItem.FontFormat );
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( Graphic2D aGraphics, int aMeasureNo, float aDiffX, float aDiffY )
    {
        var r = new RectangleF( _Rect.Location, _Rect.Size );
        r.X += aDiffX;
        r.Y += aDiffY;

        aGraphics.DrawText
            ( 
                String.Format( "{0:000}", aMeasureNo ), 
                _TextFormatKey, 
                r, 
                _TextColor 
            );
    }
}
