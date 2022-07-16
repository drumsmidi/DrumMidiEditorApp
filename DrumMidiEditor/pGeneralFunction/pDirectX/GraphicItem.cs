using System.Drawing;
using System.Text.Json.Serialization;

namespace DrumMidiEditor.pGeneralFunction.pDirectX;

/// <summary>
/// グラフィックアイテム
/// </summary>
public class GraphicItem
{
    /// <summary>
    /// 背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush BackBrush = new( Color.Empty );

    /// <summary>
    /// 外枠ペン
    /// </summary>
    [JsonInclude]
    public Pen ForePen = new( Brushes.White, 0 );

    /// <summary>
    /// フォント
    /// </summary>
    [JsonInclude]
    public Font Font = new( FontFamily.GenericSerif, 1 );

    /// <summary>
    /// 文字色
    /// </summary>
    [JsonInclude]
    public SolidBrush FontBrush = new( Color.Empty );

    /// <summary>
    /// フォント書式
    /// </summary>
    [JsonInclude]
    public StringFormat FontFormat = new();
}
