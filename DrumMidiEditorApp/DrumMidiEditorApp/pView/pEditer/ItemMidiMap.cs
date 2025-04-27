using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：ドラム
/// </summary>
public partial class ItemMidiMap : ItemBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
    /// <param name="aGroup">MidiMapGroup</param>
    /// <param name="aMidiMapGroupFlag">true:MidiMapGroup, false:MidiMapGroup ScaleKey</param>
    public ItemMidiMap( float aX, float aY, float aWidth, float aHeight, MidiMapGroup aGroup, bool aMidiMapGroupFlag )
        : base( 0, aX, aY, aWidth, aHeight )
    {
        _MidiMapGroupFlag   = aMidiMapGroupFlag;
        _MidiMapGroup       = aGroup;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
    /// <param name="aMidiMap">MidiMap</param>
    public ItemMidiMap( float aX, float aY, float aWidth, float aHeight, MidiMap aMidiMap )
        : base( 0, aX, aY, aWidth, aHeight )
    {
        _MidiMap = aMidiMap;
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
    /// true:MidiMapGroup, false:MidiMapGroup ScaleKey
    /// </summary>
	private readonly bool _MidiMapGroupFlag = false;

    /// <summary>
    /// MidiMapGroup
    /// </summary>
	private MidiMapGroup? _MidiMapGroup = null;

    /// <summary>
    /// MidiMap
    /// </summary>
	private MidiMap? _MidiMap = null;

    #endregion

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        string text;
        bool   select;

        if ( _MidiMap != null )
        {
            text    = _MidiMap.MidiMapName;
            select  = _MidiMap.Selected;
        }
        else if ( _MidiMapGroup != null )
        {
            if ( _MidiMapGroupFlag )
            {
                text    = _MidiMapGroup.GroupName;
                select  = _MidiMapGroup.Selected;
            }
            else
            {
                text    = _MidiMapGroup.ScaleKey;
                select  = _MidiMapGroup.ScaleKeySelected;
            }
        }
        else
        {
            return;
        }

        // 描画範囲の左上の座標基準
        var rect = DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

        HelperWin2D.DrawFormatRect
            (
                aGraphics,
                rect,
                select ? Config.Editer.HeaderSelectRect : Config.Editer.HeaderNonSelectRect,
                text
            );
    }
}
