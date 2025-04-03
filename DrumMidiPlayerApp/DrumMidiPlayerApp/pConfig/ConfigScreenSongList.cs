using System.Text.Json.Serialization;
using DrumMidiLibrary.pUtil;
using Microsoft.Graphics.Canvas.Text;

namespace DrumMidiPlayerApp.pConfig;

/// <summary>
/// 曲選択リスト設定
/// </summary>
public class ConfigScreenSongList
{
    /// <summary>
    /// 処理中表示：半径
    /// </summary>
    [JsonInclude]
    public float ProcessingRadius { get; set; } = 80F;

    /// <summary>
    /// SongListItem：書式
    /// </summary>
    [JsonInclude]
    public FormatRect ActiveFormatRect
    {
        get; set;
    } = new()
        {
            Background  = new( HelperColor.GetColor( "#AA66FFFF" ) ),
            Line        = new( HelperColor.GetColor( "#FF000000" ), 1.0F ),
            Text        = new( HelperColor.GetColor( "#FFFFFFFF" ),
                                new()
                                {
                                    FontFamily          = Config.Media.DefaultFontFamily,
                                    FontSize            = 36F,
                                    HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                    VerticalAlignment   = CanvasVerticalAlignment.Center,
                                } ),
        };

    /// <summary>
    /// SongListItem：書式
    /// </summary>
    [JsonInclude]
    public FormatRect NormalSelectFormatRect
    {
        get; set;
    } = new()
        {
            Background  = new( HelperColor.GetColor( "#AA666666" ) ),
            Line        = new( HelperColor.GetColor( "#FF000000" ), 1.0F ),
            Text        = new( HelperColor.GetColor( "#FFFFFFFF" ),
                                new()
                                {
                                    FontFamily          = Config.Media.DefaultFontFamily,
                                    FontSize            = 36F,
                                    HorizontalAlignment = CanvasHorizontalAlignment.Left,
                                    VerticalAlignment   = CanvasVerticalAlignment.Center,
                                } ),
        };

    /// <summary>
    /// SongListItem：書式
    /// </summary>
    [JsonInclude]
    public FormatLine StartFormatLine
    {
        get; set;
    } = new( HelperColor.GetColor( "#FF0000FF" ), 1.0F );

}
