using System.Text.Json.Serialization;
using DrumMidiLibrary.pConfig;
using Microsoft.Graphics.Canvas.Text;
using Windows.UI;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// グラフィックアイテム：テキスト
/// </summary>
public class FormatText
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public FormatText()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aTextColor">テキストの色</param>
    /// <param name="aTextFormat">テキストの書式</param>
    public FormatText( Color aTextColor, CanvasTextFormat aTextFormat )
    {
        TextColor   = new( aTextColor );
        TextFormat  = aTextFormat;
    }

    /// <summary>
    /// テキスト色
    /// </summary>
    [JsonInclude]
    public FormatColor TextColor { get; set; } = new();

    /// <summary>
    /// テキスト書式
    /// </summary>
    [JsonInclude]
    public CanvasTextFormat TextFormat { get; set; } = new()
    {
        FontFamily          = ConfigLib.Media.DefaultFontFamily,
        FontSize            = 8,
        HorizontalAlignment = CanvasHorizontalAlignment.Center,
        VerticalAlignment   = CanvasVerticalAlignment.Center,
    };
}
