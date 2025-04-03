﻿using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

namespace DrumMidiPlayerApp.pView.pScreen.pPlayer;

/// <summary>
/// プレイヤー描画アイテム：ラベル
/// </summary>
/// <param name="aX">描画位置X座標</param>
/// <param name="aY">描画位置Y座標</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
/// <param name="aText">描画テキスト</param>
/// <param name="aFormatRect">描画書式</param>
internal partial class ItemLabel( float aX, float aY, float aWidth, float aHeight, string aText, FormatRect aFormatRect ) 
    : ItemBase( 0, aX, aY, aWidth, aHeight )
{
    /// <summary>
    /// 描画テキスト
    /// </summary>
    public string Text { get; set; } = aText;

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
        => Draw( aGraphics, 0, 0 );

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        var rect = DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

        HelperWin2D.DrawFormatRect( aGraphics, rect, _FormatRect, Text );
    }
}
