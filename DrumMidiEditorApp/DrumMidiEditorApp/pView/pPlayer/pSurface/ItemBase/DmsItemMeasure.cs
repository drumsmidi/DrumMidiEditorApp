using Microsoft.Graphics.Canvas;
using System;
using Windows.Foundation;

using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface;

/// <summary>
/// プレイヤー描画アイテム：小節ヘッダ
/// </summary>
internal class DmsItemMeasure : DisposeBaseClass
{
    /// <summary>
    /// 描画範囲
    /// </summary>
    private Rect _DrawRect = new();

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置</param>
	/// <param name="aY">描画位置</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
    public DmsItemMeasure( float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect )
    {
        _DrawRect.X         = aX;
        _DrawRect.Y         = aY;
        _DrawRect.Width     = aWidth;
        _DrawRect.Height    = aHeight;
		_FormatRect			= aFormatRect;
    }

	protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
                // Dispose managed resources.
                _FormatRect = null;
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
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics, int aMeasureNo, float aDiffX, float aDiffY )
    {
        if ( _FormatRect == null )
        {
            return;
        }

        var rect = _DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

		aGraphics.DrawText
			(
				String.Format( "{0:000}", aMeasureNo ),
				rect,
				_FormatRect.TextColor,
				_FormatRect.TextFormat
			);
    }
}
