using DrumMidiLibrary.pUtil;
using DrumMidiPlayerApp.pConfig;
using Microsoft.Graphics.Canvas;

namespace DrumMidiPlayerApp.pView.pScreen.pPlayer;

/// <summary>
/// プレイヤー描画アイテム：小節ヘッダ
/// </summary>
/// <param name="aX">描画位置</param>
/// <param name="aY">描画位置</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
/// <param name="aFormatRect">描画書式</param>
/// <param name="aFormatSelectRect">描画書式</param>
internal partial class ItemMeasure( float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect, FormatRect aFormatSelectRect ) 
    : ItemBase( 0, aX, aY, aWidth, aHeight )
{
    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = aFormatRect;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatSelectRect = aFormatSelectRect;

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
        var rect = DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

        HelperWin2D.DrawFormatRect
            ( 
                aGraphics, 
                rect,
                aSelectFlag ? _FormatSelectRect : _FormatRect, 
                string.Format( " " + Config.System.MeasureNumberFormat, aMeasureNo ) 
            );
    }
}
