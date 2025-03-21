using DrumMidiEditorApp.pConfig;
using DrumMidiEditorApp.pUtil;
using DrumMidiLibrary.pModel;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：ドラム
/// </summary>
public partial class DmsItemMidiMap : DisposeBaseClass
{
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

    /// <summary>
    /// 描画範囲
    /// </summary>
    private Rect _DrawRect = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
    /// <param name="aGroup">MidiMapGroup</param>
    /// <param name="aMidiMapGroupFlag">true:MidiMapGroup, false:MidiMapGroup ScaleKey</param>
    public DmsItemMidiMap( float aX, float aY, float aWidth, float aHeight, MidiMapGroup aGroup, bool aMidiMapGroupFlag )
    {
        _MidiMapGroupFlag   = aMidiMapGroupFlag;
        _MidiMapGroup       = aGroup;

        SetValue( aX, aY, aWidth, aHeight );
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
    /// <param name="aMidiMap">MidiMap</param>
    public DmsItemMidiMap( float aX, float aY, float aWidth, float aHeight, MidiMap aMidiMap )
    {
        _MidiMap = aMidiMap;

        SetValue( aX, aY, aWidth, aHeight );
    }

    protected override void Dispose( bool aDisposing )
    {
        if ( !_Disposed )
        {
            if ( aDisposing )
            {
                // Dispose managed resources.
                _MidiMapGroup   = null;
                _MidiMap        = null;
            }

            // Dispose unmanaged resources.

            _Disposed = true;

            // Note disposing has been done.
            base.Dispose( aDisposing );
        }
    }
    private bool _Disposed = false;

    /// <summary>
    /// 描画範囲再設定
    /// </summary>
	/// <param name="aX">描画位置X座標</param>
	/// <param name="aY">描画位置Y座標</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
    private void SetValue( float aX, float aY, float aWidth, float aHeight )
    {
        _DrawRect.X         = aX;
        _DrawRect.Y         = aY;
        _DrawRect.Width     = aWidth;
        _DrawRect.Height    = aHeight;
    }

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
        var rect = _DrawRect;
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
