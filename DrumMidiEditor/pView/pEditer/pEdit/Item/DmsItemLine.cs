using System.Drawing;

using DrumMidiEditor.pGeneralFunction.pUtil;

namespace DrumMidiEditor.pView.pEditer.pEdit;

/// <summary>
/// エディター描画アイテム：小節線
/// </summary>
public class DmsItemLine : DisposeBaseClass
{
    /// <summary>
    /// 描画範囲
    /// </summary>
    private Rectangle _Rect = new();

    /// <summary>
    /// ラインペン
    /// </summary>
    private Pen? _LinePen = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置＋１小節内での相対X座標</param>
	/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
    /// <param name="aLinePen">線用Pen</param>
    public DmsItemLine( int aX, int aY, int aWidth, int aHeight, Pen aLinePen )
    {
        _Rect.X        = aX;
        _Rect.Y        = aY;
        _Rect.Width    = aWidth;
        _Rect.Height   = aHeight;
        _LinePen       = aLinePen;
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
				// Dispose managed resources.
				_LinePen = null;
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
    public void Draw( Graphics aGraphics, int aDiffX, int aDiffY )
    {
        var r = new Rectangle( _Rect.Location, _Rect.Size );
        r.X  += aDiffX;
        r.Y  += aDiffY;

        if ( _LinePen != null )
        { 
            aGraphics.DrawLine( _LinePen, r.Left, r.Top, r.Right, r.Bottom );
        }
    }
}
