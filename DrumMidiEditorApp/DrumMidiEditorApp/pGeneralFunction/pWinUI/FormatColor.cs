using System.Text.Json.Serialization;
using Windows.UI;

namespace DrumMidiEditorApp.pGeneralFunction.pWinUI;

/// <summary>
/// グラフィックアイテム：色
/// </summary>
public class FormatColor
{
    /// <summary>
    /// ライン色
    /// </summary>
    [JsonInclude]
    public Color Color = ColorHelper.EmptyColor;
}
