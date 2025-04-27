using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

namespace DrumMidiEditorApp.pView.pPlayer.pSurface;

/// <summary>
/// プレイヤー描画アイテム：小節ヘッダ
/// </summary>
internal partial class ItemMeasure : ItemBase
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置</param>
	/// <param name="aY">描画位置</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
    public ItemMeasure( float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect )
        : base( 0, aX, aY, aWidth, aHeight )
    {
        _FormatRect         = aFormatRect;
        _FormatSelectRect   = aFormatRect;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
	/// <param name="aX">描画位置</param>
	/// <param name="aY">描画位置</param>
	/// <param name="aWidth">横幅</param>
	/// <param name="aHeight">高さ</param>
	/// <param name="aFormatRect">描画書式</param>
	/// <param name="aFormatSelectRect">描画書式</param>
    public ItemMeasure( float aX, float aY, float aWidth, float aHeight, FormatRect aFormatRect, FormatRect aFormatSelectRect )
        : base( 0, aX, aY, aWidth, aHeight )
    {
        _FormatRect         = aFormatRect;
        _FormatSelectRect   = aFormatSelectRect;
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
            _FormatRect         = null;
            _FormatSelectRect   = null;
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
    private FormatRect? _FormatRect = null;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatSelectRect = null;

    #endregion

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics, int aMeasureNo, float aDiffX, float aDiffY )
        => Draw( aGraphics, aMeasureNo, aDiffX, aDiffY, false  );

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aMeasureNo">小節番号</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    /// <param name="aSelectFlag">選択中フラグ</param>
    public void Draw( CanvasDrawingSession aGraphics, int aMeasureNo, float aDiffX, float aDiffY, bool aSelectFlag )
    {
        var formatRect = aSelectFlag ? _FormatSelectRect : _FormatRect ;

        if ( formatRect == null )
        {
            return;
        }

        var rect = DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

        HelperWin2D.DrawFormatRect
            ( 
                aGraphics, 
                rect,
                formatRect, 
                string.Format( " " + Config.System.MeasureNumberFormat, aMeasureNo ) 
            );
    }
}
