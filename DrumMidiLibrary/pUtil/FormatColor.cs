using System.Text.Json.Serialization;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// グラフィックアイテム：色
/// </summary>
public class FormatColor
{
    /// <summary>
    /// 色
    /// </summary>
    [JsonInclude]
    public Color Color = HelperColor.EmptyColor;

    /// <summary>
    /// 色
    /// </summary>
    [JsonIgnore]
    public string ColorText
    {
        get => HelperColor.GetColor( Color );
        set => Color = HelperColor.GetColor( value );
    }

    /// <summary>
    /// 色
    /// </summary>
    [JsonIgnore]
    public Brush ColorBrush
    {
        get => new SolidColorBrush( Color );
        set => Color = ( value as SolidColorBrush )?.Color ?? HelperColor.EmptyColor;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public FormatColor()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aColor"></param>
    public FormatColor( Color aColor )
    {
        Color = aColor;
    }
}
