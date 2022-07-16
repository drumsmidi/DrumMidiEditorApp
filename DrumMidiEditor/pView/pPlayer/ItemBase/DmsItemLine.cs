using System.Drawing;

using DrumMidiEditor.pGeneralFunction.pDirectX;
using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pPlayer;

/// <summary>
/// プレイヤー描画アイテム：小節線
/// </summary>
internal class DmsItemLine : DisposeBaseClass
{
    /// <summary>
    /// ライン描画範囲
    /// </summary>
    private RectangleF _Rect = new();

    /// <summary>
    /// ラインカラー
    /// </summary>
    private readonly Color _Color = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aGraphics">グラフィック</param>
	/// <param name="aX">描画位置＋１小節内での相対X座標</param>
	/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aLineColor">ラインカラー</param>
    public DmsItemLine( Graphic2D aGraphics, float aX, float aY, float aWidth, float aHeight, Color aLineColor )
    {
        _Rect.X        = aX;
        _Rect.Y        = aY;
        _Rect.Width    = aWidth;
        _Rect.Height   = aHeight;
        _Color         = aLineColor;

		aGraphics.AddBrush( _Color );
    }

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( Graphic2D aGraphics, float aDiffX, float aDiffY )
    {
        var r = new RectangleF( _Rect.Location, _Rect.Size );
        r.X += aDiffX;
        r.Y += aDiffY;

        aGraphics.FillRectangle( r, _Color );
    }
}
