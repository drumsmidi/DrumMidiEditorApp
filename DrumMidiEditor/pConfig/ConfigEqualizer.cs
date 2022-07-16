using System.Collections.Generic;
using System.Drawing;
using System.Text.Json.Serialization;

using Font = System.Drawing.Font;

namespace DrumMidiEditor.pConfig;

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

    #region String

    /// <summary>
    /// 文字列書式（右寄せ）
    /// </summary>
    [JsonIgnore]
    public StringFormat StrFmtRight { get; set; } = new()
    {
        Alignment       = StringAlignment.Far,
        LineAlignment   = StringAlignment.Center
    };

    /// <summary>
    /// 文字列書式（中央寄せ）
    /// </summary>
    [JsonIgnore]
    public StringFormat StrFmtCenter { get; set; } = new()
    {
        Alignment       = StringAlignment.Center,
        LineAlignment   = StringAlignment.Center
    };

    /// <summary>
    /// 文字フォント
    /// </summary>
    [JsonInclude]
    public Font StrFont { get; set; } = new( FontFamily.GenericSerif, 8F );

    /// <summary>
    /// 文字色
    /// </summary>
    [JsonInclude]
    public SolidBrush StrBrush { get; set; } = new( Color.FromArgb( 255, 100, 200, 100 ) );

    #endregion

    #region Line

    /// <summary>
    /// ラインペン
    /// </summary>
    [JsonInclude]
    public Pen LinePen { get; set; } = new( Color.FromArgb( 255, 30, 30, 30 ), 1F );

    #endregion

    #region Point

    /// <summary>
    /// イコライザ入力：外枠ペン
    /// </summary>
    [JsonInclude]
    public Pen PointPen { get; set; } = new( Color.FromArgb( 255, 0, 100, 255 ), 1F );

    /// <summary>
    /// イコライザ入力：選択背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush PointSelectBrush { get; set; } = new( Color.FromArgb( 255, 100, 200, 100 ) );

    /// <summary>
    /// イコライザ入力：非選択背景色
    /// </summary>
    [JsonInclude]
    public SolidBrush PointNonSelectBrush { get; set; } = new( Color.FromArgb( 255, 0, 100, 255 ) );

    /// <summary>
    /// イコライザ入力：サイズ
    /// </summary>
    [JsonInclude]
    public int PointSize { get; set; } = 10;

    #endregion

    #region Size

    /// <summary>
    /// イコライザ X/Y軸左上余白
    /// </summary>
    [JsonInclude]
    public Point EqulizerPaddingLeftTop { get; set; } = new( 50, 20 );

    /// <summary>
    /// ラベル表示：X軸下側余白
    /// </summary>
    [JsonInclude]
    public int XLineTitlePaddingBottom { get; set; } = 20;

    /// <summary>
    /// ラベル表示：Y軸左側の余白
    /// </summary>
    [JsonInclude]
    public int YLineTitlePaddingRight { get; set; } = 10;

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
    public int DbGainSeparateHeightTermSize { get; set; } = 30;

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
    public int GetDbTotalHeight() => DbGainSeparateHeightCount * DbGainSeparateHeightTermSize;

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
    public class HzItem
    {
        /// <summary>
        /// Hz値
        /// </summary>
        public int Hz { get; set; }

        /// <summary>
        /// ラベル名称
        /// </summary>
        public string LabelName { get; set; }

        /// <summary>
        /// 横幅
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="aHz">Hz値</param>
        /// <param name="aLabelName">ラベル名称</param>
        /// <param name="aWidth">横幅</param>
        public HzItem( int aHz, string aLabelName, int aWidth )
        {
            Hz          = aHz;
            LabelName   = aLabelName;
            Width       = aWidth;
        }
    }

    /// <summary>
    /// X軸：Hzリスト
    /// </summary>
    [JsonInclude]
    public readonly List<HzItem> HzList = new()
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
    public int GetHzTotalWidth()
    {
        int width = 0;

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
    public bool WaveFormOn { get; set; } = true;

    /// <summary>
    /// 波形描画間隔（ミリ秒）
    /// </summary>
    [JsonInclude]
    public int WaveFormDrawInterval { get; set; } = 1000;

    /// <summary>
    /// 波形：左チャンネルペン
    /// </summary>
    [JsonInclude]
    public Pen WaveLeftPen { get; set; } = new( Color.FromArgb( 255, 255, 0, 0 ), 1F );

    /// <summary>
    /// 波形：右チャンネルペン
    /// </summary>
    [JsonInclude]
    public Pen WaveRightPen { get; set; } = new( Color.FromArgb( 255, 0, 255, 0 ), 1F );

    #endregion
}
