using System.Drawing;
using System.Text.Json.Serialization;

using DrumMidiEditor.pGeneralFunction.pDirectX;

namespace DrumMidiEditor.pConfig;

/// <summary>
/// プレイヤー設定
/// </summary>
public class ConfigPlayerSimuration
{
    #region Bpm

    /// <summary>
    /// 現在のBPM値表示フラグ
    /// </summary>
    [JsonInclude]
    public bool BpmNowDisplay { get; set; } = true;

    /// <summary>
    /// BPMテキスト表示横幅
    /// </summary>
    [JsonInclude]
    public int BpmNowWidthSize { get; set; } = 60;

    /// <summary>
    /// BPM行の高さ
    /// </summary>
    [JsonInclude]
    public int BpmNowHeightSize { get; set; } = 36;

    /// <summary>
    /// 現在のBPM値描画アイテム
    /// </summary>
    [JsonInclude]
    public GraphicItem BpmNowGI { get; set; } = new()
    {
        BackBrush   = new( Color.FromArgb(   0,   0,   0,   0 ) ),
        ForePen     = new( Color.FromArgb( 255,  60,  60,  60 ), 0F ),
        FontBrush   = new( Color.FromArgb( 255, 100, 200, 100 ) ),
        Font        = new( FontFamily.GenericSerif, 18F ),
        FontFormat  = new()
            {
                Alignment       = StringAlignment.Center,
                LineAlignment   = StringAlignment.Center,
            }
    };

    #endregion

    #region Measure number

    /// <summary>
    /// 小節番号表示フラグ
    /// </summary>
    [JsonInclude]
    public bool MeasureNoDisplay { get; set; } = true;

    /// <summary>
    /// 小節番号行の横幅
    /// </summary>
    [JsonInclude]
    public int MeasureNoWidthSize { get; set; } = 50;

    /// <summary>
    /// 小節番号行の高さ
    /// </summary>
    [JsonInclude]
    public int MeasureNoHeightSize { get; set; } = 36;

    /// <summary>
    /// 小節番号描画アイテム
    /// </summary>
    [JsonInclude]
    public GraphicItem MeasureNoGI { get; set; } = new()
    {
        BackBrush   = new( Color.FromArgb(   0,   0,   0,   0 ) ),
        ForePen     = new( Color.FromArgb( 255,  60,  60,  60 ), 0F ),
        FontBrush   = new( Color.FromArgb( 255, 100, 200, 100 ) ),
        Font        = new( FontFamily.GenericSerif, 18F ),
        FontFormat  = new()
            {
                Alignment       = StringAlignment.Center,
                LineAlignment   = StringAlignment.Center,
            }
    };

    #endregion

    #region Header

    /// <summary>
    /// ヘッダーエフェクト
    /// </summary>
    [JsonInclude]
    public bool HeaderEffectOn { get; set; } = true;

    /// <summary>
    /// ヘッダー文字
    /// </summary>
    [JsonInclude]
    public bool HeaderStrOn { get; set; } = true;

    /// <summary>
    /// ヘッダー横幅
    /// </summary>
    [JsonInclude]
    public int HeaderSize { get; set; } = 80;

    /// <summary>
    /// ヘッダー描画アイテム
    /// </summary>
    [JsonInclude]
    public GraphicItem HeaderGI { get; set; } = new()
    {
        BackBrush   = new( Color.FromArgb(   0,   0,   0,   0 ) ),
        ForePen     = new( Color.FromArgb( 255,  60,  60,  60 ), 0.4F ),
        FontBrush   = new( Color.FromArgb( 255, 100, 200, 100 ) ),
        Font        = new( FontFamily.GenericSerif, 14F ),
        FontFormat  = new()
            {
                Alignment       = StringAlignment.Center,
                LineAlignment   = StringAlignment.Center,
            }
    };

    #endregion

    #region Note

    /// <summary>
    /// ノート間隔：横
    /// </summary>
    [JsonInclude]
    public int NoteTermSize { get; set; } = 2;

    /// <summary>
    /// １回の描画で描画する小節数
    /// </summary>
    [JsonInclude]
    public int DrawMeasureCount { get; set; } = 10;

    #endregion

    /// <summary>
    /// １小節の横幅
    /// </summary>
    public int MeasureSize => NoteTermSize * Config.System.MeasureNoteNumber;
}
