using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface;

/// <summary>
/// プレイヤー描画アイテム：MidiMapヘッダ
/// </summary>
internal partial class ItemMidiMap : ItemBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aGroup">MidiMapGroup</param>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
    public ItemMidiMap( MidiMapGroup aGroup, float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect )
        : base( 0, aX, aY, aWidth, aHeight )
    {
        _MidiMapGroup   = aGroup;
        _FormatRect     = aFormatRect;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aMidiMap">MidiMap</param>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
    public ItemMidiMap( MidiMap aMidiMap, float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect )
        : base( 0, aX, aY, aWidth, aHeight )
    {
        _MidiMap        = aMidiMap;
        _FormatRect     = aFormatRect;
    }

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
            _MidiMap        = null;
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
	private MidiMapGroup? _MidiMapGroup = null;

    /// <summary>
    /// 描画対象のMidiMap
    /// </summary>
	private MidiMap? _MidiMap = null;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = null;

    #endregion

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics )
    {
        if ( _FormatRect == null )
        {
            return;
        }

        HelperWin2D.DrawFormatRect
            (
                aGraphics,
                DrawRect,
                _FormatRect,
                _MidiMapGroup?.GroupName ?? _MidiMap?.MidiMapName ?? string.Empty
            );
    }
}
