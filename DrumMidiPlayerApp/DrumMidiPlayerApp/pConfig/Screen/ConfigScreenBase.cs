using System.Text.Json.Serialization;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas.Text;

namespace DrumMidiPlayerApp.pConfig;

/// <summary>
/// プレイヤー設定
/// </summary>
public class ConfigScreenBase
{
    /// <summary>
    /// 処理中表示：半径
    /// </summary>
    [JsonInclude]
    public float ProcessingRadius { get; set; } = 100F;

    /// <summary>
    /// 処理中表示：書式
    /// </summary>
    [JsonInclude]
    public FormatRect ProcessingRect
    {
        get; set;
    } = new()
        {
            Background  = new( HelperColor.GetColor( "#AA999999" ) ),
            Line        = new( HelperColor.GetColor( "#00000000" ), 0F ),
            Text        = new( HelperColor.GetColor( "#AA999999" ),
                                new()
                                {
                                    FontFamily          = Config.Media.DefaultFontFamily,
                                    FontSize            = 14F,
                                    HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                    VerticalAlignment   = CanvasVerticalAlignment.Center,
                                } ),
        };
}
