using System.Text.Json.Serialization;
using Windows.UI;

namespace DrumMidiEditorApp.pGeneralFunction.pWinUI;

/// <summary>
/// グラフィックアイテム：直線
/// </summary>
public class FormatLine
{
    /// <summary>
    /// ライン色
    /// </summary>
    [JsonInclude]
    public Color LineColor = ColorHelper.EmptyColor;

    /// <summary>
    /// ラインサイズ
    /// </summary>
    [JsonInclude]
    public float LineSize = 1;
}
