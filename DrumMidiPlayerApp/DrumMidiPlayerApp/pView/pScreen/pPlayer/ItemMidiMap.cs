using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

namespace DrumMidiPlayerApp.pView.pScreen.pPlayer;

/// <summary>
/// プレイヤー描画アイテム：MidiMapヘッダ
/// </summary>
/// <param name="aGroup">MidiMapGroup</param>
/// <param name="aX">描画位置X座標</param>
/// <param name="aY">描画位置Y座標</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
/// <param name="aFormatRect">描画書式</param>
internal partial class ItemMidiMap( MidiMapGroup aGroup, float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect ) 
    : ItemBase( 0, aX, aY, aWidth, aHeight )
{
    /// <summary>
    /// 描画対象のMidiMapGroup
    /// </summary>
	private readonly MidiMapGroup _MidiMapGroup = aGroup;

    /// <summary>
    /// 描画書式
    /// </summary>
    private readonly FormatRect _FormatRect = aFormatRect;

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
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
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
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
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics )
    {
        HelperWin2D.DrawFormatRect
            (
                aGraphics,
                DrawRect,
                _FormatRect,
                _MidiMapGroup.GroupName
            );
    }
}
