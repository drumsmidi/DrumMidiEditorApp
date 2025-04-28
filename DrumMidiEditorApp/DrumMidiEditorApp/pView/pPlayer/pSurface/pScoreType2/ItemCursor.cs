using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface.pScoreType2;

/// <summary>
/// プレイヤー描画アイテム：小節線
/// </summary>
/// <param name="aX">描画位置＋１小節内での相対X座標</param>
/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
/// <param name="aFormatRect">描画書式</param>
/// <param name="aFormatLine">描画書式</param>
internal partial class ItemCursor( float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect, FormatLine aFormatLine ) 
    : ItemBase( 0, aX, aY, aWidth, aHeight )
{
    protected override void Dispose( bool aDisposing )
    {
        if ( _Disposed )
        {
            return;
        }

        // マネージドリソースの解放
        if ( aDisposing )
        {
            _FormatRect = null;
            _FormatLine = null;
        }

        // アンマネージドリソースの解放
        {
        }

        _Disposed = true;
        base.Dispose( aDisposing );
    }
    private bool _Disposed = false;

    #region member

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = aFormatRect;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatLine? _FormatLine = aFormatLine;

    #endregion

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    /// <param name="aCursolX">カーソル描画相対位置Y</param>
    /// <param name="aDrawLine">カーソル描画フラグ</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY, float aCursolX, bool aDrawLine )
    {
        if ( aCursolX <= 0 )
        {
            return;
        }

        var rect     = DrawRect;
        rect.X      += aDiffX;
        rect.Y      += aDiffY;
        rect.Width   = aCursolX;

        if ( Config.Player.ScoreType2.CursorRectOn && _FormatRect != null )
        {
            HelperWin2D.DrawFormatRect
                (
                    aGraphics,
                    rect,
                    _FormatRect,
                    string.Empty
                );
        }

        if ( aDrawLine && _FormatLine != null )
        {
            rect.X     += aCursolX;
            rect.Width  = DrawRect.Width;

            HelperWin2D.DrawFormatLine( aGraphics, rect, _FormatLine );
        }
    }
}
