using Microsoft.Graphics.Canvas;
using System;
using Windows.Foundation;

using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pPlayer.pPlay.pSurface;

/// <summary>
/// プレイヤー描画アイテム：ラベル
/// </summary>
internal class DmsItemLabel : DisposeBaseClass
{
    /// <summary>
    /// 描画範囲
    /// </summary>
    private Rect _DrawRect = new();

    /// <summary>
    /// 描画テキスト
    /// </summary>
    public string Text { get; set; } = String.Empty;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
    /// <param name="aText">描画テキスト</param>
	/// <param name="aFormatRect">描画書式</param>
    public DmsItemLabel( float aX, float aY, float aWidth, float aHeight, string aText, FormatRect aFormatRect )
    {
        _DrawRect.X         = aX;
        _DrawRect.Y         = aY;
        _DrawRect.Width     = aWidth;
        _DrawRect.Height    = aHeight;
        Text		        = aText;
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
    public void Draw( CanvasDrawingSession aGraphics )
    {
        if ( _FormatRect == null )
        {
            return;
        }

        XamlHelper.FormatRectDraw
            (
                aGraphics,
                _DrawRect,
                _FormatRect,
                Text ?? String.Empty
            );
    }
}
