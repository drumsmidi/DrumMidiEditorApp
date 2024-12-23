using System.Collections.Generic;
using System.Text.Json.Serialization;
using DrumMidiEditorApp.pUtil.pFormat;
using Microsoft.Graphics.Canvas.Text;
using Windows.Foundation;
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
    public FormatColor BackGround
    {
        get; set;
    } = new()
    {
        Color = Color.FromArgb( 255, 0, 0, 0 ),
    };

    #region Text

    /// <summary>
    /// テキスト書式（右寄せ）
    /// </summary>
    [JsonInclude]
    public FormatRect TextRightRect
    {
        get; set;
    } = new()
    {
        Text = new( Color.FromArgb( 255, 100, 200, 100 ),
                        new()
                        {
                            FontFamily          = "system-ui",
                            FontSize            = 12,
                            HorizontalAlignment = CanvasHorizontalAlignment.Right,
                            VerticalAlignment   = CanvasVerticalAlignment.Center,
                        } ),
    };

    /// <summary>
    /// テキスト書式（中央寄せ）
    /// </summary>
    [JsonInclude]
    public FormatRect TextCenterRect
    {
        get; set;
    } = new()
    {
        Text = new( Color.FromArgb( 255, 100, 200, 100 ),
                        new()
                        {
                            FontFamily          = "system-ui",
                            FontSize            = 12,
                            HorizontalAlignment = CanvasHorizontalAlignment.Center,
                            VerticalAlignment   = CanvasVerticalAlignment.Center,
                        } ),
    };

    #endregion

    #region Line

    /// <summary>
    /// ライン色
    /// </summary>
    [JsonInclude]
    public FormatLine Line
    {
        get; set;
    } = new()
    {
        LineColor   = new( Color.FromArgb( 255, 30, 30, 30 ) ),
        LineSize    = 1,
    };

    #endregion

    #region Point

    /// <summary>
    /// イコライザ入力：選択背景色
    /// </summary>
    [JsonInclude]
    public FormatRect PointSelectRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 100, 200, 100 ) ),
        Line        = new( Color.FromArgb( 255, 0, 100, 255 ), 1F ),
    };

    /// <summary>
    /// イコライザ入力：非選択背景色
    /// </summary>
    [JsonInclude]
    public FormatRect PointNonSelectRect
    {
        get; set;
    } = new()
    {
        Background  = new( Color.FromArgb( 255, 0, 100, 255 ) ),
        Line        = new( Color.FromArgb( 255, 0, 100, 255 ), 1F ),
    };

    /// <summary>
    /// イコライザ入力：サイズ
    /// </summary>
    [JsonInclude]
    public float PointSize { get; set; } = 14F;

    #endregion

    #region Layout

    /// <summary>
    /// イコライザ X/Y軸左上余白
    /// </summary>
    [JsonInclude]
    public Point EqulizerBodyMarginLeftTop { get; set; } = new( 50F, 20F );

    /// <summary>
    /// イコライザ X/Y軸右下余白
    /// </summary>
    [JsonInclude]
    public Point EqulizerBodyMarginRightBottom { get; set; } = new( 20F, 50F );

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
    [JsonIgnore]
    public float DbTotalHeight => DbGainSeparateHeightCount * DbGainSeparateHeightTermSize;

    /// <summary>
    /// Y軸：１メモリ分のDbGain
    /// </summary>
    [JsonIgnore]
    public float DbGainSeparate => ( DbGainMax - DbGainMin ) / DbGainSeparateHeightCount;

    #endregion

    #region Hz

    /// <summary>
    /// X軸：Hzアイテム
    /// </summary>
    /// <remarks>
    /// コンストラクタ
    /// </remarks>
    /// <param name="aHz">Hz値</param>
    /// <param name="aLabelName">ラベル名称</param>
    /// <param name="aWidth">横幅</param>
    public class HzItem( int aHz, string aLabelName, int aWidth )
    {
        /// <summary>
        /// Hz値
        /// </summary>
        [JsonInclude]
        public int Hz
        {
            get; set;
        } = aHz;

        /// <summary>
        /// ラベル名称
        /// </summary>
        [JsonInclude]
        public string LabelName
        {
            get; set;
        } = aLabelName;

        /// <summary>
        /// 横幅
        /// </summary>
        [JsonInclude]
        public int Width
        {
            get; set;
        } = aWidth;
    }

    /// <summary>
    /// X軸：Hzリスト
    /// </summary>
    [JsonInclude]
    public readonly List<HzItem> HzList =
        [
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
        ];

    /// <summary>
    /// X軸：Hz横幅合計
    /// </summary>
    /// <returns>X軸横幅</returns>
    public float GetHzTotalWidth()
    {
        float width = 0;

        foreach ( var item in HzList )
        {
            width += item.Width;
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
    public double WaveFormDrawInterval { get; set; } = 0.5;

    /// <summary>
    /// 波形：左チャンネルペン
    /// </summary>
    [JsonInclude]
    public FormatLine WaveLeftLine
    {
        get; set;
    } = new()
    {
        LineColor   = new( Color.FromArgb( 100, 255, 0, 0 ) ),
        LineSize    = 0.4F,
    };

    /// <summary>
    /// 波形：右チャンネルペン
    /// </summary>
    [JsonInclude]
    public FormatLine WaveRightLine
    {
        get; set;
    } = new()
    {
        LineColor   = new( Color.FromArgb( 100, 0, 255, 0 ) ),
        LineSize    = 0.4F,
    };

    #endregion
}
