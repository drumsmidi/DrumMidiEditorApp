using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace DrumMidiPlayerApp.pView.pScreen;

/// <summary>
/// 描画アイテム：処理中
/// </summary>
/// <param name="aX">描画中心座標</param>
/// <param name="aY">描画中心座標</param>
/// <param name="aRadius">半径</param>
/// <param name="aFormatRect">描画書式</param>
internal partial class ItemProcessing( float aX, float aY, float aRadius, FormatRect aFormatRect ) 
    : ItemBase( 10, aX - aRadius, aY - aRadius, aRadius, aRadius )
{
    /// <summary>
    /// 描画位置：X座標（中心値）
    /// </summary>
    private readonly float _PositionX = aX;

    /// <summary>
    /// 描画位置：Y座標（中心値）
    /// </summary>
    private readonly float _PositionY = aY;

    /// <summary>
    /// 描画範囲：半径
    /// </summary>
    private readonly float _PositionRadius = aRadius;

    /// <summary>
    /// １アイテムの基準サイズ：半径
    /// </summary>
    private readonly float _DrawRadius = aRadius / 8.0F;

    /// <summary>
    /// 描画アイテム数
    /// </summary>
    private readonly int _ItemCount = 6;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = aFormatRect;

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

        var progress = FrameProgress;

        var rect = new Rect();

        for ( var i = _ItemCount; i > 0; i-- )
        {
            var ( x, y ) = HelperMath.GetCirclePosition
                ( 
                    _PositionX, 
                    _PositionY, 
                    _PositionRadius, 
                    360F * i * progress
                );

            var radius = _DrawRadius * i / _ItemCount;

            rect.X      = x - radius;
            rect.Y      = y - radius;
            rect.Width  = radius * 2F;
            rect.Height = rect.Width;

            HelperWin2D.DrawFormatRectFillEllipse( aGraphics, rect, _FormatRect );
        }
    }
}
