using System.Text.Json.Serialization;
using Windows.UI;

namespace DrumMidiClassLibrary.pWinUI;

/// <summary>
/// グラフィックアイテム：線
/// </summary>
public class FormatLine
{
    /// <summary>
    /// ライン色
    /// </summary>
    [JsonInclude]
    public FormatColor LineColor = new();

    /// <summary>
    /// ラインサイズ
    /// </summary>
    [JsonInclude]
    public float LineSize = 1;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public FormatLine() { }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aLineColor"></param>
    /// <param name="aLineSize"></param>
    public FormatLine( Color aLineColor, float aLineSize )
    {
        LineColor   = new( aLineColor );
        LineSize    = aLineSize;
    }
}
