using Microsoft.Graphics.Canvas.Text;
using System.Collections.Generic;
using System.Numerics;
using System.Text.Json.Serialization;
using Windows.UI;

namespace DrumMidiEditorApp.pConfig;

/// <summary>
/// イコライザ設定
/// </summary>
public class ConfigEqualizer
{
    /// <summary>
    /// イコライザON/OFF
    /// </summary>
    [JsonIgnore]
    public bool EqualizerOn { get; set; } = true;

    /// <summary>
    /// 背景色色
    /// </summary>
    [JsonInclude]
    public Color BackGroundColor { get; set; } = Color.FromArgb( 255, 0, 0, 0 );

    #region Text

    /// <summary>
    /// テキスト書式（右寄せ）
    /// </summary>
    [JsonIgnore]
    public CanvasTextFormat TextFormatRight { get; set; } = new()
    {
        FontFamily          = "system-ui",
        FontSize            = 12,
        HorizontalAlignment = CanvasHorizontalAlignment.Right,
    };

    /// <summary>
    /// テキスト書式（中央寄せ）
    /// </summary>
    [JsonIgnore]
    public CanvasTextFormat TextFormatCenter { get; set; } = new()
    {
        FontFamily          = "system-ui",
        FontSize            = 12,
        HorizontalAlignment = CanvasHorizontalAlignment.Center,
    };

    /// <summary>
    /// 文字色
    /// </summary>
    [JsonInclude]
    public Color TextColor { get; set; } = Color.FromArgb( 255, 100, 200, 100 );

    #endregion

    #region Line

    /// <summary>
    /// ライン色
    /// </summary>
    [JsonInclude]
    public Color LineColor { get; set; } = Color.FromArgb( 255, 30, 30, 30 );

    #endregion

    #region Point

    /// <summary>
    /// イコライザ入力：外枠ペン
    /// </summary>
    [JsonInclude]
    public Color PointColor { get; set; } = Color.FromArgb( 255, 0, 100, 255 );

    /// <summary>
    /// イコライザ入力：選択背景色
    /// </summary>
    [JsonInclude]
    public Color PointSelectColor { get; set; } = Color.FromArgb( 255, 100, 200, 100 );

    /// <summary>
    /// イコライザ入力：非選択背景色
    /// </summary>
    [JsonInclude]
    public Color PointNonSelectColor { get; set; } = Color.FromArgb( 255, 0, 100, 255 );

    /// <summary>
    /// イコライザ入力：サイズ
    /// </summary>
    [JsonInclude]
    public float PointSize { get; set; } = 10F;

    #endregion

    #region Layout

    /// <summary>
    /// イコライザ X/Y軸左上余白
    /// </summary>
    [JsonInclude]
    public Vector2 EqulizerBodyMarginLeftTop { get; set; } = new( 50F, 20F );

    /// <summary>
    /// イコライザ X/Y軸右下余白
    /// </summary>
    [JsonInclude]
    public Vector2 EqulizerBodyMarginRightBottom { get; set; } = new( 20F, 50F );

    /// <summary>
    /// ラベル表示：X軸下側余白
    /// </summary>
    [JsonInclude]
    public float XLineTitlePaddingBottom { get; set; } = 20F;

    /// <summary>
    /// ラベル表示：Y軸左側の余白
    /// </summary>
    [JsonInclude]
    public float YLineTitlePaddingRight { get; set; } = 10F;

    #endregion

    #region Db

    /// <summary>
    /// Y軸：分割数
    /// </summary>
    [JsonInclude]
    public int DbGainSeparateHeightCount { get; set; } = 7;

    /// <summary>
    /// Y軸：１メモリの高さ
    /// </summary>
    [JsonInclude]
    public float DbGainSeparateHeightTermSize { get; set; } = 30F;

    /// <summary>
    /// Y軸：Db増減最小値
    /// </summary>
    [JsonInclude]
    public float DbGainMin { get; set; } = -60F;

    /// <summary>
    /// Y軸：Db増減最大値
    /// </summary>
    [JsonInclude]
    public float DbGainMax { get; set; } = 10F;

    /// <summary>
    /// Y軸：高さ合計
    /// </summary>
    /// <returns>Y軸高さ</returns>
    public float GetDbTotalHeight() => DbGainSeparateHeightCount * DbGainSeparateHeightTermSize;

    /// <summary>
    /// Y軸：１メモリ分のDbGain
    /// </summary>
    [JsonIgnore]
    public float DbGainSeparate => ( DbGainMax - DbGainMin ) / DbGainSeparateHeightCount;

    #endregion

    #region Hz

    /// <summary>
    /// X軸：Hzリスト
    /// </summary>
    [JsonInclude]
    public readonly List<(float Hz, string LabelName, float Width)> HzList = new()
        {
            new(    20,  "20",   0 ),
            new(    50,  "50",  15 ),
            new(   100, "100",  25 ),
            new(   200, "200",  30 ),
            new(   500, "500",  40 ),
            new(   800, "800",  50 ),
            new(  1000,  "1K",  60 ),
            new(  2000,  "2K", 120 ),
            new(  4000,  "5K", 100 ),
            new( 10000, "10K",  70 ),
            new( 20000, "20K",  70 ),
        };

    /// <summary>
    /// X軸：Hz横幅合計
    /// </summary>
    /// <returns>X軸横幅</returns>
    public float GetHzTotalWidth()
    {
        float width = 0;

        foreach ( var ( Hz, LabelName, Width ) in HzList )
        {
            width += Width;
        }
        return width;
    }

    #endregion

    #region WaveForm

    /// <summary>
    /// 波形表示ON/OFF
    /// </summary>
    [JsonInclude]
    public bool WaveFormOn { get; set; } = false;

    /// <summary>
    /// 波形描画間隔（秒）
    /// </summary>
    [JsonInclude]
    public float WaveFormDrawInterval { get; set; } = 1;

    /// <summary>
    /// 波形：左チャンネルペン
    /// </summary>
    [JsonInclude]
    public Color WaveLeftColor { get; set; } = Color.FromArgb( 255, 255, 0, 0 );

    /// <summary>
    /// 波形：右チャンネルペン
    /// </summary>
    [JsonInclude]
    public Color WaveRightColor { get; set; } = Color.FromArgb( 255, 0, 255, 0 );

    #endregion
}
