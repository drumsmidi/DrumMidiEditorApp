using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface.pSimuration;

/// <summary>
/// プレイヤー描画アイテム：MidiMapヘッダ
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aGroup">MidiMapGroup</param>
/// <param name="aX">描画位置X座標</param>
/// <param name="aY">描画位置Y座標</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
/// <param name="aFormatRect">描画書式</param>
internal partial class ItemMidiMap( MidiMapGroup aGroup, float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect ) 
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
            _MidiMapGroup   = null;
            _FormatRect     = null;
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
    /// 描画対象のMidiMapGroup
    /// </summary>
	private MidiMapGroup? _MidiMapGroup = aGroup;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = aFormatRect;

    #endregion

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

        HelperWin2D.DrawFormatRect( aGraphics, DrawRect, _FormatRect, _MidiMapGroup?.GroupName ?? string.Empty );
    }
}
