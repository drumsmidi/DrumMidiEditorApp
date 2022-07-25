using Microsoft.Graphics.Canvas;
using Windows.Foundation;

using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：小節線
/// </summary>
public class DmsItemLine : DisposeBaseClass
{
    /// <summary>
    /// 描画範囲
    /// </summary>
    private Rect _DrawRect = new();

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatLine? _FormatLine = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置＋１小節内での相対X座標</param>
	/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatLine">描画書式</param>
    public DmsItemLine( float aX, float aY, float aWidth, float aHeight, FormatLine aFormatLine )
    {
        _DrawRect.X         = aX;
        _DrawRect.Y         = aY;
        _DrawRect.Width     = aWidth;
        _DrawRect.Height    = aHeight;
        _FormatLine         = aFormatLine;
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
                // Dispose managed resources.
                _FormatLine = null;
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
        if ( _FormatLine == null || _FormatLine.LineSize <= 0 )
        {
            return;
        }

        var rect = _DrawRect;
        rect.X  += aDiffX;
        rect.Y  += aDiffY;

        aGraphics.DrawLine
            (
                (float)rect.Left,
                (float)rect.Top,
                (float)rect.Right,
                (float)rect.Bottom,
                _FormatLine.LineColor,
                _FormatLine.LineSize
            );
    }
}
