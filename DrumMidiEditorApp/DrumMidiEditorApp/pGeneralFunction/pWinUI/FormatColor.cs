using Microsoft.UI.Xaml.Media;
using System.Text.Json.Serialization;
using Windows.UI;

namespace DrumMidiEditorApp.pGeneralFunction.pWinUI;

/// <summary>
/// グラフィックアイテム：色
/// </summary>
public class FormatColor
{
    /// <summary>
    /// 色
    /// </summary>
    [JsonInclude]
    public Color Color = ColorHelper.EmptyColor;

    /// <summary>
    /// 色
    /// </summary>
    [JsonIgnore]
    public string ColorText
    {
        get => ColorHelper.GetColor( Color );
        set => Color = ColorHelper.GetColor( value );
    }

    /// <summary>
    /// 色
    /// </summary>
    [JsonIgnore]
    public Brush ColorBrush
    {
        get => new SolidColorBrush( Color );
        set => Color = ( value as SolidColorBrush )?.Color ?? ColorHelper.EmptyColor ;
    }


}
