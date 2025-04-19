using System.Text.Json.Serialization;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// グラフィックアイテム：四角
/// </summary>
public class FormatRect()
{
    /// <summary>
    /// 背景色
    /// </summary>
    [JsonInclude]
    public FormatColor Background { get; set; } = new();

    /// <summary>
    /// ライン書式
    /// </summary>
    [JsonInclude]
    public FormatLine Line { get; set; } = new();

    /// <summary>
    /// 角の丸み
    /// </summary>
    [JsonInclude]
    public float RadiusX { get; set; } = 5;

    /// <summary>
    /// 角の丸み
    /// </summary>
    [JsonInclude]
    public float RadiusY { get; set; } = 5;

    /// <summary>
    /// テキスト書式
    /// </summary>
    [JsonInclude]
    public FormatText Text { get; set; } = new();
}
