using System.Numerics;
using DrumMidiEditorApp.pConfig;
using DrumMidiLibrary.pModel.pScore;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;

namespace DrumMidiEditorApp.pView.pEditer;

/// <summary>
/// エディター描画アイテム：ノート
/// </summary>
/// <remarks>
/// コンストラクタ
/// </remarks>
/// <param name="aX">描画位置＋１小節内での相対X座標</param>
/// <param name="aY">描画位置＋１小節内での相対Y座標</param>
/// <param name="aWidth">横幅</param>
/// <param name="aHeight">高さ</param>
/// <param name="aInfo">NOTE情報</param>
/// <param name="aFormatRect">描画書式</param>
public partial class ItemNote( float aX, float aY, float aWidth, float aHeight, InfoNote aInfo, FormatRect aFormatRect ) 
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
            InfoNote    = null;
            _FormatRect = null;
            NoteOnItem  = null;
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
    /// NOTE情報
    /// </summary>
    public InfoNote? InfoNote { get; private set; } = aInfo;

    /// <summary>
    /// 描画書式
    /// </summary>
    private FormatRect? _FormatRect = aFormatRect;

    /// <summary>
    /// NoteOn-Off間の距離（ノートON右端⇔ノートON左端）
    /// </summary>
    public float NoteLength { get; set; } = 0;

    /// <summary>
    /// NoteOffに対応するNoteOnアイテム
    /// </summary>
    public ItemNote? NoteOnItem { get; set; } = null;

    #endregion

    /// <summary>
    /// 描画
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void Draw( CanvasControl sender, CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        if ( InfoNote == null || _FormatRect == null )
        {
            return;
        }

        // 描画範囲の左上の座標基準
        var rect = DrawRect;
        rect.X += aDiffX;
        rect.Y += aDiffY;

        if ( InfoNote.NoteOn )
        {
            // ノートON描画
            HelperWin2D.DrawFormatRectFillEllipse( aGraphics, rect, _FormatRect );

            // ノートON-OFF間の線を描画
            if ( NoteLength > 0 )
            {
                HelperWin2D.DrawFormatLine
                    (
                        aGraphics,
                        (float)rect.Right,
                        (float)( rect.Y + ( rect.Height / 2F ) ),
                        (float)( rect.Left + NoteLength ),
                        (float)( rect.Y + ( rect.Height / 2F ) ),
                        _FormatRect.Line
                    );
            }
        }

        if ( !InfoNote.NoteOn && InfoNote.NoteOff )
        {
            Vector2[] ps =
                [
                    new( rect._x                                , (float)( rect.Y + (rect.Height / 2) ) ),
                    new( (float)( rect.X + (rect.Width / 2) )   , rect._y                               ),
                    new( (float)  rect.Right                    , (float)( rect.Y + (rect.Height / 2) ) ),
                    new( (float)( rect.X + (rect.Width / 2) )   , (float) rect.Bottom                   ),
                ];

            // ノートOFF描画
            HelperWin2D.DrawFormatRectOutlineGeometry
                    (
                        aGraphics,  
                        CanvasGeometry.CreatePolygon( sender, ps ),
                        _FormatRect
                    );
        }

        // ノートON-OFF間の線を描画
        if ( NoteOnItem != null )
        {
            HelperWin2D.DrawFormatLine
                (
                    aGraphics,
                    (float)( rect.X - NoteOnItem.NoteLength ),
                    (float)( rect.Y + ( rect.Height / 2 ) ),
                    (float)  rect.X,
                    (float)( rect.Y + ( rect.Height / 2 ) ),
                    _FormatRect.Line
                );
        }

        if ( InfoNote.Selected )
        {
            // ノート選択時の外枠を描画
            HelperWin2D.DrawFormatRectOutlineRectangle( aGraphics, rect, Config.Editer.NoteSelectRect );
        }
    }

    /// <summary>
    /// 描画（機械学習 予測結果用）
    /// </summary>
    /// <param name="aGraphics">グラフィック</param>
    /// <param name="aDiffX">描画差分X</param>
    /// <param name="aDiffY">描画差分Y</param>
    public void DrawPredict( CanvasDrawingSession aGraphics, float aDiffX, float aDiffY )
    {
        if ( InfoNote == null )
        {
            return;
        }

        var rect = new Rect
            (
                DrawRect.X + aDiffX - 2,
                DrawRect.Y + aDiffY - 2,
                DrawRect.Width  + 4,
                DrawRect.Height + 4
            );

        HelperWin2D.DrawFormatRectOutlineEllipse( aGraphics, rect, Config.Editer.NotePredictRect );
    }
}
