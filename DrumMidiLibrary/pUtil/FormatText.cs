﻿using System.Text.Json.Serialization;
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
    /// テキスト色
    /// </summary>
    [JsonInclude]
    public FormatColor TextColor = new();

    /// <summary>
    /// テキスト書式
    /// </summary>
    [JsonInclude]
    public CanvasTextFormat TextFormat
    {
        get; set;
    } = new()
        {
            FontFamily          = ConfigLib.Media.DefaultFontFamily,
            FontSize            = 8,
            HorizontalAlignment = CanvasHorizontalAlignment.Center,
            VerticalAlignment   = CanvasVerticalAlignment.Center,
        };

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public FormatText()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="aTextColor"></param>
    /// <param name="aTextFormat"></param>
    public FormatText( Color aTextColor, CanvasTextFormat aTextFormat )
    {
        TextColor   = new( aTextColor );
        TextFormat  = aTextFormat;
    }
}
