using Microsoft.Graphics.Canvas.Text;
using System.Text.Json.Serialization;
using Windows.UI;

namespace DrumMidiEditorApp.pGeneralFunction.pWinUI;

/// <summary>
/// グラフィックアイテム：四角
/// </summary>
public class FormatRect
{
    /// <summary>
    /// 背景色
    /// </summary>
    [JsonInclude]
    public Color BackColor = ColorHelper.EmptyColor;

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

    /// <summary>
    /// 角の丸み
    /// </summary>
    [JsonInclude]
    public float RadiusX = 1;

    /// <summary>
    /// 角の丸み
    /// </summary>
    [JsonInclude]
    public float RadiusY = 1;

    /// <summary>
    /// テキスト色
    /// </summary>
    [JsonInclude]
    public Color TextColor = ColorHelper.EmptyColor;

    /// <summary>
    /// テキスト書式
    /// </summary>
    [JsonIgnore]
    public CanvasTextFormat TextFormat { get; set; } = new()
    {
        FontFamily          = "system-ui",
        FontSize            = 8,
        HorizontalAlignment = CanvasHorizontalAlignment.Center,
        VerticalAlignment   = CanvasVerticalAlignment.Center,
    };
}
