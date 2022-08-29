using System.Text.Json.Serialization;
using Windows.UI;

namespace DrumMidiClassLibrary.pWinUI;

/// <summary>
/// グラフィックアイテム：四角
/// </summary>
public class FormatRect
{
    /// <summary>
    /// 背景色
    /// </summary>
    [JsonInclude]
    public FormatColor Background = new();

    /// <summary>
    /// ライン書式
    /// </summary>
    [JsonInclude]
    public FormatLine Line = new();

    /// <summary>
    /// 角の丸み
    /// </summary>
    [JsonInclude]
    public float RadiusX = 5;

    /// <summary>
    /// 角の丸み
    /// </summary>
    [JsonInclude]
    public float RadiusY = 5;

    /// <summary>
    /// テキスト書式
    /// </summary>
    [JsonInclude]
    public FormatText Text = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public FormatRect() { }
}
