using System.Text.Json.Serialization;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas.Text;

namespace DrumMidiPlayerApp.pConfig;

/// <summary>
/// スクリーン設定：オプション
/// </summary>
public class ConfigScreenOptionMain
{
    /// <summary>
    /// 背景：書式
    /// </summary>
    [JsonInclude]
    public FormatRect BackgroundRect
    {
        get; set;
    } = new()
        {
            Background  = new( HelperColor.GetColor( "#EE000066" ) ),
            Line        = new( HelperColor.GetColor( "#FF000000" ), 4F ),
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
