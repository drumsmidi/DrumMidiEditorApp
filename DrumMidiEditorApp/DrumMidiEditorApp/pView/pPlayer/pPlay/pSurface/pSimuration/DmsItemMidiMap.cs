using Microsoft.Graphics.Canvas;
using System;
using Windows.Foundation;

using DrumMidiEditorApp.pDMS;
using DrumMidiEditorApp.pGeneralFunction.pUtil;
using DrumMidiEditorApp.pGeneralFunction.pWinUI;

namespace DrumMidiEditorApp.pView.pPlayer.pPlay.pSurface.pSimuration;

/// <summary>
/// プレイヤー描画アイテム：MidiMapヘッダ
/// </summary>
internal class DmsItemMidiMap : DisposeBaseClass
{
    /// <summary>
    /// 描画対象のMidiMapGroup
    /// </summary>
	private MidiMapGroup? _MidiMapGroup = null;

    /// <summary>
    /// 描画範囲
    /// </summary>
    public Rect DrawRect { get; private set; } = new();

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = null;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aGroup">MidiMapGroup</param>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
    public DmsItemMidiMap( MidiMapGroup aGroup, float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect )
    {
        _MidiMapGroup   = aGroup;
        DrawRect        = new( aX, aY, aWidth, aHeight );
        _FormatRect     = aFormatRect;
    }

    protected override void Dispose( bool aDisposing )
	{
		if ( !_Disposed )
		{
			if ( aDisposing )
			{
                // Dispose managed resources.
                _MidiMapGroup   = null;
                _FormatRect     = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

			// Note disposing has been done.
			base.Dispose( aDisposing );
		}
	}
    private bool _Disposed = false;

    /// <summary>
    /// 位置設定
    /// </summary>
	/// <param name="aMoveX">描画位置X座標</param>
	/// <param name="aMoveY">描画位置Y座標</param>
    public void SetMovePosition( float aMoveX, float aMoveY )
    {
        DrawRect = new 
            (
                DrawRect.X + aMoveX,
                DrawRect.Y + aMoveY,
                DrawRect.Width,
                DrawRect.Height 
            );
    }

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
                DrawRect,
                _FormatRect,
                _MidiMapGroup?.GroupName ?? String.Empty
            );
    }
}
