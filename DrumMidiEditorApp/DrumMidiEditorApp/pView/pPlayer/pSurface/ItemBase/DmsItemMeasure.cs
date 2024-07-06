using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pUtil;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

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
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatSelectRect = null;


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
        _FormatRect         = aFormatRect;
        _FormatSelectRect   = aFormatRect;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置</param>
	/// <param name="aY">描画位置</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
	/// <param name="aFormatSelectRect">描画書式</param>
    public DmsItemMeasure( float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect, FormatRect aFormatSelectRect )
    {
        _DrawRect.X         = aX;
        _DrawRect.Y         = aY;
        _DrawRect.Width     = aWidth;
        _DrawRect.Height    = aHeight;
        _FormatRect         = aFormatRect;
        _FormatSelectRect   = aFormatSelectRect;
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _FormatRect         = null;
                _FormatSelectRect   = null;
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
        => Draw( aGraphics, aMeasureNo, aDiffX, aDiffY, false  );

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    /// <param name="aSelectFlag">選択中フラグ</param>
    public void Draw( CanvasDrawingSession aGraphics, int aMeasureNo, float aDiffX, float aDiffY, bool aSelectFlag )
    {
        var rect = _DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

        HelperXaml.DrawFormatRect
            ( 
                aGraphics, 
                rect,
                aSelectFlag ? _FormatSelectRect : _FormatRect, 
                string.Format( " " + Config.System.MeasureNumberFormat, aMeasureNo ) 
            );
    }
}
