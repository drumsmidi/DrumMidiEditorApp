using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pScreen.pPlayer;

/// <summary>
/// プレイヤー描画アイテム：ノート
/// </summary>
/// <param name="aX">描画位置＋１小節内での相対X座標</param>
/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
/// <param name="aFormatRect">描画書式</param>
/// <param name="aLabelText">ラベル</param>
internal partial class ItemNote( float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect, string aLabelText ) 
    : ItemBase( 0, aX, aY, aWidth, aHeight )
{
    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = aFormatRect;

    /// <summary>
    /// ラベルテキスト
    /// </summary>
    private readonly string _LabelText = aLabelText;

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
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY, bool aTextFlag )
    {
        if ( _FormatRect == null )
        {
            return;
        }

        var rect = DrawRect;
        rect.X  += aDiffX;
        rect.Y  += aDiffY;

        // テキスト
        if ( aTextFlag && _LabelText.Length != 0 )
        {
            aGraphics.DrawText
                (
                    _LabelText,
                    rect._x - rect._width / 2F,
                    rect._y - rect._height,
                    rect._width,
                    rect._height,
                    _FormatRect.Background.Color,
                    _FormatRect.Text.TextFormat
                );
        }
        else
        {
            // 背景色
            aGraphics.FillEllipse
                (
                    rect._x,
                    rect._y - rect._height / 2.0f,
                    rect._width,
                    rect._height,
                    _FormatRect.Background.Color
                );
        }
    }
}
