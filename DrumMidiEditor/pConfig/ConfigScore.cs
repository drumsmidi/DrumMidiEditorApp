using System.Drawing;
using System.Text.Json.Serialization;

namespace DrumMidiEditor.pConfig;

/// <summary>
/// Scoreタブ設定
/// </summary>
public class ConfigScore
{
    #region Update flag

    /// <summary>
    /// スコア更新フラグ
    /// </summary>
    [JsonIgnore]
    public bool UpdateScoreFlag { get; set; } = false;

    #endregion

    #region Sheet

    /// <summary>
    /// シート背景色
    /// </summary>
    [JsonInclude]
    public Color SheetColor { get; set; } = Color.Black;

    #endregion

    #region Measure

    /// <summary>
    /// 小節囲い線ペン
    /// </summary>
    [JsonInclude]
    public Pen MeasureLinePen { get; set; } = new( Color.FromArgb( 60, 60, 60 ), 1 );

    #endregion

    #region Note

    /// <summary>
    /// ノート横幅
    /// </summary>
    [JsonInclude]
    public int NoteWidthSize { get; set; } = 8;

    /// <summary>
    /// ノート高さ
    /// </summary>
    [JsonInclude]
    public int NoteHeightSize { get; set; } = 8;

    /// <summary>
    /// ノート音量０表示フラグ
    /// </summary>
    [JsonInclude]
    public bool NoteVolumeZeroOn { get; set; } = false;

    /// <summary>
    /// ノート音量サイズ表示フラグ
    /// </summary>
    [JsonInclude]
    public bool NoteVolumeSizeOn { get; set; } = true;

    #endregion
}
