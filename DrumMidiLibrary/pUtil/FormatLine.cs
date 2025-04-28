using System.Text.Json.Serialization;
using Windows.UI;

namespace DrumMidiLibrary.pUtil;

/// <summary>
/// グラフィックアイテム：線
/// </summary>
public class FormatLine
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public FormatLine()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aLineColor"></param>
    /// <param name="aLineSize"></param>
    public FormatLine( Color aLineColor, float aLineSize )
    {
        LineColor   = new( aLineColor );
        LineSize    = aLineSize;
    }

    /// <summary>
    /// ライン色
    /// </summary>
    [JsonInclude]
    public FormatColor LineColor { get; set; } = new();

    /// <summary>
    /// ラインサイズ
    /// </summary>
    [JsonIgnore]
    private float _LineSize = 1;

    /// <summary>
    /// ラインサイズ
    /// </summary>
    [JsonInclude]
    public float LineSize 
    { 
        get => _LineSize;
        set => _LineSize = ( value < 0 ) ? 0 : value ;
    }
}
