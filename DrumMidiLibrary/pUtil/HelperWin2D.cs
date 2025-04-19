using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// Win2Dユーティリティ
/// </summary>
public static class HelperWin2D
{
    #region FormatRect

    /// <summary>
    /// 四角形描画（背景塗りつぶし＋外枠描画＋テキスト描画）
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatRect"></param>
    /// <param name="aLabelText"></param>
    public static void DrawFormatRect( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatRect aFormatRect, string aLabelText )
    {
        // 背景色
        DrawFormatRectFillRectangle( aGraphics, aDrawRect, aFormatRect );

        // テキスト
        DrawFormatText( aGraphics, aDrawRect, aFormatRect.Text, aLabelText );

        // 外枠
        DrawFormatRectOutlineRectangle( aGraphics, aDrawRect, aFormatRect );
    }

    /// <summary>
    /// 四角形描画（背景＋外枠）
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatRect"></param>
    public static void DrawFormatRect( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatRect aFormatRect )
    {
        // 背景色
        DrawFormatRectFillRectangle( aGraphics, aDrawRect, aFormatRect );

        // 外枠
        DrawFormatRectOutlineRectangle( aGraphics, aDrawRect, aFormatRect );
    }

    /// <summary>
    /// 四角形描画（テキスト）
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatRect"></param>
    /// <param name="aLabelText"></param>
    public static void DrawFormatRectText( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatRect aFormatRect, string aLabelText )
    {
        DrawFormatText( aGraphics, aDrawRect, aFormatRect.Text, aLabelText );
    }

    /// <summary>
    /// 四角形描画（背景）
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatRect"></param>
    public static void DrawFormatRectFillRectangle( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatRect aFormatRect )
    {
        if ( !IsValidFormatRect( aFormatRect ) )
        {
            return;
        }

        aGraphics.FillRectangle( aDrawRect, aFormatRect.Background.Color );
    }

    /// <summary>
    /// 四角形描画（外枠）
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatRect"></param>
    public static void DrawFormatRectOutlineRectangle( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatRect aFormatRect )
    {
        if ( !IsValidFormatRectOutline( aFormatRect ) )
        {
            return;
        }

        aGraphics.DrawRectangle( aDrawRect, aFormatRect.Line.LineColor.Color, aFormatRect.Line.LineSize );
    }

    /// <summary>
    /// 円描画（背景）
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatRect"></param>
    public static void DrawFormatRectFillEllipse( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatRect aFormatRect )
    {
        if ( !IsValidFormatRect( aFormatRect ) )
        {
            return;
        }

        aGraphics.FillEllipse
            (
                (float)( aDrawRect.X + aDrawRect.Width  / 2 ),
                (float)( aDrawRect.Y + aDrawRect.Height / 2 ),
                aDrawRect._width  / 2,
                aDrawRect._height / 2,
                aFormatRect.Background.Color
            );
    }

    /// <summary>
    /// 円描画（外枠）
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatRect"></param>
    public static void DrawFormatRectOutlineEllipse( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatRect aFormatRect )
    {
        if ( !IsValidFormatRectOutline( aFormatRect ) )
        {
            return;
        }

        aGraphics.DrawEllipse
            (
                (float)( aDrawRect.X + aDrawRect.Width  / 2 ),
                (float)( aDrawRect.Y + aDrawRect.Height / 2 ),
                aDrawRect._width  / 2,
                aDrawRect._height / 2,
                aFormatRect.Line.LineColor.Color,
                aFormatRect.Line.LineSize
            );
    }

    /// <summary>
    /// ジオメトリ描画
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aCanvasGeometry"></param>
    /// <param name="aFormatRect"></param>
    public static void DrawFormatRectOutlineGeometry( CanvasDrawingSession aGraphics, CanvasGeometry aCanvasGeometry, FormatRect aFormatRect )
    {
        if ( !IsValidFormatRectOutline( aFormatRect ) )
        {
            return;
        }

        aGraphics.DrawGeometry( aCanvasGeometry, aFormatRect.Line.LineColor.Color, aFormatRect.Line.LineSize );
    }

    /// <summary>
    /// 共通入力チェック：FormatRect 塗潰
    /// </summary>
    /// <param name="aFormatRect"></param>
    /// <returns></returns>
    private static bool IsValidFormatRect( FormatRect aFormatRect )
        => aFormatRect != null;

    /// <summary>
    /// 共通入力チェック：FormatRect 外枠
    /// </summary>
    /// <param name="aFormatRect"></param>
    /// <returns></returns>
    private static bool IsValidFormatRectOutline( FormatRect aFormatRect )
        => aFormatRect != null && aFormatRect.Line.LineSize > 0;

    #endregion

    #region FormatText

    /// <summary>
    /// テキスト描画
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatText"></param>
    /// <param name="aLabelText"></param>
    public static void DrawFormatText( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatText aFormatText, string aLabelText )
    {
        if ( !IsValidFormatText( aFormatText, aLabelText ) )
        {
            return;
        }

        aGraphics.DrawText
            (
                aLabelText,
                aDrawRect,
                aFormatText.TextColor.Color,
                aFormatText.TextFormat
            );
    }

    /// <summary>
    /// 共通入力チェック：FormatText
    /// </summary>
    /// <param name="aFormatText"></param>
    /// <param name="aLabelText"></param>
    /// <returns></returns>
    private static bool IsValidFormatText( FormatText aFormatText, string aLabelText )
        => aFormatText != null && aFormatText.TextFormat.FontSize > 0 && aLabelText.Length != 0;

    #endregion

    #region FormatLine

    /// <summary>
    /// 直線描画
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aDrawRect"></param>
    /// <param name="aFormatLine"></param>
    public static void DrawFormatLine( CanvasDrawingSession aGraphics, Rect aDrawRect, FormatLine aFormatLine )
        => DrawFormatLine( aGraphics, (float)aDrawRect.Left, (float)aDrawRect.Top, (float)aDrawRect.Right, (float)aDrawRect.Bottom, aFormatLine );

    /// <summary>
    /// 直線描画
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aP1"></param>
    /// <param name="aP2"></param>
    /// <param name="aFormatLine"></param>
    public static void DrawFormatLine( CanvasDrawingSession aGraphics, Point aP1, Point aP2, FormatLine aFormatLine )
        => DrawFormatLine( aGraphics, aP1._x, aP1._y, aP2._x, aP2._y, aFormatLine );

    /// <summary>
    /// 直線描画
    /// </summary>
    /// <param name="aGraphics"></param>
    /// <param name="aX1"></param>
    /// <param name="aY1"></param>
    /// <param name="aX2"></param>
    /// <param name="aY2"></param>
    /// <param name="aFormatLine"></param>
    public static void DrawFormatLine( CanvasDrawingSession aGraphics, float aX1, float aY1, float aX2, float aY2, FormatLine aFormatLine )
    {
        if ( !IsValidFormatLine( aFormatLine ) )
        {
            return;
        }

        aGraphics.DrawLine
            (
                aX1,
                aY1,
                aX2,
                aY2,
                aFormatLine.LineColor.Color,
                aFormatLine.LineSize
            );
    }

    /// <summary>
    /// 共通入力チェック：FormatRect 外枠
    /// </summary>
    /// <param name="aFormatLine"></param>
    /// <returns></returns>
    private static bool IsValidFormatLine( FormatLine aFormatLine )
        => aFormatLine != null && aFormatLine.LineSize > 0;

    #endregion
}
