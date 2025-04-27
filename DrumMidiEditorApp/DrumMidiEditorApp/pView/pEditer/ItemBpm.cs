using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：BPM
/// </summary>
/// <param name="aX">描画位置＋１小節内での相対X座標</param>
/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
/// <param name="aInfo">BPM情報</param>
public partial class ItemBpm( float aX, float aY, float aWidth, float aHeight, InfoBpm aInfo ) 
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
            _BpmInfo = null;
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
    /// BPM情報
    /// </summary>
    private InfoBpm? _BpmInfo = aInfo;

    #endregion

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        if ( _BpmInfo == null )
        {
            return;
        }

        var format = _BpmInfo.Selected ? Config.Editer.BpmSelectRect : Config.Editer.BpmNonSelectRect ;

        // 描画範囲の左上の座標基準
        var rect = DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

        // 背景色
        HelperWin2D.DrawFormatRectFillRectangle( aGraphics, rect, format );

        rect.X      += rect.Width + 1;
        rect.Width  += 100;

        // テキスト描画
        HelperWin2D.DrawFormatRectText( aGraphics, rect, format, $"{_BpmInfo.Bpm}" );
    }
}
