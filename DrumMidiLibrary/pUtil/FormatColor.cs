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
    /// コンストラクタ
    /// </summary>
    public FormatColor()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aColor">初期化する色</param>
    public FormatColor( Color aColor )
    {
        Color = aColor;
    }

    /// <summary>
    /// 色
    /// </summary>
    [JsonInclude]
    public Color Color { get; set; } = HelperColor.EmptyColor;

    /// <summary>
    /// 色を文字列形式で取得または設定
    /// </summary>
    [JsonIgnore]
    public string ColorText
    {
        get => HelperColor.GetColorText( Color );
        set => Color = HelperColor.GetColor( value );
    }

    /// <summary>
    /// 色をブラシ形式で取得または設定
    /// </summary>
    [JsonIgnore]
    public Brush ColorBrush
    {
        get => HelperColor.GetColorBrush( Color );
        set => Color = HelperColor.GetColor( value );
    }
}
